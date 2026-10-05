using Nameless.Content;

using Terraria;
using Terraria.GameContent;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nameless;

namespace HonKaiImpact.Content.TheDivineKey.Fire.Projectiles
{
	public class JudgmentOfShamashHeld : HeldProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
			Projectile.timeLeft = 2;
			Projectile.hide = true;
		}

		public override bool? CanDamage() => false;

		public override bool ShouldUpdatePosition() => false;

		public override bool PreHeldAI()
		{
			PlayerOwner.HKI().bHeldDivineKey = true;

			// 手上已经不是需要这把枪的形态了（切成大剑、换成别的武器、空手）→ 自行销毁
			if (HeldItem.HKI()?.HeldProjectile != Type)
			{
				Projectile.Kill();
				return false;
			}

			// 常驻：不续期就会自己消失
			Projectile.timeLeft = 2;

			// 顶替原版手持贴图（配合形态数据里的 NoUseGraphic = true）
			SetHeldProjectile();

			return true;
		}

		public override void AI()
		{
			UpdateHeldTransform();
		}

		private void UpdateHeldTransform()
		{
			int direction = PlayerOwner.direction;

			Projectile.direction = direction;
			Projectile.spriteDirection = direction;
			Projectile.scale = 0.7f;

			Projectile.Center = PlayerOwner.GetStableCenter() + new Vector2(0.0f, 5.0f);
		}

		/// <summary>
		/// 待机与开火都画同一张纹理 —— 使用中不会消失。
		/// 以后若有了「开火中」专用贴图，在这里按 <c>PlayerOwner.ItemAnimationActive</c> 二选一即可。
		/// </summary>
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Type].Value;
			Vector2 drawPos = Projectile.Center - Main.screenPosition;

			Main.EntitySpriteDraw(tex, drawPos, null, lightColor,
				Projectile.rotation, tex.Size() / 2.0f, Projectile.scale,
				Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f
			);

			return false;
		}

		public override string Texture => HKIConstants.FireDivineKeyProjectiles + "JudgmentOfShamashHeld";
	}
}
