using Nameless.Structures;

using Terraria;
using Terraria.DataStructures;

using System;

namespace Nameless.GameSystem
{
	public abstract class PlayerOverride : NamelessType<PlayerOverride>
	{
		public virtual void SetDefaults() { }

		public PlayerOverride Clone() => (PlayerOverride)Activator.CreateInstance(GetType());

		public virtual bool On_OnHitNpc(NPC InTarget, NPC.HitInfo InHit, int InDamageDone)
		{
			return true;
		}

		public virtual bool On_Hurt(HurtContext InHurtContext)
		{
			return true;
		}

		public virtual bool? On_PreKill(double InDamage, int InHitDirection, bool InbPvp, ref bool RefbPlaySound, ref bool RefbGenDust, ref PlayerDeathReason RefDamageSource)
		{
			return null;
		}

		public sealed override void NamelessSetup()
		{
			SetStaticDefaults();
		}

		protected override void NamelessRegister()
		{
			Instances.Add(this);
		}

		public Player Owner { get; internal set; }
	}
}
