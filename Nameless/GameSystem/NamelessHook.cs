using MonoMod.RuntimeDetour;

using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Nameless.GameSystem
{
	public class NamelessHook : INamelessLoader
	{
		public static Hook AddHook(MethodBase InMethod, Delegate InHookDelegate)
		{
			if (InMethod is null)
			{
				throw new ArgumentException("The MethodBase is Null!");
			}
			if (InHookDelegate is null)
			{
				throw new ArgumentException("The Hook Delegate is Null!");
			}

			Hook hook = new Hook(InMethod, InHookDelegate);

			if (!hook.IsApplied)
			{
				hook.Apply();
			}

			if (!Hooks.TryAdd((InMethod, InHookDelegate), hook))
			{
				NamelessMod.LogInfo("The target method is already mounted by the delegate");
			}

			return hook;
		}

		void INamelessLoader.UnloadData()
		{
			foreach (var hook in Hooks.Values)
			{
				if (hook is null)
				{
					continue;
				}

				if (hook.IsApplied)
				{
					hook.Undo();
				}
				hook.Dispose();
			}

			Hooks.Clear();
		}



		public static ConcurrentDictionary<(MethodBase, Delegate), Hook> Hooks { get; private set; } = new ConcurrentDictionary<(MethodBase, Delegate), Hook>();
	}
}
