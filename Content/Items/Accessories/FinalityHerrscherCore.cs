using Terraria;
using Terraria.ModLoader;

namespace HonKaiImpact.Content.Items.Accessories
{
	public class FinalityHerrscherCore : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = Item.height = 32;
			Item.accessory = true;
		}

		public override void UpdateInventory(Player InPlayer)
		{
			InPlayer.HKI().HerrscherCoreStatus.Reset(true);
		}

		public override void UpdateAccessory(Player InPlayer, bool InbHideVisual)
		{
			InPlayer.HKI().HerrscherCoreStatus.Reset(true);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.Register();
		}

		internal static void CoreAbility(Player InPlayer, Item InItem)
		{
			ReasonHerrscherCore.CoreAbility(InPlayer, InItem);
		}
	}
}
