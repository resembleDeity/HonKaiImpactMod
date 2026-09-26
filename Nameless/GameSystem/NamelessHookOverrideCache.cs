using Terraria.ModLoader.Core;

using System;
using System.Linq;
using System.Linq.Expressions;

namespace Nameless.GameSystem
{
	public class NamelessHookOverrideCache<T> where T : NamelessType<T>
	{
		public NamelessHookOverrideCache(LoaderUtils.MethodOverrideQuery<T> InQuery)
		{
			QueryHook = InQuery;
			RefreshCache();
		}

		public ReadOnlySpan<T> Enumerate() => m_Cache;

		public void RefreshCache() => m_Cache = [.. NamelessType<T>.Instances.Where(QueryHook.HasOverride)];

		public bool HasOverride(T InInstance)
		{
			return QueryHook.HasOverride(InInstance);
		}

		public static NamelessHookOverrideCache<T> Create<TDelegate>(Expression<Func<T, TDelegate>> InSelector) where TDelegate : Delegate => new(InSelector.ToOverrideQuery());

		private T[] m_Cache;

		private LoaderUtils.MethodOverrideQuery<T> QueryHook { get; }
	}
}
