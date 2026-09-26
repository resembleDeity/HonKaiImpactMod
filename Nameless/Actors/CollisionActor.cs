using Nameless.Structures;

using Microsoft.Xna.Framework;

namespace Nameless.Actors
{
	public abstract class CollisionActor : Actor
	{

		public Vector2 LastPosition;

		public Vector2 FrameVelocity => Transform.Position - LastPosition;

		public bool bCollision = true;

		public virtual Rectangle CollisionBox => Hitbox;

		public sealed override EExecutionPolicy ExecutionPolicy => EExecutionPolicy.Sequential;
	}
}
