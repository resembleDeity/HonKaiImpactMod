// =====================================================================================
// 抽象重构暂缓：本文件内容整体注释保留，等后续决定是否启用。
// 这是原 DivineKeyItem<TMode> 泛型抽象基类的完整实现（保留 Dictionary 方案，不引入 List 下标）。
// 启用步骤见 DivineKeyModeData.cs 顶部说明。
// =====================================================================================
/*
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HonKaiImpact.Content.TheDivineKey
{
	/// <summary>
	/// 所有带形态的神之键的共享实现。
	/// <para/>
	/// 形态逻辑（<c>Mode</c> 字段、切换、套用、冷却判断、tooltip）与形态数据分离：
	/// 基类只写一遍逻辑，子类只提供一张 <see cref="ModeTable"/>。
	/// <para/>
	/// 为什么用 <see cref="Dictionary{TKey,TValue}"/> 而不是 <c>List</c> 按枚举值下标取：
	/// 字典以「枚举成员」为键，不需要维护「表长度 == 枚举成员数」和「枚举值必须从 0 连续」这两个隐形约定；
	/// 往枚举中间插成员时键仍然对得上，也不会因为写了个 <c>JudgmentOfShamash = 1</c> 就静默取到隔壁那一行。
	/// <para/>
	/// 泛型参数就是各神之键自己的形态枚举（例如 <c>FireDivineKey.EMode</c>），
	/// 非泛型的 <see cref="ITheDivineKey"/> 由本类统一实现，外部系统不需要知道具体枚举类型。
	/// </summary>
	public abstract class DivineKeyItem<TMode> : ModItem, ITheDivineKey
		where TMode : struct, Enum
	{
		/// <summary>当前形态。子类自己的枚举类型，外部通过 <see cref="ITheDivineKey.ModeIndex"/> 读。</summary>
		public TMode Mode;

		/// <summary>
		/// 形态表，由子类提供（通常是自己的 <c>private static readonly Dictionary&lt;TMode, DivineKeyModeData&gt;</c>）。
		/// </summary>
		protected abstract IReadOnlyDictionary<TMode, DivineKeyModeData> ModeTable { get; }

		protected virtual int ItemWidth => 90;

		protected virtual int ItemHeight => 134;

		public int ModeIndex => Convert.ToInt32(Mode);

		public string ModeKey => Mode.ToString();

		public DivineKeyModeData CurrentMode => ModeTable[Mode];

		/// <summary>
		/// 套用当前形态（<paramref name="InbSwitch"/> 为 true 时先切到下一个形态）。
		/// <para/>
		/// <see cref="Utils.NextEnum{T}"/> 是按枚举 <b>声明顺序</b> 取下一个并在末尾回绕的，
		/// 与枚举值连不连续无关 —— 这也是用它配字典、而不是配 List 下标更稳的原因。
		/// </summary>
		public void SwitchDivineKeyMode(bool InbSwitch = false)
		{
			if (InbSwitch)
			{
				Mode = Mode.NextEnum();
			}

			ApplyModeData(CurrentMode);
		}

		public override bool CanUseItem(Player InPlayer)
		{
			// 挥砍形态要等上一次挥砍投射物消失，否则按住不放会一次叠出好几段挥砍。
			// 枪形态的 Item.shoot 是子弹，不能拿它当冷却，否则子弹没消失就不能再开枪。
			if (CurrentMode.UseStyle != ItemUseStyleID.Swing)
			{
				return true;
			}

			return InPlayer.ownedProjectileCounts[Item.shoot] == 0;
		}

		public override void ModifyTooltips(List<TooltipLine> InTooltips)
		{
			InTooltips.Add(new TooltipLine(Mod, s_ModeTooltipLine,
				Language.GetTextValue($"Mods.{Mod.Name}.Items.{Name}.Mode.{ModeKey}")));
		}

		/// <summary>
		/// 把一份形态数据套用到 <see cref="Item"/> 上。
		/// <para/>
		/// 这些字段全是 Item 的实例字段，所以同一个 Item 类型可以随时切换使用动画。
		/// <c>bHeld</c> / <c>HeldProjectile</c> 写在 GlobalItem 上，
		/// 具体的手持投射物生成由 <see cref="HKIItem.HoldItem"/> 统一负责。
		/// </summary>
		protected void ApplyModeData(DivineKeyModeData InData)
		{
			Item.useStyle = InData.UseStyle;      // ← 多种使用动画的分水岭
			Item.useTime = InData.UseTime;
			Item.useAnimation = InData.UseAnimation;
			Item.autoReuse = InData.AutoReuse;
			Item.channel = InData.Channel;
			Item.noMelee = InData.NoMelee;
			Item.noUseGraphic = InData.NoUseGraphic;
			Item.damage = InData.Damage;
			Item.knockBack = InData.KnockBack;
			Item.shoot = InData.UseProjectile;
			Item.shootSpeed = InData.ShootSpeed;
			Item.UseSound = InData.UseSound;

			Item.width = ItemWidth;
			Item.height = ItemHeight;

			HKIItem divineKey = Item.HKI();
			if (divineKey is not null)
			{
				divineKey.bHeld = true;
				divineKey.HeldProjectile = InData.HeldProjectile;
			}
		}

		private const string c_ModeTooltipLine = "DivineKeyMode";
	}
}
*/
