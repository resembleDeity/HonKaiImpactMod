using Nameless;
using Nameless.Content;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace HonKaiImpact.Content.TheDivineKey.Fire.Projectiles
{
	/// <summary>
	/// 大剑形态的挥砍动画（动画 B）：14 帧一轮，由 <see cref="FireDivineKey"/> 的 Swing 形态生成。
	/// </summary>
	internal class MightOfAnUtu : HeldProjectile
	{
		public override void SetStaticDefaults()
		{
			// 贴图是 384x3780 的纵向帧表，14 帧 → 每帧 384x270
			Main.projFrames[Type] = 14;
		}

		public override void SetDefaults()
		{
			Projectile.width = 216;
			Projectile.height = 216;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;

			Projectile.DamageType = DamageClass.Melee;

			Projectile.frameCounter = 0;

			Projectile.alpha = 255;
		}

		public override void AI()
		{
			if (!bSpan)
			{
				InitializeProjectile();
			}

			UpdateAttackState();

			UpdateAnimation();

			Vector2 playerRotatedPoint = PlayerOwner.RotatedRelativePoint(PlayerOwner.MountedCenter, true);
			if (Projectile.owner == Main.myPlayer)
			{
				if (PlayerOwner.channel && !PlayerOwner.noItems && !PlayerOwner.CCed && PlayerOwner.GetHeldItem().shoot == Projectile.type)
				{
					ChannelMovement(playerRotatedPoint);
				}
				else if (bKill)
				{
					Projectile.Kill();
				}
			}

			UpdateTransform();
		}

		public override bool PreDraw(ref Color inLightColor)
		{
			if (Projectile.frameCounter <= 1)
			{
				return false;
			}

			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Rectangle rect = texture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
			SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + Projectile.velocity * 0.3f
					+ new Vector2(0, -32).RotatedBy(Projectile.rotation),
				rect, Color.White, Projectile.rotation, rect.Size() * 0.5f, Projectile.scale, effects, 0
			);

			return false;
		}

		private void InitializeProjectile()
		{
			bSpan = true;
		}

		private void SetAttackState()
		{
			bAttack = true;
			bKill = true;
			Projectile.numHits = 0;
		}

		private void UpdateAttackState()
		{
			if (IsAttack1)
			{
				SetAttackState();
			}
			else if (IsAttack2)
			{
				SetAttackState();
			}
			else if (IsAttack3)
			{
				SetAttackState();
			}
			else
			{
				bKill = false;
				bAttack = false;
			}
		}

		private void UpdateAnimation()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 3 == 0)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}
		}

		private void UpdateTransform()
		{
			float velocityAngle = Projectile.velocity.ToRotation();

			if (bAttack || IsAttack1)
			{
				Projectile.rotation = velocityAngle + (Projectile.direction == -1).ToInt() * MathHelper.Pi;
			}
			Projectile.direction = (Math.Cos(velocityAngle) > 0).ToDirectionInt();

			float offset = 80.0f * Projectile.scale;
			Vector2 playerRotatedPoint = PlayerOwner.RotatedRelativePoint(PlayerOwner.MountedCenter, true);
			Projectile.Center = playerRotatedPoint + velocityAngle.ToRotationVector2() * offset;

			PlayerOwner.ChangeDir(Projectile.direction);
			PlayerOwner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
			PlayerOwner.heldProj = Projectile.whoAmI;
			PlayerOwner.itemTime = 2;
			PlayerOwner.itemAnimation = 2;
		}

		private void ChannelMovement(Vector2 InPlayerRotatedPoint)
		{
			float speed = 1.0f;
			Item heldItem = PlayerOwner.GetHeldItem();
			if (heldItem.shoot == Projectile.type)
			{
				speed = heldItem.shootSpeed * Projectile.scale;
			}
			Vector2 newVelocity = MouseOffset.SafeNormalize(Vector2.UnitX * PlayerOwner.direction) * speed;

			if (bAttack)
			{
				Projectile.velocity = newVelocity;
			}
		}

		public bool bSpan;
		public bool bKill;
		public bool bAttack = false;

		public bool IsAttack1 => Projectile.frame == 0;
		public bool IsAttack2 => Projectile.frame == 6;
		public bool IsAttack3 => Projectile.frame == 10;

		public override string Texture => HKIConstants.FireDivineKeyProjectiles + "MightOfAnUtu";
	}
}
