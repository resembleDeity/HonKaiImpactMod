using Nameless.GameSystem;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace Nameless
{
	public static partial class Utils
	{
		public static LocalizedText GetItemLocalizedText(this ItemOverride InItem, string InSuffix, string InRootPath, Func<string> InDefaultValue)
		{
			LocalizedText text;
			if (InItem.TargetId < ItemID.Count)
			{
				string LocalizedPath = InRootPath + "." + ItemID.Search.GetName(InItem.TargetId);
				text = Language.GetText(LocalizedPath);
				if (text.Value == LocalizedPath)
				{
					return InItem.GetLocalization(InSuffix, InDefaultValue);
				}
			}
			else
			{
				text = ItemLoader.GetItem(InItem.TargetId).GetLocalization(InSuffix);
			}

			return InItem.GetLocalization(InSuffix, () => text.Value);
		}



		public static bool IsAlives(this Player InPlayer) => InPlayer is not null && InPlayer.active && !InPlayer.dead;

		public static Vector2 GetStableCenter(this Player InPlayer) => InPlayer.MountedCenter.Floor() + new Vector2(0, InPlayer.gfxOffY);

		public static Item GetSelectedItem(this Player InPlayer) => InPlayer.inventory[InPlayer.selectedItem];

		public static Item GetHeldItem(this Player InPlayer) => Main.mouseItem.IsAir ? InPlayer.GetSelectedItem() : Main.mouseItem;

		public static int GetProjectileCount(this Player InPlayer, int InProjectileType)
		{
			int count = 0;
			foreach (var projectile in Main.ActiveProjectiles)
			{
				if (InPlayer.whoAmI >=0 && InPlayer.whoAmI != projectile.owner)
				{
					continue;
				}

				if (projectile.type == InProjectileType)
				{
					count++;
				}
			}
			return count;
		}



		public static bool IsAlives(this NPC InNpc) => InNpc is not null && InNpc.active && InNpc.timeLeft > 0;

		public static bool IsAlives(this Item InItem) => InItem is not null && InItem.type > ItemID.None && InItem.stack > 0;

		public static bool IsAlives(this Projectile InProjectile) => InProjectile is not null && InProjectile.active && InProjectile.timeLeft > 0;


	
		public static void AddNestedValue<TOuterKey, TInnerKey, TInnerValue>(
			this Dictionary<TOuterKey, Dictionary<TInnerKey, TInnerValue>> InDictionary,
			TOuterKey InOuterKey,
			TInnerKey InInnerKey,
			TInnerValue InValue)
		{
			if (!InDictionary.TryGetValue(InOuterKey, out var innerDictionary))
			{
				innerDictionary = [];
				InDictionary[InOuterKey] = innerDictionary;
			}
			innerDictionary[InInnerKey] = InValue;
		}
	}
}
