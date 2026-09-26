using Terraria.ModLoader;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nameless
{
	public abstract class AssetMountLoader
	{
		public virtual void Initialize()
		{

		}

		public virtual void Destroy()
		{

		}

		public abstract object LoadAsset(MemberInfo InMember, AssetMountAttribute InAttribute);
		public virtual void UnloadAsset(MemberInfo InMember, object InAsset)
		{

		}

		public virtual object GetDefaultAsset(Type InType)
		{
			if (InType.IsValueType)
			{
				return Activator.CreateInstance(InType);
			}

			return null;
		}

		public virtual bool CanLoad(Type InType)
		{
			if (InType is null || AssetType is null)
			{
				return false;
			}

			return AssetType.IsAssignableFrom(InType) || InType == AssetType;
		}

		public static AssetMountLoader FindLoader(Type InType)
		{
			foreach (var loader in s_Loaders)
			{
				if (loader.CanLoad(InType))
				{
					return loader;
				}
			}

			return null;
		}

		public static void Register()
		{
			foreach (var type in Utils.GetDerivedTypes<AssetMountLoader>())
			{
				var loader = (AssetMountLoader)Activator.CreateInstance(type);
				loader.Owner = null;
				loader.Initialize();

				s_Loaders.Add(loader);
			}
		}

		public Mod Owner { get; internal set; }

		public abstract Type AssetType { get; }

		private static readonly List<AssetMountLoader> s_Loaders = [];
	}
}
