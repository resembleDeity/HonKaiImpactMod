using Nameless.Structures;

using Terraria.ModLoader;

using System;

namespace Nameless
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NetworkSyncAttribute : Attribute
    {

    }

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
	public class AssetMountAttribute(Mod InMode, EAssetType InType, string InPath) : Attribute
	{
		public AssetMountAttribute(string InPath) : this(null, EAssetType.None, InPath)
		{

		}

		public AssetMountAttribute(EAssetType InType, string InPath) : this(null, InType, InPath)
		{

		}

		public Mod Owner { get; internal set; } = InMode;

		public EAssetType AssetType { get; internal set; } = InType;

		public string Path { get; internal set; } = InPath;
	}
}
