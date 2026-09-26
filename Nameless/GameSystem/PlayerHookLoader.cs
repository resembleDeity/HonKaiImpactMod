using Nameless.Structures;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nameless.GameSystem
{
	public class PlayerHookLoader : ModPlayer, INamelessLoader
	{
		public override void SetStaticDefaults() => SetDefaults();

		public override void Initialize() => SetDefaults();

		void INamelessLoader.LoadData()
		{
			s_PlayerLoaderType = typeof(PlayerLoader);

			OnHitNpcMethod = s_PlayerLoaderType.GetUniquePublicStaticMethod("OnHitNPC");
			if (OnHitNpcMethod is not null)
			{
				NamelessHook.AddHook(OnHitNpcMethod, On_OnHitNpc_Hook);
			}

			HurtMethod = typeof(Player).GetPublicMethod<On_Hurt_Delegate>("Hurt");
			if (HurtMethod is not null)
			{
				NamelessHook.AddHook(HurtMethod, On_Hurt_Hook);
			}

			PreKillMethod = s_PlayerLoaderType.GetUniquePublicStaticMethod("PreKill");
			if (PreKillMethod is not null)
			{
				NamelessHook.AddHook(PreKillMethod, On_PreKill_Hook);
			}
		}

		void INamelessLoader.UnloadData()
		{
			ActiveOverrides?.Clear();
			PlayerOverrides?.Clear();
			PlayerOverride.Clear();

			s_PlayerLoaderType = null;
			OnHitNpcMethod = null;
			HurtMethod = null;
			PreKillMethod = null;
		}

		public override void PreUpdate() => ApplyOverrides();

		public void SetDefaults()
		{
			PlayerOverrides.Clear();
			foreach (var overrideInstance in PlayerOverride.Instances)
			{
				var newInstance = overrideInstance.Clone();
				newInstance.Owner = Player;
				newInstance.SetDefaults();
				PlayerOverrides.Add(newInstance.GetType(), newInstance);
			}

			ApplyOverrides();
		}

		public void ApplyOverrides()
		{
			ActiveOverrides.Clear();
			foreach (var (key, overrideInstance) in PlayerOverrides)
			{
				if (!overrideInstance.CanOverride())
				{
					continue;
				}

				ActiveOverrides.Add(key, overrideInstance);
			}
		}



		private static PlayerOverride[] SnapshotOverrides(Dictionary<Type, PlayerOverride> InOverrides) =>
			InOverrides is null || InOverrides.Count == 0 ? [] : [.. InOverrides.Values];
		

		private static void On_OnHitNpc_Hook(On_OnHitNpc_Delegate InOrig,
			Player InPlayer,
			NPC InNpc,
			in NPC.HitInfo InHit,
			int InDamageDone)
		{
			if (InPlayer.TryGetOverrides(out var overrides))
			{
				bool bResult = true;
				foreach (var overrideInstance in SnapshotOverrides(overrides))
				{
					if (!overrideInstance.On_OnHitNpc(InNpc, InHit, InDamageDone))
					{
						bResult = false;
					}
				}

				if (!bResult)
				{
					return;
				}
			}

			InOrig.Invoke(InPlayer, InNpc, InHit, InDamageDone);
		}

		private static double On_Hurt_Hook(On_Hurt_Delegate InOrig,
			Player InPlayer,
			PlayerDeathReason InDamageSource,
			int InDamage,
			int InHitDirection,
			out Player.HurtInfo OutInfo,
			bool InbPvp = false,
			bool InbQuiet = false,
			int InCooldownCounter = -1,
			bool InbDodgeable = true,
			float InArmorPenetration = 0,
			float InScalingArmorPenetration = 0,
			float InKnockback = 4.5f)
		{
			HurtContext context = new()
			{
				DamageSource = InDamageSource, Damage = InDamage, HitDirection = InHitDirection,
				bPvp = InbPvp, bQuiet = InbQuiet, CooldownCounter = InCooldownCounter, bDodgeable = InbDodgeable,
				ArmorPenetration = InArmorPenetration, ScalingArmorPenetration = InScalingArmorPenetration, Knockback = InKnockback
			};

			if (InPlayer.TryGetOverrides(out var overrides))
			{
				OutInfo = default;
				bool bResult = true;
				foreach(var overrideInstance in SnapshotOverrides(overrides))
				{
					if (!overrideInstance.On_Hurt(context))
					{
						bResult = false;
					}
				}

				if (!bResult)
				{
					return 0.0;
				}
			}

			return InOrig.Invoke(InPlayer,
				context.DamageSource, context.Damage, context.HitDirection, out OutInfo,
				context.bPvp, context.bQuiet, context.CooldownCounter, context.bDodgeable,
				context.ArmorPenetration, context.ScalingArmorPenetration, context.Knockback);
		}

		private static bool On_PreKill_Hook(On_PreKill_Delegate InOrig, 
			Player InPlayer, 
			double InDamage, 
			int InHitDirection, 
			bool InbPvp, 
			ref bool RefbPlaySound, 
			ref bool RefbGenGore, 
			ref PlayerDeathReason RefDamageSource)
		{
			if (InPlayer.TryGetOverrides(out var overrides))
			{
				bool? bResult = null;
				foreach (var overrideInstance in SnapshotOverrides(overrides))
				{
					bResult = overrideInstance.On_PreKill(InDamage, InHitDirection, InbPvp, ref RefbPlaySound, ref RefbGenGore, ref RefDamageSource);
				}
				if (bResult.HasValue)
				{
					return bResult.Value;
				}
			}

			return InOrig.Invoke(InPlayer, InDamage, InHitDirection, InbPvp, ref RefbPlaySound, ref RefbGenGore, ref RefDamageSource);
		}



		public Dictionary<Type, PlayerOverride> PlayerOverrides { get; private set; } = [];
		public Dictionary<Type, PlayerOverride> ActiveOverrides { get; private set; } = [];



		private static Type s_PlayerLoaderType;

		private static MethodBase OnHitNpcMethod;
		private delegate void On_OnHitNpc_Delegate(
			Player InPlayer, 
			NPC InNpc, 
			in NPC.HitInfo InHit, 
			int InDamageDone
		);

		private static MethodBase HurtMethod;
		private delegate double On_Hurt_Delegate(
			Player InPlayer, 
			PlayerDeathReason InDamageSource, 
			int InDamage, 
			int InHitDirection, 
			out Player.HurtInfo OutInfo, 
			bool InbPvp = false, 
			bool InbQuiet = false, 
			int InCooldownCounter = -1, 
			bool InbDodgeable = true, 
			float InArmorPenetration = 0, 
			float InScalingArmorPenetration = 0, 
			float InKnockback = 4.5f
		);

		private static MethodBase PreKillMethod;
		private delegate bool On_PreKill_Delegate(
			Player InPlayer, 
			double InDamage, 
			int InHitDirection, 
			bool InbPvp, 
			ref bool RefbPlaySound, 
			ref bool RefbGenGore, 
			ref PlayerDeathReason RefDamageSource
		);
	}
}
