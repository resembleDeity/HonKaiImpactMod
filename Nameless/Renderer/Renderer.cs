using Terraria;
using Terraria.Graphics.Effects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nameless.Renderer
{
	public abstract class NamelessRenderer : NamelessType<NamelessRenderer>
	{
		public virtual void DrawBeforeTiles(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{

		}
		public virtual void DrawAfterTiles(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{

		}

		public virtual void DrawBeforePlayers(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{

		}
		public virtual void DrawAfterPlayers(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{

		}

		public virtual void DrawBeforeInfernoRings(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{

		}
		public virtual void DrawAfterInfernoRings(SpriteBatch InSpriteBatch, GraphicsDevice InGraphicsDevice)
		{

		}

		public override void NamelessSetup()
		{
			SetStaticDefaults();
		}

		public void Create()
		{
			if (ScreenTargets?.Length != ScreenCount)
			{
				Destroy();
				ScreenTargets = new RenderTarget2D[ScreenCount];
			}

			for (int i = 0; i < ScreenTargets.Length; i++)
			{
				ScreenTargets[i]?.Dispose();
				ScreenTargets[i] = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
			}
		}

		public void Destroy()
		{
			if (ScreenTargets is null)
			{
				return;
			}

			foreach (var target in ScreenTargets)
			{
				target?.Dispose();
			}
			ScreenTargets = null;
		}

		public virtual void Invalidate()
		{

		}

		public virtual void Resize(Vector2 InSize)
		{
			Create();
		}

		protected override void NamelessRegister()
		{
			if (ScreenCount > 0)
			{
				Main.QueueMainThreadAction(Create);
			}

			Instances.Add(this);
		}

		public RenderTarget2D[] ScreenTargets { get; private set; }

		public FilterManager Filters;

		public RenderTarget2D FinalImage;

		protected virtual int ScreenCount => 0;
	}
}
