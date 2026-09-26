using Nameless.Renderer;
using Nameless.Structures;

using Microsoft.Xna.Framework.Graphics;

namespace Nameless.Actors
{
	internal sealed class ActorRenderer : NamelessRenderer
	{
		public override void DrawBeforeTiles(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{
			ActorLoader.DrawActors(InSpriteBatch, ERenderLayer.BeforeTiles);
		}

		public override void DrawBeforePlayers(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{
			ActorLoader.DrawActors(InSpriteBatch, ERenderLayer.BeforePlayers);
		}

		public override void DrawAfterPlayers(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{
			ActorLoader.DrawActors(InSpriteBatch, ERenderLayer.AfterPlayers);
		}

		public override void DrawBeforeInfernoRings(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{
			ActorLoader.DrawActors(InSpriteBatch, ERenderLayer.BeforeInfernoRings);
		}
	}
}
