using Terraria.ModLoader;

using System;
using System.Collections.Generic;

namespace Nameless
{
	public abstract class NamelessType<T> : ModType where T : NamelessType<T>
	{
		public virtual bool CanLoad()
		{
			return true;
		}

		public virtual bool CanOverride()
		{
			return true;
		}

		public sealed override void SetupContent()
		{
			if (!CanLoad())
			{
				return;
			}

			NamelessSetup();
		}

		public virtual void NamelessSetup()
		{
			
		}

		internal static void Clear()
		{
			InstanceById?.Clear();
			Instances?.Clear();
			UniversalInstances?.Clear();
		}

		protected sealed override void Register()
		{
			NamelessRegister();
		}

		protected virtual void NamelessRegister()
		{

		}

		public static List<T> Instances { get; private set; } = [];
		public static List<T> UniversalInstances { get; private set; } = [];

		public static Dictionary<int, Dictionary<Type, T>> InstanceById { get; private set; } = [];
	}
}
