using HonKaiImpact.Content;
using HonKaiImpact.Cooldowns;
using Terraria;

namespace HonKaiImpact
{
	public static partial class Utils
	{
		public static Cooldown AddCooldown(this Player InPlayer, string InName, int InTicks, bool InbOverwrite = true)
		{
			Cooldown cooldown = CooldownLoader.CreateInstance(InName);

			if (!InPlayer.HasCooldown(InName) || InbOverwrite)
			{
				cooldown.Owner = InPlayer;
				cooldown.TimeTotal = InTicks;
				cooldown.TimeLeft = InTicks;

				InPlayer.HKI().CooldownsMap[InName] = cooldown;
			}

			return cooldown;
		}

		public static bool HasCooldown(this Player InPlayer, string InName)
		{
			if (InPlayer is null)
			{
				return false;
			}
			var player = InPlayer.HKI();
			return player is not null && player.CooldownsMap.ContainsKey(InName);
		}
	}
}
