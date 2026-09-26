// =====================================================================================
// 抽象重构暂缓：本文件内容整体注释保留，等后续决定是否启用。
// 启用步骤：
//   1. 解开本文件的注释块
//   2. 解开 DivineKeyItem.cs 的注释块
//   3. FireDivineKey 改为 : DivineKeyItem<FireDivineKey.EMode>，并删掉自带的
//      ModeData / s_ModeData / SwitchDivineKeyMode / CanUseItem / ModifyTooltips
//   4. ITheDivineKey 恢复 ModeIndex / ModeKey / CurrentMode 三个成员
// =====================================================================================
/*
using Terraria.Audio;
using Terraria.ID;

namespace HonKaiImpact.Content.TheDivineKey
{
	/// <summary>
	/// 神之键一个形态需要的全部差异数据，所有神之键共用同一套形状。
	/// <para/>
	/// 为什么一个 Item 能有两种使用动画：<see cref="Item.useStyle"/>、<see cref="Item.useAnimation"/>、
	/// <see cref="Item.useTime"/>、<see cref="Item.shoot"/> …… 全部是 <b>Item 实例字段</b>，
	/// 运行时随时可以改，下一次使用就读到新值，所以多个形态共用一个 Item 完全没有问题。
	/// <para/>
	/// 反过来，<see cref="Main.RegisterItemAnimation"/>、<see cref="Main.projFrames"/>、<see cref="ItemID.Sets"/>
	/// 都是 <b>按内容类型（type）</b> 的静态表，一个类型只存得下一份，因此「每个形态一套贴图动画」
	/// 不能靠物品贴图本身实现 —— 动画必须放进各形态自己的投射物里。
	/// </summary>
	public struct DivineKeyModeData
	{
		/// <summary>使用动画：<see cref="ItemUseStyleID.Shoot"/>（枪）或 <see cref="ItemUseStyleID.Swing"/>（大剑）。</summary>
		public int UseStyle;

		public int UseTime;

		public int UseAnimation;

		public bool AutoReuse;

		/// <summary>按住鼠标持续使用。近战形态靠它循环挥砍。</summary>
		public bool Channel;

		public bool NoMelee;

		/// <summary>true = 不画物品贴图，手持武器交给 <see cref="HeldProjectile"/> 去画。</summary>
		public bool NoUseGraphic;

		public int Damage;

		public float KnockBack;

		public float ShootSpeed;

		public SoundStyle? UseSound;

		/// <summary>左键使用时生成的投射物：枪 = 子弹；近战 = 挥砍动画。</summary>
		public int UseProjectile;

		/// <summary>常驻的手持投射物；<see cref="ProjectileID.None"/> 表示没有，交回原版绘制手持物品贴图。</summary>
		public int HeldProjectile;
	}
}
*/
