namespace Nameless.Structures
{
	/// <summary>
	/// Asset<T> => "T" + "Asset"
	/// Asset<Texture2D> => "TextureAsset"
	/// </summary>
	public enum EAssetType
	{
		None,
		Sound,
		TextureAsset,
		EffectAsset,
		Texture,
		Effect,
		Custom,
	}
}
