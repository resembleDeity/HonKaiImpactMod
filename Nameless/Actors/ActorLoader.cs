using Nameless.Concurrent;
using Nameless.GameSystem;
using Nameless.Structures;

using Terraria;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nameless.Actors
{
	public class ActorLoader : ModSystem, INamelessLoader
	{
		#region MainOverride-ModSystem

		public override void OnWorldLoad()
		{
			Actors = new Actor[MaxActorCount];
			Reset();
		}

		public override void OnWorldUnload()
		{
			if (Actors is not null)
			{
				for (int i = 0; i < MaxActorCount; i++)
				{
					if (Actors[i] is not null)
					{
						Actors[i].bActive = false;
						Actors[i].ActiveIndex = -1;
						Actors[i] = null;
					}
				}
			}

			Reset();

			NamelessParallel.ClearDeferred();
		}

		public override void PostUpdateEverything()
		{
			if (Actors is null)
			{
				return;
			}

			s_MouseRect = Main.MouseWorld.GetRectangle(1);

			if (!NamelessParallel.CanParallel())
			{
				foreach (var actor in s_ActiveActors)
				{
					UpdateActor(actor);
				}
			}
			else
			{
				try
				{
					ClassifyActors();

					NamelessParallel.BeginParallel();
					try
					{
						NamelessParallel.ExecuteBatch(s_ParallelActors, UpdateActor);
						NamelessParallel.EndParallel();

						foreach (var actor in s_SequentialActors)
						{
							UpdateActor(actor);
						}
					}
					finally
					{
						NamelessParallel.EndParallel();
						try
						{
							NamelessParallel.ExecuteDeferred();
						}
						catch (Exception ex)
						{
							NamelessMod.ParallelError("[Actor]", $"Execute after parallel update failed: {ex}");
						}
					}
				}
				catch (Exception ex)
				{
					NamelessMod.ParallelError("[Actor]", $"Parallel update failed, disabled and fell back to sequential: {ex}");
				}
			}
		}

		#endregion

		#region InterfaceOverride-INamelessLoader

		void INamelessLoader.LoadData()
		{
			Actors = new Actor[MaxActorCount];
			Reset();

			OnSpawnHooks = Utils.CreateHookCache<Action<Actor>>(selector => selector.OnSpawn);
			PreAIHooks = Utils.CreateHookCache<Func<Actor, bool>>(selector => selector.PreAI);
			PostAIHooks = Utils.CreateHookCache<Action<Actor>>(selector => selector.PostAI);
			PreDrawHooks = Utils.CreateHookCache<Func<Actor, SpriteBatch, Color, bool>>(selector => selector.PreDraw);
			PostDrawHooks = Utils.CreateHookCache<Action<Actor, SpriteBatch, Color>>(selector => selector.PostDraw);
		}

		void INamelessLoader.UnloadData()
		{
			Actors = null;
			Reset();

			OnSpawnHooks = null;
			PreAIHooks = null;
			PostAIHooks = null;

			ActorFactory.Clear();

			GlobalActor.Instances?.Clear();
		}

		#endregion

		#region API

		public static int NewActor<T>(Vector2 InPosition, Vector2 InVelocity = default) where T : Actor
			=> NewActor(GetTypeId<T>(), InPosition, InVelocity);

		public static int NewActor(int InType, Vector2 InPosition, Vector2 InVelocity = default)
		{
			if (NamelessParallel.IsInParallel())
			{
				NamelessParallel.DeferCommand(() => NewActor(InType, InPosition, InVelocity));
				return -1;
			}

			int slot = GetSlot();
			if (slot == -1)
			{
				return -1;
			}

			Actor actor = Instantiate(InType, slot);
			actor.Transform.Position = InPosition;
			actor.Velocity = InVelocity;

			actor.OnSpawn();

			foreach (var global in OnSpawnHooks.Enumerate())
			{
				global.OnSpawn(actor);
			}

			return slot;
		}

		public static void DrawActors(SpriteBatch InSpriteBatch, ERenderLayer InLayer = ERenderLayer.BeforeInfernoRings)
		{
			if (Main.dedServ)
			{
				return;
			}

			foreach (var actor in s_ActiveActors)
			{
				if (actor is null || !actor.bActive || actor.RenderLayer != InLayer)
				{
					continue;
				}

				Color drawColor = Lighting.GetColor(
					(int)(actor.Transform.Position.X / 16.0f),
					(int)(actor.Transform.Position.Y / 16.0f)
				);

				bool bDraw = true;
				foreach (var global in PreDrawHooks.Enumerate())
				{
					if (!global.PreDraw(actor, InSpriteBatch, drawColor))
					{
						bDraw = false;
					}
				}

				if (bDraw && actor.PreDraw(InSpriteBatch, ref drawColor))
				{
					if (!actor.Draw(InSpriteBatch, ref drawColor))
					{
						DrawError(InSpriteBatch, actor, drawColor);
					}
				}

				actor.PostDraw(InSpriteBatch, drawColor);
				foreach (var global in PostDrawHooks.Enumerate())
				{
					global.PostDraw(actor, InSpriteBatch, drawColor);
				}
			}
		}

		#endregion

		#region PrivateStatic

		private static int GetTypeId<T>() where T : Actor => Actor.IdByType[typeof(T)];
		private static int GetTypeId(Type InType) => Actor.IdByType[InType];

		private static void DrawError(SpriteBatch InSpriteBatch, Actor InActor, Color InDrawColor)
		{
			Rectangle frame = new(0, 0, InActor.Width, InActor.Height);
			InSpriteBatch.Draw(NamelessAsset.ErrorPlaceholder.Value, 
				InActor.Transform.Position - Main.screenPosition,
				frame, InDrawColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f
			);
		}

		private static void RegisterActive(Actor InActor)
		{
			InActor.ActiveIndex = s_ActiveActors.Count;
			s_ActiveActors.Add(InActor);
		}

		private static void UnregisterActive(Actor InActor)
		{
			int index = InActor.ActiveIndex;
			if (index < 0 || index >= s_ActiveActors.Count || s_ActiveActors[index] != InActor)
			{
				InActor.ActiveIndex = -1;
				return;
			}

			int lastIndex = s_ActiveActors.Count - 1;
			Actor moved = s_ActiveActors[lastIndex];
			s_ActiveActors[index] = moved;
			moved.ActiveIndex = index;
			s_ActiveActors.RemoveAt(lastIndex);
			InActor.ActiveIndex = -1;
		}

		private static void ClassifyActors()
		{
			s_SequentialActors.Clear();
			s_ParallelActors.Clear();
			foreach (var actor in s_ActiveActors)
			{
				if (actor is null || !actor.bActive)
				{
					continue;
				}

				if (actor.ExecutionPolicy == EExecutionPolicy.Parallel)
				{
					s_ParallelActors.Add(actor);
				}
				else
				{
					s_SequentialActors.Add(actor);
				}
			}
		}

		private static int GetSlot()
		{
			if (s_NextSlot < MaxActorCount)
			{
				return s_NextSlot++;
			}

			for (int i = 0; i < MaxActorCount; i++)
			{
				if (Actors[i] is null || !Actors[i].bActive)
				{
					return i;
				}
			}

			return -1;
		}

		private static void ResetSlot(Actor InActor)
		{
			InActor.bActive = false;
			UnregisterActive(InActor);

			int slot = InActor.WhoAmI;
			if (slot >= 0 && slot < MaxActorCount && Actors[slot] == InActor)
			{
				Actors[slot] = null;

				// TODO:... server
			}
		}

		private static void Reset()
		{
			s_NextSlot = 0;
			s_ActiveActors.Clear();
			s_SequentialActors.Clear();
			s_ParallelActors.Clear();
		}

		private static Actor Instantiate(int InTypeId, int InSlot)
		{
			Actor instance = Actor.InstanceById[InTypeId].First().Value.Clone();
			instance.Id = InTypeId;
			instance.WhoAmI = InSlot;
			instance.bActive = true;
			Actors[InSlot] = instance;
			RegisterActive(instance);
			return instance;
		}

		private static void UpdateActor(Actor InActor)
		{
			if (InActor is null || !InActor.bActive)
			{
				return;
			}

			try
			{
				bool bUpdate = true;
				foreach (var global in PreAIHooks.Enumerate())
				{
					if (!global.PreAI(InActor))
					{
						bUpdate = false;
					}
				}

				if (bUpdate)
				{
					InActor.AI();
					// InActor.Transform.Position += InActor.Velocity;
				}

				foreach (var global in PostAIHooks.Enumerate())
				{
					global.PostAI(InActor);
				}
			}
			catch (Exception ex)
			{
				NamelessMod.LogError($"Error updating actor {InActor.WhoAmI}: {ex}");

				if (NamelessParallel.IsInParallel())
				{
					NamelessParallel.DeferCommand(() => ResetSlot(InActor));
				}
				else
				{
					ResetSlot(InActor);
				}
			}
		}

		#endregion

		public static readonly int MaxActorCount = short.MaxValue;

		public static Actor[] Actors { get; private set; }



		internal static IReadOnlyList<Actor> ActiveActors => s_ActiveActors;

		internal static Dictionary<Type, Func<Actor>> ActorFactory { get; private set; } = [];

		internal static NamelessHookOverrideCache<GlobalActor> OnSpawnHooks;
		internal static NamelessHookOverrideCache<GlobalActor> PreAIHooks;
		internal static NamelessHookOverrideCache<GlobalActor> PostAIHooks;
		internal static NamelessHookOverrideCache<GlobalActor> PreDrawHooks;
		internal static NamelessHookOverrideCache<GlobalActor> PostDrawHooks;



		private static int s_NextSlot;

		private static readonly List<Actor> s_ActiveActors = [];

		private static readonly List<Actor> s_SequentialActors = new(256);
		private static readonly List<Actor> s_ParallelActors = new(256);

		private static Rectangle s_MouseRect;
	}
}
