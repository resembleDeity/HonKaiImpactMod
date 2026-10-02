using Nameless.Content;

using Terraria;
using Terraria.GameContent;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

		/// <summary>
		/// 跟随人物朝向摆放 —— 只用 <see cref="Player.direction"/>（1 朝右 / -1 朝左），不读鼠标。
		/// 这样手持武器是稳定的"握着"姿势，而不是一直指向鼠标。
		/// </summary>
		private void UpdateHeldTransform()
		{
			int direction = PlayerOwner.direction;

			Projectile.direction = direction;
			Projectile.spriteDirection = direction;

			// 贴图按朝右绘制：朝左时整体转 180°，同时靠 FlipHorizontally 防止上下颠倒（见 PreDraw）
			Projectile.rotation = (direction > 0 ? 0f : MathHelper.Pi) + c_RotationOffset;

			Projectile.Center = PlayerOwner.MountedCenter + new Vector2(c_ForwardOffset * direction, 0f);
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

		// TUNING: 手枪相对玩家中心的偏移与贴图朝向，等美术定稿后按贴图微调
		private const float c_ForwardOffset = 8f;
		private const float c_RotationOffset = 0f;
	}
}
