using Terraria;
using Terraria.ID;

using System;

namespace Nameless.GameSystem
{
	public abstract class NpcOverride : NamelessType<NpcOverride>
	{
		public virtual void SetDefaults()
		{

		}

		public NpcOverride Clone() => (NpcOverride)Activator.CreateInstance(GetType());

		/// <summary>
		/// 
		/// </summary>
		/// <param name="RefModifiers"></param>
		/// <returns>
		/// <see langword="null"/> execute origin ModNpc function and global modify <br/>
		/// <see langword="true"/> execute origin ModNpc only <br/>
		/// <see langword="false"/> no execute
		/// </returns>
		public virtual bool? On_ModifyIncomingHit(NPC InNpc, ref NPC.HitModifiers RefModifiers)
		{
			return null;
		}

		internal bool ModifyIncomingHit(ref NPC.HitModifiers RefModifiers)
		{
			bool? bOverride = On_ModifyIncomingHit(Owner, ref RefModifiers);

			if (!bOverride.HasValue)
			{
				return true;
			}

			if (bOverride.Value)
			{
				Owner.ModNPC?.ModifyIncomingHit(ref RefModifiers);
			}

			return false;
		}

		public sealed override void NamelessSetup()
		{
			SetStaticDefaults();
		}

		protected sealed override void NamelessRegister()
		{
			if (TargetId == NPCID.None)
			{
				return;
			}

			Instances.Add(this);
			if (TargetId > NPCID.None)
			{
				InstanceById.AddNestedValue(TargetId, GetType(), this);
			}
			if (TargetId == -1)
			{
				UniversalInstances.Add(this);
			}
		}

		public virtual int TargetId => NPCID.None;

		public NPC Owner { get; internal set; }
	}
}
