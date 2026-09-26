using Terraria.ModLoader;

namespace HonKaiImpact
{
	internal class HKIKeySystem : ModSystem, ILocalizedModType
	{
		public override void Load()
		{
			Mod owner = HKIMod.Instance;
			DivineKey_SwitchModeKey = KeybindLoader.RegisterKeybind(owner, nameof(DivineKey_SwitchModeKey), "R");
		}

		public override void Unload()
		{
			DivineKey_SwitchModeKey = null;
		}

		public static ModKeybind DivineKey_SwitchModeKey { get; private set; }

		public string LocalizationCategory => "Keybinds";
	}
}
