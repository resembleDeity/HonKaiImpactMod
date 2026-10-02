using HonKaiImpact.Content.TheDivineKey.Sentience.Buffs;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace HonKaiImpact.Content
{
	public partial class HKIPlayer
	{
		public override void PostUpdate()
		{
			if (m_FenghuangDownCooldown > 0)
			{
				m_FenghuangDownCooldown--;
			}
		}

		public override void PostUpdateEquips()
		{
			if (!bFenghuangDown)
			{
				return;
			}

			Player.statLifeMax2 += (int)(Player.statLifeMax2 * FenghuangDownMaxLifeBonus);
		}

		public override void UpdateLifeRegen()
		{
			if (!bFenghuangDown)
			{
				return;
			}

			float lifePerSecond = FenghuangDownBaseLifeRegen + Player.statLifeMax2 * FenghuangDownMaxLifeRegen;

			Player.lifeRegen += (int)(lifePerSecond * 2f);
		}

		/// <summary>免死一次。</summary>
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (!FenghuangDownReviveReady)
			{
				return true;
			}

			Player.statLife = Player.statLifeMax2;
			Player.immune = true;
			Player.immuneTime = c_FenghuangDownReviveImmuneTicks;

			m_FenghuangDownCooldown = FenghuangDownCooldownTicks;

			playSound = false;
			genGore = false;

			return false;
		}

		private void ResetDivineKeys()
		{
			bHeldDivineKey = false;
			bFenghuangDown = false;
		}

		#region 意识之键 / 羽渡尘的效果
		public bool bFenghuangDown;

		public bool FenghuangDownCoolingDown => m_FenghuangDownCooldown > 0;

		private bool SentienceCoreBoost => SnapshotCoreStatus.bSentience || SnapshotCoreStatus.bFinality;

		private int FenghuangDownCooldownTicks => SentienceCoreBoost ? c_FenghuangDownCooldownBaseTicks / 2 : c_FenghuangDownCooldownBaseTicks;

		private bool FenghuangDownReviveReady => bFenghuangDown && m_FenghuangDownCooldown <= 0;

		private float FenghuangDownMaxLifeBonus => SentienceCoreBoost ? 1.0f : 0.5f;

		private float FenghuangDownBaseLifeRegen => SentienceCoreBoost ? 80.0f : 20.0f;

		private float FenghuangDownMaxLifeRegen => SentienceCoreBoost ? 0.2f : 0.1f;
		
		private int m_FenghuangDownCooldown;

		private const int c_FenghuangDownCooldownBaseTicks = 120 * 60;

		private const int c_FenghuangDownReviveImmuneTicks = 120;
		#endregion
	}
}
