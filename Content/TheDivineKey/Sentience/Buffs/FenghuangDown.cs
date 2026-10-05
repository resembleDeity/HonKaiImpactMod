using HonKaiImpact.Content.TheDivineKey.Sentience.Projectiles;

using Nameless;

using ReLogic.Content;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;

namespace HonKaiImpact.Content.TheDivineKey.Sentience.Buffs
{
	/// <summary>
	/// 意识之键召唤物的 buff —— 只负责「状态」与图标。
	/// <para/>
	/// 判定链：<see cref="SentienceDivineKey"/> 在背包/饰品栏 → 维持本 buff + 维持召唤物；
	/// 召唤物 <see cref="SentienceMinion"/> 靠本 buff 存在。饰品被取下 → buff 断 → 召唤物消失 → 效果同时失效。
	/// <para/>
	/// 具体机制（免死与冷却、最大生命值、生命回复）全部在 <see cref="HKIPlayer"/> 里；
	/// 本类只提供状态、说明文字，以及「冷却中换成冷却版图标」。
	/// </summary>
	public class FenghuangDown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player InPlayer, ref int RefBuffIndex)
		{
			HKIPlayer hkiPlayer = InPlayer.HKI();
			hkiPlayer.bFenghuangDown = true;
			m_CoreBoost = hkiPlayer.SentienceCoreBoost;

			InPlayer.statLifeMax2 += (int)(InPlayer.statLifeMax2 * MaxLifeBonus);

			float lifePerSecond = BaseLifeRegen + InPlayer.statLifeMax2 * MaxLifeRegen;
			InPlayer.lifeRegen += (int)(lifePerSecond * 2);
		}

		public override bool PreDraw(SpriteBatch InSpriteBatch, int InBuffIndex, ref BuffDrawParams RefDrawParams)
		{
			RefDrawParams.Texture = Main.LocalPlayer.HasCooldown(Cooldowns.FenghuangDown.Name)
				? s_CooldownTexture.Value
				: s_Texture.Value;

			return true;
		}

		public override string Texture => HKIConstants.SentienceDivineKeyBuffs + "FenghuangDown";

		[AssetMount(HKIConstants.SentienceDivineKeyBuffs + "FenghuangDown")]
		private static Asset<Texture2D> s_Texture = null;

		[AssetMount(HKIConstants.SentienceDivineKeyBuffs + "FenghuangDownCooldown")]
		private static Asset<Texture2D> s_CooldownTexture = null;

		private bool m_CoreBoost;

		private float MaxLifeBonus => m_CoreBoost ? 1.0f : 0.5f;

		private float BaseLifeRegen => m_CoreBoost ? 80.0f : 20.0f;
		private float MaxLifeRegen => m_CoreBoost ? 0.2f : 0.1f;
	}
}
