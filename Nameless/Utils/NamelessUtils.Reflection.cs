using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Nameless
{
	public static partial class Utils
	{
		public static MethodBase GetUniquePublicMethod(this Type InType, string InMethodName)
		{
			return InType.GetMethod(InMethodName, BindingFlags.Public | BindingFlags.Instance);
		}

		public static MethodBase GetPublicMethod(this Type InType, string InMethodName, Type[] InArgs)
		{
			return InType.GetMethod(InMethodName, BindingFlags.Public | BindingFlags.Instance, null, InArgs, null);
		}

		public static MethodBase GetPublicMethod<T>(this Type InType, string InMethodName) where T : Delegate
		{

			return InType.GetMethod(InMethodName, BindingFlags.Public | BindingFlags.Instance, null, GetMethodParametersByDelegate<T>(), null);
		}

		public static MethodBase GetUniqueNonPublicMethod(this Type InType, string InMethodName)
		{
			return InType.GetMethod(InMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public static MethodBase GetNonPublicMethod(this Type InType, string InMethodName, Type[] InArgs)
		{
			return InType.GetMethod(InMethodName, BindingFlags.NonPublic | BindingFlags.Instance, null, InArgs, null);
		}

		public static MethodBase GetNonPublicMethod<T>(this Type InType, string InMethodName) where T : Delegate
		{

			return InType.GetMethod(InMethodName, BindingFlags.NonPublic | BindingFlags.Instance, null, GetMethodParametersByDelegate<T>(), null);
		}

		public static MethodBase GetUniquePublicStaticMethod(this Type InType, string InMethodName)
		{
			return InType.GetMethod(InMethodName, BindingFlags.Public | BindingFlags.Static);
		}

		public static MethodBase GetUniqueNonPublicStaticMethod(this Type InType, string InMethodName)
		{
			return InType.GetMethod(InMethodName, BindingFlags.NonPublic | BindingFlags.Static);
		}

		public static Type[] GetMethodParametersByDelegate<T>() where T : Delegate
		{
			return [.. typeof(T).GetMethod("Invoke").GetParameters().Skip(1).Select(arg => arg.ParameterType)];
		}

		public static bool TryGetFieldOrProperty(this MemberInfo InMember, out Type OutType)
		{
			OutType = InMember is FieldInfo field ? field.FieldType : (InMember as PropertyInfo)?.PropertyType;
			return OutType is not null;
		}

		public static T GetAttribute<T>(MemberInfo InMember) where T: Attribute
		{
			if (!CustomAttributeData.GetCustomAttributes(InMember).Any(attribute => attribute.AttributeType == typeof(T)))
			{
				return null;
			}

			return InMember.GetCustomAttribute<T>();
		}

		public static IList<Type> GetDerivedTypes<T>(Type[] InTypes = null)
		{
			IList<Type> types = [];
			Type baseType = typeof(T);

			InTypes ??= GetAnyModType();

			foreach (var type in InTypes)
			{
				if (!type.IsClass || !baseType.IsAssignableFrom(type) || type.IsAbstract || type == baseType)
				{
					continue;
				}

				types.Add(type);
			}

			return types;
		}

		public static List<T> GetDerivedInstances<T>(Type[] InAllTypes = null, bool InbUnsafeInitialization = false)
		{
			List<T> instances = [];
			foreach (var type in GetDerivedTypes<T>(InAllTypes))
			{
				try
				{
					object obj;
					if (InbUnsafeInitialization)
					{
						obj = RuntimeHelpers.GetUninitializedObject(type);
					}
					else
					{
						if (type.GetConstructor(Type.EmptyTypes) == null)
						{
							NamelessMod.LogError($"Type {type.Name} does not have a parameterless constructor and was skipped.");
							continue;
						}
						obj = Activator.CreateInstance(type);
					}

					if (obj is T instance)
					{
						instances.Add(instance);
					}
				}
				catch (Exception inException)
				{
					NamelessMod.LogError($"Failed to create instance of type {type.Name}. Error: {inException.Message}");
				}
			}

			return instances;
		}
	}
}
