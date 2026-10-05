using Nameless.Structures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Nameless.Actors
{
	public abstract class Actor : NamelessType<Actor>
	{
		public sealed override void NamelessSetup()
		{
			SetStaticDefaults();
		}

		protected sealed override void NamelessRegister()
		{
			Type type = GetType();
			Id = Instances.Count;
			Instances.Add(this);
			IdByType[type] = Id;
			InstanceById[Id] = new()
			{
				[type] = this,
			};

			try
			{
				ActorLoader.ActorFactory[type] = Expression.Lambda<Func<Actor>>(Expression.New(type)).Compile();
			}
			catch (Exception ex)
			{
				NamelessMod.LogWarn($"Actor factory compile failed for {type.FullName}, will fall back to Activator: {ex.Message}");
			}
		}

		public virtual void OnSpawn(params object[] InArgs)
		{

		}

		public virtual void AI()
		{

		}

		#region Draw

		public virtual bool PreDraw(SpriteBatch InSpriteBatch, ref Color RefDrawColor)
		{
			return true;
		}

		public virtual bool Draw(SpriteBatch InSpriteBatch, ref Color RefDrawColor)
		{
			return true;
		}

		public virtual void PostDraw(SpriteBatch InSpriteBatch, Color RefDrawColor)
		{
			
		}

		#endregion

		public Actor Clone()
		{
			Type type = GetType();
			if (ActorLoader.ActorFactory.TryGetValue(type, out var factory))
			{
				return factory();
			}

			return (Actor)Activator.CreateInstance(type);
		}

		public int Id;
		public int WhoAmI;

		public virtual EExecutionPolicy ExecutionPolicy => EExecutionPolicy.Sequential;

		public ERenderLayer RenderLayer = ERenderLayer.BeforeInfernoRings;

		[NetworkSync]
		public bool bActive;

		[NetworkSync]
		public int Width;

		[NetworkSync]
		public int Height;

		[NetworkSync]
		public TransformComponent Transform = new();

		[NetworkSync]
		public Vector2 Velocity;

		public Vector2 Size => new Vector2(Width, Height) * Transform.Scale;

		public virtual Rectangle Hitbox => Transform.Position.GetRectangle(Size);

		public virtual Vector2 Center => Transform.Position + Size / 2;

		public static Dictionary<Type, int> IdByType { get; private set; } = [];



		internal int ActiveIndex = -1;
	}
}
