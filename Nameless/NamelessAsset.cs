using ReLogic.Content;

using Microsoft.Xna.Framework.Graphics;

namespace Nameless
{
	// [AssetMount("Assets/")]
	public static class NamelessAsset
	{
		public static Asset<Texture2D> AlphaPlaceholder { get; set; }

		public static Asset<Texture2D> WithePlaceholder { get; set; }

		public static Asset<Texture2D> ErrorPlaceholder { get; set; }
	}
}
