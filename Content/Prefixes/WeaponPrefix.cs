using HonKaiImpact.Content.DamageTypes;

using Terraria;
using Terraria.ModLoader;

namespace HonKaiImpact.Content.Prefixes
{
	public abstract class WeaponPrefix : ModPrefix, ILocalizedModType
	{
		public virtual float RarityMult => Utils.s_PrefixRareLvNormal;

		public override PrefixCategory Category => PrefixCategory.AnyWeapon;

		public override void ModifyValue(ref float RefValueMult)
		{
			RefValueMult = RarityMult;
		}
	}

	public class FinalityPrefix : WeaponPrefix
	{
		public override float RarityMult => Utils.s_PrefixRareLvAddTwo;

		public override bool CanRoll(Item InItem)
		{
			return InItem.CountsAsClass<ImaginaryDamageClass>();
		}

		public override float RollChance(Item InItem)
		{
			return 14;
		}

		public override void Apply(Item InItem)
		{
			InItem.damage *= 15;
			InItem.crit = 100;
		}
	}
}
