using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nameless.Actors
{
	public abstract class GlobalActor : NamelessType<GlobalActor>
	{
		public override void NamelessSetup()
		{
			SetStaticDefaults();
		}

		protected override void NamelessRegister()
		{
			Instances.Add(this);
		}

		public virtual void OnSpawn(Actor InActor)
		{

		}

		public virtual bool PreAI(Actor InActor)
		{
			return true;
		}

		public virtual void PostAI(Actor InActor)
		{
			
		}

		public virtual bool PreDraw(Actor InActor, SpriteBatch InSpriteBatch, Color InColor)
		{
			return true;
		}

		public virtual void PostDraw(Actor InActor, SpriteBatch InSpriteBatch, Color InColor)
		{

		}
	}
}
