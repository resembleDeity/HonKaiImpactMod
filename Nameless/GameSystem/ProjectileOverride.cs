using Terraria;
using Terraria.ID;

using System;

namespace Nameless.GameSystem
{
	public abstract class ProjectileOverride : NamelessType<ProjectileOverride>
	{
		public ProjectileOverride Clone() => (ProjectileOverride)Activator.CreateInstance(GetType());

		public override void NamelessSetup()
		{
			SetStaticDefaults();
		}

		protected override void NamelessRegister()
		{
			if (TargetId == ProjectileID.None)
			{
				return;
			}

			Instances.Add(this);
			if (TargetId > ProjectileID.None)
			{
				InstanceById.AddNestedValue(TargetId, GetType(), this);
			}
			else if (TargetId == -1)
			{
				UniversalInstances.Add(this);
			}
		}

		public virtual int TargetId => ProjectileID.None;
		public Projectile Owner { get; internal set; }
	}
}
