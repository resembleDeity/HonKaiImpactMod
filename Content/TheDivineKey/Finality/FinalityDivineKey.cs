using HonKaiImpact.Content.DamageTypes;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace HonKaiImpact.Content.TheDivineKey.Finality
{
	public class FinalityDivineKey : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item1;

			Item.DamageType = ImaginaryDamageClass.s_Instance;
			Item.damage = 40;
			Item.knockBack = 6;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.ChlorophyteBullet;
			Item.shootSpeed = 24;

			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
		}

		public override void ModifyWeaponDamage(Player InPlayer, ref StatModifier RefDamage)
		{
			Item.ModifyTheDivineKeysDamage(InPlayer.HKI(), 15.0f, ref RefDamage);
		}

		public override bool Shoot(Player InPlayer, EntitySource_ItemUse_WithAmmo InSource, Vector2 InPosition, Vector2 InVelocity, int InType, int InDamage, float InKnockback)
		{
			Projectile.NewProjectile(InSource, InPosition, InVelocity, InType, InDamage, InKnockback, InPlayer.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.Register();
		}
	}
}
