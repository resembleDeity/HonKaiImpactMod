using Nameless.GameSystem;

using Terraria;
using Terraria.ModLoader;

namespace HonKaiImpact.Content.DamageTypes
{
	internal class ImaginaryDamageClass : DamageClass
	{
		public override void Load() => s_Instance = this;
		public override void Unload() => s_Instance = null;

		public override bool GetEffectInheritance(DamageClass InDamageClass) => true;

		public override StatInheritanceData GetModifierInheritance(DamageClass InDamageClass) => StatInheritanceData.Full;

		internal static ImaginaryDamageClass s_Instance;
	}

	internal class NpcDamageOverride : NpcOverride
	{
		public override bool? On_ModifyIncomingHit(NPC InNpc, ref NPC.HitModifiers RefModifiers)
		{
			if (RefModifiers.DamageType == ImaginaryDamageClass.s_Instance)
			{
				// RefModifiers.SourceDamage *= 10;
				return false;
			}

			return base.On_ModifyIncomingHit(InNpc, ref RefModifiers);
		}

		public override int TargetId => -1;
	}
}
