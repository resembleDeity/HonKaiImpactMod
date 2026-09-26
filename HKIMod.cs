using Nameless;

using Terraria.ModLoader;

namespace HonKaiImpact
{
	public class HKIMod : Mod
	{
        public override void Load()
        {
			NamelessMod.Load(this);
        }

        public override void Unload()
        {
			NamelessMod.Unload();
			s_Instance = null;
        }

		public override void PostSetupContent()
		{
			NamelessMod.PostSetupContent();
		}

		internal static HKIMod Instance
		{
			get { return s_Instance ??= (HKIMod)ModLoader.GetMod("HonKaiImpact"); }
		} 
		private static HKIMod s_Instance;
	}
}
