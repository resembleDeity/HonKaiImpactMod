using Terraria;
using Terraria.ModLoader;

using System;
using System.Reflection;
using System.Collections.Generic;

namespace Nameless.GameSystem
{
	public class NpcHookLoader : GlobalNPC, INamelessLoader
	{
		public override void SetDefaults(NPC InEntity)
		{
			if (InEntity.IsAlives())
			{

			}

			ProcessOverrides(InEntity);

			if (InEntity.IsAlives())
			{

			}
		}

		void INamelessLoader.LoadData()
		{
			s_NpcLoaderType = typeof(NPCLoader);

			ModifyIncomingHitMethod = s_NpcLoaderType.GetUniquePublicStaticMethod("ModifyIncomingHit");
			if (ModifyIncomingHitMethod is not null)
			{
				NamelessHook.AddHook(ModifyIncomingHitMethod, On_ModifyIncomingHitMethod_Hook);
			}
		}

		void INamelessLoader.UnloadData()
		{
			NpcOverrides?.Clear();
			NpcOverride.Clear();

			s_NpcLoaderType = null;
			ModifyIncomingHitMethod = null;
		}



		private static void ProcessOverrides(NPC InNpc)
		{
			if (!InNpc.TryGetOverridesById(InNpc.type, out var overrides))
			{
				return;
			}

			if (!InNpc.TryGetGlobalNPC<NpcHookLoader>(out var global))
			{
				return;
			}

			global.NpcOverrides = overrides;

			foreach (var overrideInstance in overrides.Values)
			{
				overrideInstance.Owner = InNpc;
				overrideInstance.SetDefaults();
			}
		}

		private static void On_ModifyIncomingHitMethod_Hook(On_ModifyIncomingHit_Delegate InOrig,
			NPC InNpc,
			ref NPC.HitModifiers RefModifiers)
		{
			if (InNpc.TryGetOverrides(out var overrides))
			{
				foreach (var overrideInstance in overrides.Values)
				{
					if (!overrideInstance.ModifyIncomingHit(ref RefModifiers))
					{
						return;
					}
				}
			}

			foreach (var overrideInstance in NpcOverride.UniversalInstances)
			{
				overrideInstance.Owner = InNpc;
				if (!overrideInstance.ModifyIncomingHit(ref RefModifiers))
				{
					return;
				}
			}

			InOrig.Invoke(InNpc, ref RefModifiers);
		}

		public override bool InstancePerEntity => true;

		public Dictionary<Type, NpcOverride> NpcOverrides { get; private set; } = [];

		private static Type s_NpcLoaderType;

		private static MethodBase ModifyIncomingHitMethod;
		private delegate void On_ModifyIncomingHit_Delegate(NPC InNpc, ref NPC.HitModifiers RefModifiers);
	}
}
