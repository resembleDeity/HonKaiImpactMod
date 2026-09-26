using Terraria.ModLoader;

using System.Collections.Generic;
using System.Linq;

namespace Nameless.Particle
{
	public class ParticleLoader : ModSystem, INamelessLoader
	{
		public override void Load()
		{
			Instances = [.. Utils.GetDerivedInstances<Particle>(null, true).Where(instance => instance.CanLoad())];

			s_Pool = new List<Particle>[Instances.Count];
			for (int i = 0; i < s_Pool.Length; i++)
			{
				s_Pool[i] = [];
			}
		}

		public static readonly int MaxParticleCount = short.MaxValue;

		public static List<Particle> Instances { get; private set; }

		private static List<Particle>[] s_Pool;
	}
}
