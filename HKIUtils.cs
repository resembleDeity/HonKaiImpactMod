using HonKaiImpact.Content;

using Terraria;
using Terraria.ModLoader;

namespace HonKaiImpact
{
	public static partial class Utils
	{
		internal static void ModifyTools(this Item InItem, int InChangeValue, int InTileBoost, int InUseTime)
		{
			HKIItem item = InItem.HKI();

			if (InItem.pick > 0)
			{
				InItem.pick = item.DefaultPick + InChangeValue;
			}

			if (InItem.axe > 0)
			{
				InItem.axe = (int)(item.DefaultAxe + InChangeValue * 0.2f);
			}

			if (InItem.hammer > 0)
			{
				InItem.hammer = item.DefaultHammer + InChangeValue;
			}

			InItem.tileBoost = item.DefaultTileBoost + InTileBoost;
			InItem.useTime = InUseTime;
		}


		internal static void ModifyTheDivineKeysDamage(this Item _, HKIPlayer InPlayer, float InDamageScale, ref StatModifier RefDamage)
		{
			float playerHW = HKIWorld.GetWorldHW();
			if (InPlayer.SnapshotCoreStatus.bFinality)
			{
				playerHW += 10000;
			}
			RefDamage += playerHW * InDamageScale - RefDamage.Additive;
		}

		#region Prefix Rarity
		internal static readonly float s_PrefixRareLvSubTwo = 0.8f;

		internal static readonly float s_PrefixRareLvSubOne = 0.95f;

		internal static readonly float s_PrefixRareLvNormal = 1.0f;

		internal static readonly float s_PrefixRareLvAddOne = 1.05f;

		internal static readonly float s_PrefixRareLvAddTwo = 1.2f;
		#endregion
	}
}
