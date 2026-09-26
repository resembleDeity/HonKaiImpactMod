using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Nameless.GameSystem
{
	public abstract class ItemOverride : NamelessType<ItemOverride>, ILocalizedModType
	{
		public virtual void SetDefaults(Item InItem)
		{

		}

		public sealed override void NamelessSetup()
		{
			SetStaticDefaults();
		}

		protected sealed override void NamelessRegister()
		{
			if (TargetId == ItemID.None)
			{
				return;
			}
			
			Instances.Add(this);
			if (TargetId > ItemID.None)
			{
				InstanceById.AddNestedValue(TargetId, GetType(), this);
			}
			else if (TargetId == -1)
			{
				UniversalInstances.Add(this);
			}
		}

		public virtual int TargetId => ItemID.None;

        public virtual string LocalizationCategory => "OverrideItems";

		public virtual LocalizedText DisplayName => this.GetItemLocalizedText(nameof(DisplayName), "ItemName", () => Name);

		public virtual LocalizedText Tooltip => this.GetItemLocalizedText(nameof(Tooltip), "ItemTooltip", () => "");
	}
}
