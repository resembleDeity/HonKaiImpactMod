using Microsoft.Xna.Framework;

namespace Nameless.Structures
{
	public struct TransformComponent(Vector2 InPosition, float InRotation = 0.0f, float InScale = 1.0f)
	{
		public TransformComponent() : this(Vector2.Zero, 0.0f, 1.0f)
		{

		}

		public Vector2 Position = InPosition;
		public float Rotation = InRotation;
		public float Scale = InScale;
	}
}
