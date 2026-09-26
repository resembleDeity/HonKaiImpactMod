using HonKaiImpact.Content;

using Terraria;
using Terraria.ID;

namespace HonKaiImpact
{
	internal static partial class Utils
	{
		internal static HKIPlayer HKI(this Player InPlayer)
		{
			return InPlayer.GetModPlayer<HKIPlayer>();
		}

		internal static HKIItem HKI(this Item InItem)
		{
			if (InItem.type == ItemID.None)
			{
				return null;
			}

			return InItem.GetGlobalItem<HKIItem>();
		}

		internal static bool IsTool(this Item InItem) => InItem.pick > 0 || InItem.axe > 0 || InItem.hammer > 0;
	}
}
