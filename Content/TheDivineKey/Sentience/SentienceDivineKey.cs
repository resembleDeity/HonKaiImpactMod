using HonKaiImpact.Content.DamageTypes;
using HonKaiImpact.Content.TheDivineKey.Sentience.Buffs;
using HonKaiImpact.Content.TheDivineKey.Sentience.Projectiles;
using Microsoft.Xna.Framework;
using Nameless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HonKaiImpact.Content.TheDivineKey.Sentience
{
	/// <summary>
	/// 意识之键 —— 饰品。放在背包或饰品栏里就维持召唤物的 buff（<see cref="FenghuangDown"/>）
	/// 与召唤物 <see cref="SentienceMinion"/>；免死效果由那个 buff 承载。
	/// </summary>
	public class SentienceDivineKey : ModItem
	{
		public override void SetDefaults()
		{
			Item.accessory = true;
			Item.width = 16;
			Item.height = 16;
			Item.DamageType = ImaginaryDamageClass.s_Instance;

			Item.value = Item.buyPrice(platinum: 1);
			Item.rare = ItemRarityID.Blue;
		}

		/// <summary>放在背包里也生效。</summary>
		public override void UpdateInventory(Player InPlayer)
		{
			UpdateSummon(InPlayer);
		}

		/// <summary>戴在饰品栏里时生效。</summary>
		public override void UpdateAccessory(Player InPlayer, bool InbHideVisual)
		{
			UpdateSummon(InPlayer);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.Register();
		}

		private void UpdateSummon(Player InPlayer)
		{
			if (InPlayer.whoAmI != Main.myPlayer)
			{
				return;
			}

			int buffType = ModContent.BuffType<FenghuangDown>();

			if (!InPlayer.IsAlives())
			{
				InPlayer.ClearBuff(buffType);
				return;
			}

			InPlayer.AddBuff(buffType, 2);

			int minionType = ModContent.ProjectileType<SentienceMinion>();
			if (InPlayer.ownedProjectileCounts[minionType] == 0)
			{
				m_MinionIndex = Projectile.NewProjectile(InPlayer.GetSource_ItemUse(Item), InPlayer.Center, Vector2.Zero,
					minionType, 0, 0f, InPlayer.whoAmI);
			}

			if (m_MinionIndex >= 0 && m_MinionIndex < Main.maxProjectiles)
			{
				Main.projectile[m_MinionIndex].timeLeft = 2;
			}
		}

		public override string Texture => HKIConstants.SentienceDivineKey + "SentienceDivineKey";

		private int m_MinionIndex = -1;
	}
}
