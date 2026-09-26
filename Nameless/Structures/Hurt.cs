using Terraria.DataStructures;

namespace Nameless.Structures
{
	public struct HurtContext
	{
		public PlayerDeathReason DamageSource;
		public int Damage;
		public int HitDirection;
		public bool bPvp;
		public bool bQuiet;
		public int CooldownCounter;
		public bool bDodgeable;
		public float ArmorPenetration;
		public float ScalingArmorPenetration;
		public float Knockback;
	}
}
