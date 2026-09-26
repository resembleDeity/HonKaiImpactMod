using Nameless.Structures;

using Terraria.Audio;

using ReLogic.Content;

using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nameless
{
	public static class NamelessLoader
	{
		public static void LoadData()
		{
			
		}

		public static void UnloadData()
		{
			
		}



		internal static void LoadAsset()
		{
			AssetMountLoader.Register();
			s_ProcessedTypeCache.Clear();

			foreach (var type in Utils.GetAnyModType())
			{
				ProcessMember(type, true);
			}

			s_ProcessedTypeCache.Clear();
		}

		internal static void UnloadAsset()
		{
			
		}

		

		#region Asset_PrivateStatic

		private static EAssetType GetAssetType(Type InType)
		{
			if (s_AssetTypeMap.TryGetValue(InType, out var assetType))
			{
				return assetType;
			}
			return EAssetType.None;
		}

		private static object GetDefaultAsset(Type InType)
		{
			if (InType == typeof(Asset<Texture2D>))
			{
				return NamelessAsset.ErrorPlaceholder;
			}
			if (InType == typeof(Texture2D))
			{
				return NamelessAsset.ErrorPlaceholder.Value;
			}

			var customMounter = AssetMountLoader.FindLoader(InType);
			if (customMounter is not null)
			{
				return customMounter.GetDefaultAsset(InType);
			}

			if (InType.IsValueType)
			{
				return Activator.CreateInstance(InType);
			}
			return null;
		}

		private static bool FindAttributeOwningMod(AssetMountAttribute InAttribute, Type InOwingType)
		{
			if (InAttribute.Owner is not null)
			{
				return true;
			}

			InAttribute.Owner = Utils.FindTypeOwningMod(InOwingType);
			return InAttribute.Owner is not null;
		}

		private static void CheckAssetPath(Type InType, string InTargetName, AssetMountAttribute InAttribute)
		{
			if (InAttribute.Path.EndsWith('/'))
			{
				InAttribute.Path += InTargetName;
			}

			if (!string.IsNullOrEmpty(InType.Namespace))
			{
				InAttribute.Path = InAttribute.Path.Replace("{@namespace}", InType.Namespace.Replace('.', '/'));
			}

			if (InAttribute.Path.Contains("{@classPath}"))
			{
				
			}

			string[] splitStrings = InAttribute.Path.Split('/');
			if (splitStrings.Length == 0)
			{
				throw new Exception($"Attribute path on member \"{InTargetName}\" is empty or invalid: \"{InAttribute.Path}\"");
			}

			if (splitStrings[0] == InAttribute.Owner.Name)
			{
				InAttribute.Path = string.Join("/", splitStrings.Skip(1));
			}
		}

		private static void ProcessMember(Type InType, bool InbMount)
		{
			const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
			foreach (var field in InType.GetFields(flags))
			{
				ProcessAsset(field, InType, InbMount);
			}
			foreach (var property in InType.GetProperties(flags))
			{
				ProcessAsset(property, InType, InbMount);
			}
		}

		private static void ProcessAsset(MemberInfo InMember, Type InType, bool InbMount)
		{
			var attribute = InMember.GetCustomAttribute<AssetMountAttribute>();
			if (attribute is null)
			{
				return;
			}

			if (!FindAttributeOwningMod(attribute, InType))
			{
				return;
			}

			if (InbMount)
			{
				CheckAssetPath(InType, InMember.Name, attribute);
				LoadMember(InMember, attribute);
			}
			else
			{
				UnloadMember(InMember);
				attribute.Owner = null;
			}
		}

		private static void LoadMember(MemberInfo InMember, AssetMountAttribute InAttribute)
		{
			if (!InMember.TryGetFieldOrProperty(out var memberType))
			{
				return;
			}

			if (InMember is PropertyInfo prop && (!prop.CanWrite || prop.GetSetMethod(true) is null))
			{
				NamelessMod.LogError($"Property {InMember.Name} is marked with AssetMountAttribute but has no setter.");
				return;
			}

			if (InAttribute.Owner is null)
			{
				NamelessMod.LogWarn($"{InMember.MemberType} {InMember.Name} from Mod is Null, using default value instead.");
				object defaultValue = GetDefaultAsset(memberType);
				if (InMember is FieldInfo field)
				{
					field.SetValue(null, defaultValue);
				}
				else if (InMember is PropertyInfo property)
				{
					property.SetValue(null, defaultValue);
				}
				return;
			}

			if (InAttribute.AssetType == EAssetType.None)
			{
				InAttribute.AssetType = GetAssetType(memberType);
			}

			object result = LoadAsset(InMember, InAttribute);
			if (InMember is FieldInfo fieldMember)
			{
				fieldMember.SetValue(null, result);
			}
			else if (InMember is PropertyInfo propertyMember)
			{
				propertyMember.SetValue(null, result);
			}
		}

		private static void UnloadMember(MemberInfo InMember)
		{
			if (InMember is FieldInfo field)
			{
				field.SetValue(null, null);
			}
			else if (InMember is PropertyInfo property && property.CanWrite && property.GetSetMethod(true) is not null)
			{
				property.SetValue(null, null);
			}
		}

		private static T LoadAsset<T>(MemberInfo InMember, AssetMountAttribute InAttribute) => (T)LoadAsset(InMember, InAttribute);

		private static object LoadAsset(MemberInfo InMember, AssetMountAttribute InAttribute)
		{
			return InAttribute.AssetType switch
			{
				EAssetType.Custom => LoadCustom(InMember, InAttribute),
				EAssetType.Texture => LoadTexture(InAttribute),
				EAssetType.TextureAsset => LoadTextureAsset(InAttribute),
				_ => null
			};
		}

		private static object LoadCustom(MemberInfo InMember, AssetMountAttribute InAttribute)
		{
			return null;
		}

		private static Texture2D LoadTexture(AssetMountAttribute InAttribute) => LoadTextureAsset(InAttribute).Value;

		private static Asset<Texture2D> LoadTextureAsset(AssetMountAttribute InAttribute)
		{
			if (InAttribute.Owner is null)
			{
				return NamelessAsset.ErrorPlaceholder;
			}

			if (!InAttribute.Owner.HasAsset(InAttribute.Path))
			{
				NamelessMod.LogWarn($"Texture asset not found: {InAttribute.Owner.Name}/{InAttribute.Path}.");
				return NamelessAsset.ErrorPlaceholder;
			}

			return InAttribute.Owner.Assets.Request<Texture2D>(InAttribute.Path);
		}

		#endregion

		private static readonly Dictionary<Type, EAssetType> s_AssetTypeMap = new()
		{
			{ typeof(Texture2D),        EAssetType.Texture },
			{ typeof(Asset<Texture2D>), EAssetType.TextureAsset },
			{ typeof(SoundStyle),       EAssetType.Sound },
			{ typeof(Effect),           EAssetType.Effect },
			{ typeof(Asset<Effect>),    EAssetType.EffectAsset },
		};

		private static readonly HashSet<Type> s_ProcessedTypeCache = [];
	}
}
