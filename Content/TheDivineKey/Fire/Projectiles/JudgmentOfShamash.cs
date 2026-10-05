using Terraria;
using Terraria.ModLoader;

namespace HonKaiImpact.Content.TheDivineKey.Fire.Projectiles
{
	internal class JudgmentOfShamash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 10;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;

			Projectile.DamageType = DamageClass.Ranged;

			Projectile.timeLeft = 120;
		}

		public override void AI()
		{
			UpdateAnimation();
		}

		private void UpdateAnimation()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 3 == 0)
			{
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}
		}

		public override string Texture => HKIConstants.FireDivineKeyProjectiles + "JudgmentOfShamash";
	}
}
