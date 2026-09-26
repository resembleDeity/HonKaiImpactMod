namespace HonKaiImpact.Content.TheDivineKey
{
	internal interface ITheDivineKey
	{
		/// <summary>
		/// Re-applies every mode-dependent stat of the Divine Key to <c>Item</c>.
		/// Call this right after changing <c>Mode</c>.
		/// </summary>
		void SwitchMode(bool inbSwitch = false);

		// ===== 抽象重构暂缓（原地注释保留，待决定）=====
		// 曾经扩展成下面这份契约，由 DivineKeyItem<TMode> 统一实现；
		// 若要启用，连同上面对 SwitchDivineKeyMode 的声明一起恢复，并启用 DivineKeyItem.cs / DivineKeyModeData.cs。
		//
		// /// <summary>当前形态序号，等于枚举值 <c>(int)Mode</c>。</summary>
		// int ModeIndex { get; }
		//
		// /// <summary>当前形态的枚举名，拼本地化键 <c>Items.&lt;物品&gt;.Mode.&lt;ModeKey&gt;</c> 用。</summary>
		// string ModeKey { get; }
		//
		// /// <summary>当前形态的全部数据。</summary>
		// DivineKeyModeData CurrentMode { get; }
	}
}
