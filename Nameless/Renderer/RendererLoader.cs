using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Nameless.Renderer
{
	public sealed class RendererLoader : ModSystem, INamelessLoader
	{
		void INamelessLoader.LoadData()
		{
			Main.OnResolutionChanged += OnResolutionChanged;
			On_Main.DoDraw_WallsAndBlacks += On_DrawBeforeTiles_Hook;
			On_LegacyPlayerRenderer.DrawPlayers += On_DrawPlayers_Hook;
			On_Main.DrawInfernoRings += On_DrawInfernoRings_Hook;
		}

		void INamelessLoader.UnloadData()
		{
			Main.OnResolutionChanged -= OnResolutionChanged;
			On_Main.DoDraw_WallsAndBlacks -= On_DrawBeforeTiles_Hook;
			On_LegacyPlayerRenderer.DrawPlayers -= On_DrawPlayers_Hook;
			On_Main.DrawInfernoRings -= On_DrawInfernoRings_Hook;

			Main.QueueMainThreadAction(Destroy);
		}

		public override void PostUpdateEverything()
		{
			base.PostUpdateEverything();
		}

		public override void PostDrawTiles()
		{
			base.PostDrawTiles();
		}

		internal static void CheckRenderTarget()
		{
			if (TargetSwap is not null && !TargetSwap.IsDisposed)
			{
				return;
			}

			TargetSwap?.Dispose();
			TargetSwap = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);

			foreach (var renderer in NamelessRenderer.Instances)
			{
				renderer.Create();
			}
		}

		private void Destroy()
		{
			TargetSwap?.Dispose();
			TargetSwap = null;

			foreach (var renderer in NamelessRenderer.Instances)
			{
				renderer.Destroy();
			}

			NamelessRenderer.Instances?.Clear();
		}

		private static void InvalidateSwap()
		{
			TargetSwap?.Dispose();
			TargetSwap = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
		}

		private void OnResolutionChanged(Vector2 InSize)
		{
			InvalidateSwap();

			foreach (var renderer in NamelessRenderer.Instances)
			{
				renderer.Resize(InSize);
			}
		}

		private void On_DrawBeforeTiles_Hook(On_Main.orig_DoDraw_WallsAndBlacks InOrig, Main InApplication)
		{
			InOrig(InApplication);

			if (Main.gameMenu)
			{
				return;
			}

			CheckRenderTarget();
			var graphicsDevice = Main.instance.GraphicsDevice;
			Utils.DrawBatch((renderer, batch) => renderer.DrawBeforeTiles(batch, graphicsDevice), true);
		}

		private static void On_DrawPlayers_Hook(On_LegacyPlayerRenderer.orig_DrawPlayers InOrig, LegacyPlayerRenderer InRenderer, Camera InCamera, IEnumerable<Player> InPlayers)
		{
			if (Main.gameMenu)
			{
				InOrig(InRenderer, InCamera, InPlayers);
				return;
			}

			CheckRenderTarget();
			var graphicsDevice = Main.instance.GraphicsDevice;
			Utils.DrawBatch((renderer, batch) => renderer.DrawBeforePlayers(batch, graphicsDevice));

			InOrig(InRenderer, InCamera, InPlayers);

			Utils.DrawBatch((renderer, batch) => renderer.DrawAfterPlayers(batch, graphicsDevice));
		}

		private static void On_DrawInfernoRings_Hook(On_Main.orig_DrawInfernoRings InOrig, Main InApplication)
		{
			if (Main.gameMenu)
			{
				InOrig(InApplication);
				return;
			}

			CheckRenderTarget();
			Utils.DrawBatch((renderer, batch) => renderer.DrawBeforeInfernoRings(batch, Main.instance.GraphicsDevice), true);

			InOrig(InApplication);
		}

		public static RenderTarget2D TargetSwap { get; set; }
	}
}
