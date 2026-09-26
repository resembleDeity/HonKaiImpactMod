using Terraria;
using Terraria.ModLoader;

namespace Nameless.GameSystem
{
	public class ProjectileHookLoader : GlobalProjectile, INamelessLoader
	{
		void INamelessLoader.LoadData()
		{
			
		}

		void INamelessLoader.UnloadData()
		{
			ProjectileOverride.Clear();
		}
	}
}
