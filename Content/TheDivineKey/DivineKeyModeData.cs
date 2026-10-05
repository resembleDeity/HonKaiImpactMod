using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace HonKaiImpact.Content.TheDivineKey
{
	/// <summary>
	/// 描述神之键不同形态参数的结构。
	/// <para/>
	/// 非 Item 实例字段：<see cref="Main.RegisterItemAnimation"/>、<see cref="Main.projframes"/>、<see cref="ItemID.Sets"/>
	/// 是<b>按内容类型（type）</b>的静态表，无法实时更改，尝试使用 Projectile 实例或其他方式实现
	/// </summary>
	public struct DivineKeyModeData
	{
		public DivineKeyModeData()
		{
		}

		public int UseStyle;

		public int UseTime;

		public int UseAnimation;

		public bool AutoReuse = false;

		public bool Channel = true;

		public bool NoMelee = true;

		public bool NoUseGraphic = true;

		public int Damage;

		public float KnockBack;

		public float ShootSpeed;

		public SoundStyle? UseSound;

		public int UseProjectile = ProjectileID.None;

		public int HeldProjectile = ProjectileID.None;
	}
}
