using Terraria.ModLoader;

using System.Collections.Generic;

namespace Nameless.Actors
{
	public sealed class CollisionActorLoader : ModSystem, INamelessLoader
	{
		public override void OnWorldUnload() => s_ActiveCollisions.Clear();

		public override void PreUpdateEntities()
		{
			BuildActiveCollisions();
			// TODO:... update
		}

		private static void BuildActiveCollisions()
		{
			s_ActiveCollisions.Clear();

			var actors = ActorLoader.ActiveActors;
			foreach (var actor in actors)
			{
				if (actor is CollisionActor collision && collision.bActive && collision.bCollision)
				{
					s_ActiveCollisions.Add(collision);
				}
			}
		}

		private static readonly List<CollisionActor> s_ActiveCollisions = [];
	}
}
