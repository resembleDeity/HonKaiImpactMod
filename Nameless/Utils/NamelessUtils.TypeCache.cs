using Terraria.ModLoader;
using Terraria.ModLoader.Core;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nameless
{
	public static partial class Utils
	{
		/// <summary>
		/// 请确保在加载完成后释放类型缓存
		/// </summary>
		/// <returns></returns>
		public static Type[] GetAnyModType()
		{
			if (s_AnyModTypeCache is null)
			{
				CacheTypes();
			}
			return s_AnyModTypeCache;
		}

		public static Mod FindTypeOwningMod(Type InType)
		{
			foreach (var mod in ModLoader.Mods)
			{
				if (!s_TypesByModCache.TryGetValue(mod.Code, out var types))
				{
					CacheTypes();
					types = s_TypesByModCache[mod.Code];
				}

				if (types.Contains(InType))
				{
					return mod;
				}
			}

			return null;
		}

		public static void ReleaseModsAndTypesCache()
		{
			s_TypesByModCache?.Clear();
			s_AnyModTypeCache = null;
		}

		private static void CacheTypes()
		{
			List<Type> types = [];
			foreach (var mod in ModLoader.Mods)
			{
				Type[] modTypes = AssemblyManager.GetLoadableTypes(mod.Code);
				types.AddRange(modTypes);
				s_TypesByModCache[mod.Code] = [.. modTypes];
			}
			s_AnyModTypeCache = [.. types];
		}



		private static Type[] s_AnyModTypeCache = null;
		private static readonly Dictionary<Assembly, HashSet<Type>> s_TypesByModCache = [];
	}
}
