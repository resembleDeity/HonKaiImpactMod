using Nameless.Renderer;

using Terraria;
using Terraria.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Nameless
{
	public static partial class Utils
	{
		public static Rectangle GetRectangle(this Vector2 InTopLeft, int InSize)
			=> InTopLeft.GetRectangle(InSize, InSize);

		public static Rectangle GetRectangle(this Vector2 InTopLeft, int InWidth, int InHeight) 
			=> new((int)InTopLeft.X, (int)InTopLeft.Y, InWidth, InHeight);

		public static Rectangle GetRectangle(this Vector2 InTopLeft, Vector2 InSize)
			=> InTopLeft.GetRectangle((int)InSize.X, (int)InSize.Y);

		public static Rectangle GetRectangle(this Vector2 InTopLeft, Point InSize)
			=> InTopLeft.GetRectangle(InSize.X, InSize.Y);



		public static Rectangle GetRectangle(this Point16 InTopLeft, int InSize)
			=> InTopLeft.GetRectangle(InSize, InSize);

		public static Rectangle GetRectangle(this Point16 InTopLeft, int InWidth, int InHeight)
			=> new(InTopLeft.X, InTopLeft.Y, InWidth, InHeight);

		public static Rectangle GetRectangle(this Point16 InTopLeft, Point16 InSize)
			=> InTopLeft.GetRectangle(InSize.X, InSize.Y);

		public static Rectangle GetRectangle(this Point16 InTopLeft, Point InSize)
			=> InTopLeft.GetRectangle(InSize.X, InSize.Y);



		internal static void DrawBatch(Action<NamelessRenderer, SpriteBatch> InDrawFunction, bool InbNested = false)
		{
			var batch = Main.spriteBatch;

			foreach (var renderer in NamelessRenderer.Instances)
			{
				if (InbNested)
				{
					batch.End();
				}
				batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
						DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

				InDrawFunction(renderer, batch);

				batch.End();

				if (InbNested)
				{
					batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, 
						DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
				}
			}
		}
	}
}
