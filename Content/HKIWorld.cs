using Terraria;
using Terraria.ModLoader;

using System.Collections.Generic;

namespace HonKaiImpact.Content
{
	internal struct WorldHKInfo(float InMinWorldHW, float InMaxWorldHW)
	{
		internal float MinWorldHW = InMinWorldHW;
		internal float MaxWorldHW = InMaxWorldHW;
	}

	internal class HKIWorld : ModSystem
	{
		private static Dictionary<int, WorldHKInfo> s_WorldHWInfoDictionary = [];

		private static float s_WorldHW;

		public override void OnWorldLoad()
		{
			s_WorldHWInfoDictionary = new Dictionary<int, WorldHKInfo>()
			{
				{ 0, new WorldHKInfo(5, 20) },
				{ 1, new WorldHKInfo(18, 40) }
			};

			s_WorldHW = GetWorldHKInfo().MinWorldHW;
		}

		internal static WorldHKInfo GetWorldHKInfo()
		{
			return s_WorldHWInfoDictionary[HKWorldCIVLevel.CIVLevel()];
		}

		internal static float GetWorldHW()
		{
			float minWorldHW = GetWorldHKInfo().MinWorldHW;
			if (s_WorldHW < minWorldHW)
			{
				s_WorldHW = minWorldHW;
			}

			return s_WorldHW;
		}
	}

	public static class HKWorldCIVLevel
	{
		/// <summary>
		/// 克苏鲁之眼 和 史莱姆之王
		/// </summary>
		public static bool Level0 => NPC.downedBoss1 && NPC.downedSlimeKing;

		public static int CIVLevel()
		{
			int level = 0;

			if (Level0)
			{
				level = 1;
			}
			else
			{
				return level;
			}

			return level;
		}
	}
}
