using HonKaiImpact.Content.TheDivineKey.Sentience.Projectiles;

using Microsoft.Xna.Framework.Graphics;

using Nameless;
using ReLogic.Content;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

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
			InPlayer.HKI().bFenghuangDown = true;
		}

		/// <summary>
		/// 冷却中换成冷却版图标（<c>FenghuangDownCooldown</c>），就绪时用常态图标。
		/// <para/>
		/// 只改 <see cref="BuffDrawParams.Texture"/> 后返回 true，位置、缩放、文字都交回原版画。
		/// <paramref name="InBuffIndex"/> 是 <see cref="Main.LocalPlayer"/> 的 buff 下标，所以这里查本机玩家即可。
		/// </summary>
		public override bool PreDraw(SpriteBatch InSpriteBatch, int InBuffIndex, ref BuffDrawParams RefDrawParams)
		{
			RefDrawParams.Texture = Main.LocalPlayer.HKI().FenghuangDownCoolingDown
				? s_CooldownTexture.Value
				: s_Texture.Value;

			return true;
		}

		public override string Texture => HKIConstants.SentienceDivineKeyBuffs + "FenghuangDown";

		[AssetMount(HKIConstants.SentienceDivineKeyBuffs + "FenghuangDown")]
		private static Asset<Texture2D> s_Texture = null;

		[AssetMount(HKIConstants.SentienceDivineKeyBuffs + "FenghuangDownCooldown")]
		private static Asset<Texture2D> s_CooldownTexture = null;
	}
}
