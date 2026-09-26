using HonKaiImpact.Content.TheDivineKey;
using HonKaiImpact.Structures;

using Nameless;

using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace HonKaiImpact.Content
{
    public class HKIPlayer : ModPlayer
    {
		public override void Initialize()
		{
			Reset();
		}

		public override void PreUpdate()
		{
			SnapshotCoreStatus = HerrscherCoreStatus;
		}

		public override void ResetEffects() => Reset();

		public override void ModifyHurt(ref Player.HurtModifiers RefModifiers)
		{
			RefModifiers.FinalDamage *= 0.25f;
		}

		private void Reset()
		{
			HerrscherCoreStatus.Reset();
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (HKIKeySystem.DivineKey_SwitchModeKey.JustPressed)
			{
				Item heldItem = Player.GetHeldItem();

				if (heldItem.ModItem is ITheDivineKey divineKey)
				{
					divineKey.SwitchMode(true);

					// 清掉旧形态残留的使用状态：开始一次使用要求 itemAnimation == 0（见下），
					// 不清的话枪形态连射的剩余动画会把新形态拖住。
					Player.itemAnimation = 0;

					// 主动放行一次「可再次使用」判定。
					// 依据（反编译 Player.ItemCheck_Inner:42606 / 42672、TryAllowingItemReuse:54039）：
					//   开始一次使用要求 controlUseItem && releaseUseItem && itemAnimation == 0；
					//   而 releaseUseItem = !controlUseItem，按住左键时恒为 false，
					//   只有 CanAutoReuseItem(item) 为真时 vanilla 才会在 TryAllowingItemReuse 里置 true。
					// 所以按住左键切形态时必须在这里放行一次，否则新形态出不了手（必须松手再按）。
					Player.releaseUseItem = true;
				}
			}
		}

		public HerrscherCoreStatus HerrscherCoreStatus = new();
		public HerrscherCoreStatus SnapshotCoreStatus = new();

		public bool bHeldDivineKey;
	}
}
