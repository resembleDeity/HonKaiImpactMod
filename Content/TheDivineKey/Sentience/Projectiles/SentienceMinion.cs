using Microsoft.Xna.Framework;
using Nameless;
using Terraria;
using Terraria.ModLoader;

namespace HonKaiImpact.Content.TheDivineKey.Sentience.Projectiles
{
	/// <summary>
	/// 意识之键召唤出来的召唤物。
	/// <para/>
	/// 由 <see cref="SentienceDivineKey"/> 在背包或饰品栏时维持：饰品每帧给它续 <c>timeLeft</c>，
	/// 一旦停止续期它就自己消失。
	/// <para/>
	/// <b>它不是原版的召唤物</b>：不设 <c>Projectile.minion</c> / <c>Projectile.minionSlots</c>，
	/// 因此不占召唤栏、不会被原版「换召唤武器时清理召唤物」（<c>Player.FreeUpPetsAndMinions</c>）顶掉，
	/// 也避开了所有以 <c>Projectile.minion</c> 为前提的原版与模组钩子。
	/// 它的存在与行为完全由 Sentience 自己管理。
	/// </summary>
	public class SentienceMinion : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
		}

		/// <summary>纯护卫，不造成伤害。实现攻击时改成返回 true（见 <see cref="UpdateAttack"/>）。</summary>
		public override bool? CanDamage() => false;

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];

			FollowOwner(owner);
			UpdateAttack(owner);
		}

		/// <summary>
		/// 召唤物攻击占位 —— 目前是空的，它只负责跟着主人。
		/// <para/>
		/// 实现时写在这里：自己找目标（最近的敌对 NPC 之类），位移 / 生成攻击射弹，
		/// 并把 <see cref="CanDamage"/> 改成返回 true。
		/// 因为它不是原版召唤物，右键指定目标那套（<see cref="Player.MinionAttackTargetNPC"/>，
		/// 由 <c>ProjectileID.Sets.MinionTargettingFeature</c> 驱动）不会生效，目标选择要自己写。
		/// </summary>
		private void UpdateAttack(Player InOwner)
		{
		}

		/// <summary>悬停在主人身后上方；掉队太远直接拉回来，免得卡地形。</summary>
		private void FollowOwner(Player InOwner)
		{
			Vector2 target = InOwner.Center + new Vector2(-InOwner.direction * c_IdleOffsetX, c_IdleOffsetY);
			Vector2 toTarget = target - Projectile.Center;

			if (toTarget.Length() > c_TeleportDistance)
			{
				Projectile.Center = target;
				Projectile.velocity = Vector2.Zero;
				Projectile.netUpdate = true;
			}
			else
			{
				// 越远越快，靠近自然减速
				float speed = MathHelper.Lerp(c_MinSpeed, c_MaxSpeed,
					MathHelper.Clamp(toTarget.Length() / c_SlowDownDistance, 0f, 1f));

				Projectile.velocity = (Projectile.velocity * c_Damping
					+ toTarget.SafeNormalize(Vector2.Zero) * speed) / (c_Damping + 1f);
			}

			Projectile.direction = Projectile.spriteDirection = InOwner.direction;
			Projectile.rotation = 0f;
		}

		private const float c_IdleOffsetX = 48f;
		private const float c_IdleOffsetY = -32f;
		private const float c_TeleportDistance = 1600f;
		private const float c_SlowDownDistance = 240f;
		private const float c_MinSpeed = 3f;
		private const float c_MaxSpeed = 14f;
		private const float c_Damping = 20f;

		public override string Texture => HKIConstants.SentienceDivineKeyProjectiles + "SentienceMinion";
	}
}
