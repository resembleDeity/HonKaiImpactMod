using HonKaiImpact.Content.Items.Accessories;

using Nameless;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace HonKaiImpact
{
	public class HKIItem : GlobalItem
	{
		public override void SetDefaults(Item InEntity)
		{
			if (InEntity.IsTool())
			{
				bTool = true;

				DefaultPick = InEntity.pick;
				DefaultAxe = InEntity.axe;
				DefaultHammer = InEntity.hammer;
				DefaultTileBoost = InEntity.tileBoost;
				DefaultUseTime = InEntity.useTime;
			}
		}

		public override void UpdateInventory(Item InItem, Player InPlayer)
		{
			// 重铸 / 重新 SetDefaults 之类的操作可能清掉名称覆盖，这里每帧重申一次
			if (!string.IsNullOrEmpty(NameOverride))
			{
				InItem.SetNameOverride(NameOverride);
			}

			if (!bTool)
			{
				return;
			}

			InItem.pick = DefaultPick;
			InItem.axe = DefaultAxe;
			InItem.hammer = DefaultHammer;
			InItem.tileBoost = DefaultTileBoost;
			InItem.useTime = DefaultUseTime;

			if (!InPlayer.HKI().SnapshotCoreStatus.bReason)
			{
				return;
			}
			ReasonHerrscherCore.CoreAbility(InPlayer, InItem);
		}

		public override void HoldItem(Item InItem, Player InPlayer)
		{
			if (HeldProjectile == ProjectileID.None)
			{
				return;
			}
			
			if (InPlayer.whoAmI != Main.myPlayer)
			{
				return;
			}

			if (InPlayer.GetProjectileCount(HeldProjectile) > 0)
			{
				return;
			}

			Projectile.NewProjectile(InItem.GetSource_FromThis(), 
				InPlayer.Center, Vector2.Zero, 
				HeldProjectile, InItem.damage, InItem.knockBack, 
				InPlayer.whoAmI
			);
		}

		public override bool InstancePerEntity => true;

		/// <summary>该物品是否是一把「手持武器」（由神之键的形态数据设置）。</summary>
		public bool bHeld;

		/// <summary>该物品当前形态对应的常驻手持投射物；<see cref="ProjectileID.None"/> 表示没有。</summary>
		public int HeldProjectile;

		/// <summary>
		/// 该物品当前的显示名覆盖（由神之键按形态写入）。
		/// 为空表示不覆盖，用默认的 <c>Items.&lt;物品&gt;.DisplayName</c>。
		/// </summary>
		public string NameOverride;

		public bool bTool;

		public int DefaultPick;

		public int DefaultAxe;

		public int DefaultHammer;

		public int DefaultTileBoost;

		public int DefaultUseTime;
	}
}
