using Nameless.Structures;

using Microsoft.Xna.Framework.Graphics;

using System.Numerics;

namespace Nameless.Particle
{
	public abstract class Particle : NamelessType<Particle>
	{
		public virtual void AI()
		{

		}

		public virtual bool PreDraw(SpriteBatch InSpriteBatch)
		{
			return true;
		}

		public virtual void PostDraw(SpriteBatch InSpriteBatch) 
		{

		}

		public bool bActive = false;

		protected virtual string Texture => "";
		
		private int m_Id;
		private int m_Lifetime = -1;

		private TransformComponent m_Transform;

		private Vector2 m_Velocity;
	}
}
