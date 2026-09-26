using Nameless.Actors;
using Nameless.GameSystem;

using Terraria;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nameless
{
	public static partial class Utils
	{
		public static bool TryGetOverrides(this Player InPlayer, out Dictionary<Type, PlayerOverride> OutOverrides)
		{
			if (InPlayer.TryGetModPlayer<PlayerHookLoader>(out var player))
			{
				OutOverrides = player.ActiveOverrides;
				return OutOverrides.Count > 0;
			}

			OutOverrides = [];
			return false;
		}



		public static bool TryGetOverrides(this NPC InNpc, out Dictionary<Type, NpcOverride> OutOverrides)
		{
			if (InNpc.TryGetGlobalNPC<NpcHookLoader>(out var global))
			{
				OutOverrides = global.NpcOverrides;
				return OutOverrides.Count > 0;
			}

			OutOverrides = [];
			return false;
		}

		public static bool TryGetOverridesById(this NPC _, int InId, out Dictionary<Type, NpcOverride> OutOverrides)
		{
			OutOverrides = [];

			if (NpcOverride.InstanceById.TryGetValue(InId, out var overrides))
			{
				foreach (var (key, overrideInstance) in overrides)
				{
					if (!overrideInstance.CanOverride())
					{
						continue;
					}

					OutOverrides[key] = overrideInstance.Clone();
				}
			}

			return OutOverrides.Count > 0;
		}



		public static bool TryGetOverrides(this Item _, out Dictionary<Type, ItemOverride> OutOverrides)
		{
			OutOverrides = [];
			foreach (var overrideInstance in ItemOverride.Instances)
			{
				if (!overrideInstance.CanOverride())
				{
					continue;
				}

				OutOverrides[overrideInstance.GetType()] = overrideInstance;
			}

			return OutOverrides.Count > 0;
		}

		public static bool TryGetOverridesById(this Item _, int InId, out Dictionary<Type, ItemOverride> OutOverrides)
		{
			OutOverrides = [];
			if (ItemOverride.InstanceById.TryGetValue(InId, out var overrides))
			{
				foreach (var (key, overrideInstance) in overrides)
				{
					if (!overrideInstance.CanOverride())
					{
						continue;
					}

					OutOverrides[key] = overrideInstance;
				}
			}

			return OutOverrides.Count > 0;
		}



		internal static NamelessHookOverrideCache<GlobalActor> CreateHookCache<TDelegate>(Expression<Func<GlobalActor, TDelegate>> InSelector) 
			where TDelegate : Delegate
		{
			return NamelessHookOverrideCache<GlobalActor>.Create(InSelector);
		}

		internal static NamelessHookOverrideCache<NpcOverride> CreateHookCache<TDelegate>(Expression<Func<NpcOverride, TDelegate>> InSelector)
			where TDelegate : Delegate
		{
			return NamelessHookOverrideCache<NpcOverride>.Create(InSelector);
		}
	}
}
