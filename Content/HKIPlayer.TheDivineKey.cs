using HonKaiImpact.Cooldowns;

using Terraria;
using Terraria.DataStructures;

namespace HonKaiImpact.Content
{
	public partial class HKIPlayer
	{
		/// <summary>免死一次。</summary>
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (!FenghuangDownReviveReady)
			{
				Player.statLife = Player.statLifeMax2;
				Player.immune = true;
				Player.immuneTime = c_FenghuangDownReviveImmuneTicks;

				playSound = false;
				genGore = false;

				Player.AddCooldown(Cooldowns.FenghuangDown.Name, FenghuangDownCooldownTicks);

				return false;
			}

			

			return true;
		}

		private void ResetDivineKeys()
		{
			bHeldDivineKey = false;
			bFenghuangDown = false;
		}

		#region 意识之键 / 羽渡尘的效果
		public bool bFenghuangDown;

		public bool SentienceCoreBoost => SnapshotCoreStatus.bSentience || SnapshotCoreStatus.bFinality;

		private bool FenghuangDownReviveReady => bFenghuangDown && Player.HasCooldown(Cooldowns.FenghuangDown.Name);

		private int FenghuangDownCooldownTicks => SentienceCoreBoost ? c_FenghuangDownCooldownBaseTicks / 2 : c_FenghuangDownCooldownBaseTicks;

		private const int c_FenghuangDownCooldownBaseTicks = 120 * 60;

		private const int c_FenghuangDownReviveImmuneTicks = 120;
		#endregion
	}
}
