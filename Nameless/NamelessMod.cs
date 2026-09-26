using Terraria;
using Terraria.ModLoader;

using System.Collections.Generic;

namespace Nameless
{
	public class NamelessMod
	{
		public static void Load(Mod InInstance)
		{
			Instance = InInstance;
			s_Loaders = Utils.GetDerivedInstances<INamelessLoader>();
			foreach (var loader in s_Loaders)
			{
				loader.LoadData();
			}
			NamelessLoader.LoadData();
		}

		public static void Unload()
		{
			foreach (var loader in s_Loaders)
			{
				loader.UnloadData();
			}
			s_Loaders.Clear();
			NamelessLoader.UnloadData();
			NamelessLoader.UnloadAsset();

			Utils.ReleaseModsAndTypesCache();
			Instance = null;
		}

		public static void PostSetupContent()
		{
			foreach (var loader in s_Loaders)
			{
				loader.SetupData();
			}

			if (!Main.dedServ)
			{
				NamelessLoader.LoadAsset();
				foreach (var loader in s_Loaders)
				{
					loader.LoadAsset();
				}
			}

			Utils.ReleaseModsAndTypesCache();
		}



		internal static void LogInfo(string InMessage)
		{
			Instance.Logger.Info(InMessage);
		}

		internal static void LogWarn(string InMessage)
		{
            Instance.Logger.Warn(InMessage);
        }

		internal static void LogError(string InMessage)
		{
			Instance.Logger.Error(InMessage);
		}

		internal static void ParallelError(string InTag, string InMessage)
		{
			Instance.Logger.Error($"[Parallel]{InTag}: {InMessage}");
		}




		private static Mod Instance { get; set; }

		private static List<INamelessLoader> s_Loaders = [];
	}
}
