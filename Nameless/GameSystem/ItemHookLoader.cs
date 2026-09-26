using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System;
using System.Reflection;

namespace Nameless.GameSystem
{
	public class ItemHookLoader : GlobalItem, INamelessLoader
	{
		public override void SetDefaults(Item InItem)
		{
			if (InItem.type > ItemID.None)
			{
				OnPreSetDefaults?.Invoke(InItem);
			}

			ProcessOverrides(InItem, item => item.SetDefaults(InItem));

			if (InItem.type > ItemID.None)
			{
				OnPostSetDefaults?.Invoke(InItem);
			}
		}

		void INamelessLoader.LoadData()
		{
			s_ItemLoaderType = typeof(ItemLoader);

			AllowPrefixMethod = s_ItemLoaderType.GetUniquePublicStaticMethod("AllowPrefix");
			if (AllowPrefixMethod is not null)
			{
				NamelessHook.AddHook(AllowPrefixMethod, On_AllowPrefix_Hook);
			}

			MeleePrefixMethod = s_ItemLoaderType.GetUniqueNonPublicStaticMethod("MeleePrefix");
			if (MeleePrefixMethod is not null)
			{
				NamelessHook.AddHook(MeleePrefixMethod, On_MeleePrefix_Hook);
			}
		}

		void INamelessLoader.UnloadData()
		{
			OnPreSetDefaults = null;
			OnPostSetDefaults = null;

			ItemOverride.Clear();

			s_ItemLoaderType = null;
			AllowPrefixMethod = null;
			MeleePrefixMethod = null;
		}



		private static void ProcessOverrides(Item InItem, Action<ItemOverride> InAction)
		{
			if (!InItem.TryGetOverridesById(InItem.type, out var overrides))
			{
				return;
			}

			foreach (var overrideInstance in overrides.Values)
			{
				InAction(overrideInstance);
			}
		}

		private static bool On_AllowPrefix_Hook(On_AllowPrefix_Delegate InOrig, Item InItem, int InPrefix)
		{
			return InOrig(InItem, InPrefix);
		}

		private static bool On_MeleePrefix_Hook(On_MeleePrefix_Delegate InOrig, Item InItem)
		{
			return InOrig(InItem);
		}



		public static event On_PreSetDefaults_Delegate OnPreSetDefaults;
		public delegate void On_PreSetDefaults_Delegate(Item InItem);
		public static event On_PostSetDefaults_Delegate OnPostSetDefaults;
		public delegate void On_PostSetDefaults_Delegate(Item InItem);



		private static Type s_ItemLoaderType;

		private static MethodBase AllowPrefixMethod;
		private delegate bool On_AllowPrefix_Delegate(Item InItem, int InPrefix);

		private static MethodBase MeleePrefixMethod;
		private delegate bool On_MeleePrefix_Delegate(Item InItem);
	}
}
