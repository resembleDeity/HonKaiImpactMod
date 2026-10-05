using HonKaiImpact.Cooldowns;
using System.Collections.Generic;

namespace HonKaiImpact.Content
{
	public partial class HKIPlayer
	{
		private void UpdateCooldowns()
		{
			if (CooldownsMap.Count <= 0)
			{
				return;
			}

			foreach (var item in CooldownsMap)
			{
				Cooldown cooldown = item.Value;

				if (cooldown.CanTick)
				{
					cooldown.TimeLeft--;
				}

				cooldown.Tick();

				if (cooldown.TimeLeft <= 0)
				{
					cooldown.Completed();
					m_Expired.Add(item.Key);
				}
			}

			foreach (string id in m_Expired)
			{
				CooldownsMap.Remove(id);
			}

			m_Expired.Clear();
		}

		/// <summary>该玩家身上的冷却；键 = <see cref="Cooldown.Name"/>，过期即移除。</summary>
		public Dictionary<string, Cooldown> CooldownsMap;

		private readonly List<string> m_Expired = [];
	}
}
