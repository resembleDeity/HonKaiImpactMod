using Terraria.ModLoader;

namespace HonKaiImpact.Content.TheDivineKey.Fire.Projectiles
{
	internal class JudgmentOfShamash : ModProjectile
	{
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
	}
}
