using Nameless.GameSystem;
using Nameless.Structures;

using Terraria.DataStructures;

namespace HonKaiImpact.Content.Players
{
	internal class DeathOverride : PlayerOverride
	{
		public override bool On_Hurt(HurtContext InHurtContext)
		{
			return false;
		}

		public override bool? On_PreKill(double InDamage, int InHitDirection, bool InbPvp, ref bool RefbPlaySound, ref bool RefbGenDust, ref PlayerDeathReason RefDamageSource)
		{
			return false;
		}
	}
}
