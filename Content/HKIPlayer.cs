using HonKaiImpact.Content.TheDivineKey;
using HonKaiImpact.Structures;

using Nameless;

using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace HonKaiImpact.Content
{
    public partial class HKIPlayer : ModPlayer
    {
		public override void Initialize()
		{
			Reset();
			CooldownsMap = new(16);
		}
		public override void ResetEffects() => Reset();


		public override void PreUpdate()
		{
			SnapshotCoreStatus = HerrscherCoreStatus;
		}

		public override void PostUpdateMiscEffects()
		{
			UpdateCooldowns();
		}

		public override void ModifyHurt(ref Player.HurtModifiers RefModifiers)
		{
			// RefModifiers.FinalDamage *= 0.25f;
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (HKIKeySystem.DivineKey_SwitchModeKey.JustPressed)
			{
				Item heldItem = Player.GetHeldItem();

				if (heldItem.ModItem is ITheDivineKey divineKey)
				{
					divineKey.SwitchMode(true);

					// 立刻重置状态，使武器立即可用
					Player.releaseUseItem = true;
					Player.itemTime = 0;
					Player.itemAnimation = 0;
				}
			}
		}

		private void Reset()
		{
			HerrscherCoreStatus.Reset();
			ResetDivineKeys();
		}
		

		public HerrscherCoreStatus HerrscherCoreStatus = new();
		public HerrscherCoreStatus SnapshotCoreStatus = new();

		public bool bHeldDivineKey;
	}
}
