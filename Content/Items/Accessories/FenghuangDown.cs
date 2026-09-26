using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HonKaiImpact.Content.Items.Accessories
{
	public class FenghuangDown : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;

			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.Register();
		}
	}
}
