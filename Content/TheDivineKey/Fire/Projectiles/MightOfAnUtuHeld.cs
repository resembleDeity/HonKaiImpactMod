using Nameless;
using Nameless.Content;

using Terraria;
using Terraria.ModLoader;

using ReLogic.Content;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;


namespace HonKaiImpact.Content.TheDivineKey.Fire.Projectiles
{
	internal class MightOfAnUtuHeld : HeldProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
			Projectile.hide = true;
		}

		public override bool? CanDamage() => false;

		public override bool ShouldUpdatePosition() => false;

		public override bool PreHeldAI()
		{
			PlayerOwner.HKI().bHeldDivineKey = true;

			// 手上已经不是需要这把剑的形态了（切成枪、换成别的武器、空手）→ 自行销毁。
			// 用「当前物品要求的手持投射物是不是我」来判断，这样以后加多少形态都不用改这里。
			if (HeldItem.HKI()?.HeldProjectile != Type)
			{
				Projectile.Kill();
				return false;
			}

			PlayerOwner.HKI().bHeldDivineKey = true;
			if (PlayerOwner.ownedProjectileCounts[ModContent.ProjectileType<CleaverOfShamash>()] != 0)
			{
				Projectile.hide = false;
				return true;
			}
			else
			{
				Projectile.hide = true;
				SetHeldProjectile();
			}

			return true;
		}

		public override void AI()
		{
			UpdateHeldTransform();
		}

		/// <summary>
		/// 跟随人物朝向摆放 —— 只用 <see cref="Player.direction"/>（1 朝右 / -1 朝左），不读鼠标。
		/// 这样手持武器是稳定的"握着"姿势，而不是一直指向鼠标。
		/// </summary>
		private void UpdateHeldTransform()
		{
			int gravDir = Math.Sign(PlayerOwner.gravDir);

			Projectile.Center = PlayerOwner.GetStableCenter() + new Vector2(0.0f, 5.0f) * gravDir;
			Projectile.timeLeft = 2;
			Projectile.scale = 0.7f;

			float heldRotation = Projectile.hide ? 70 : 110;
			if (gravDir == -1)
			{
				heldRotation = Projectile.hide ? 110 : 110;
			}

			Projectile.rotation = MathHelper.ToRadians(heldRotation * Direction * gravDir) + PlayerOwner.fullRotation;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = Projectile.hide ? s_HeldTexture.Value : s_UseHeldTexture.Value;
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Main.EntitySpriteDraw(tex, drawPos, null, lightColor,
				Projectile.rotation, tex.Size() / 2.0f, Projectile.scale,
				Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f
			);
			return false;
		}

		public override string Texture => HKIConstants.FireDivineKeyProjectiles + "MightOfAnUtuHeld";

		[AssetMount(HKIConstants.FireDivineKeyProjectiles + "MightOfAnUtuHeld")]
		private static Asset<Texture2D> s_HeldTexture = null;

		[AssetMount(HKIConstants.FireDivineKeyProjectiles + "MightOfAnUtuUseHeld")]
		private static Asset<Texture2D> s_UseHeldTexture = null;
	}
}
