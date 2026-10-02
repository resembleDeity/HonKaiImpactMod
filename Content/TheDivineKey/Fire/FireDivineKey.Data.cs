using HonKaiImpact.Content.TheDivineKey.Fire.Projectiles;

using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

using System.Collections.Generic;

namespace HonKaiImpact.Content.TheDivineKey.Fire
{
	public partial class FireDivineKey
	{
		public enum EMode
		{
			JudgmentOfShamash,
			CleaverOfShamash,
			MightOfAnUtu,
			ShuhadakuOfUriel,
		}

		public struct ModeData
		{
			public int UseStyle;

			public int UseTime;

			public int UseAnimation;

			public bool AutoReuse;

			public bool Channel;

			public bool NoMelee;

			public bool NoUseGraphic;

			public int Damage;

			public float KnockBack;

			public float ShootSpeed;

			public SoundStyle? UseSound;

			public int UseProjectile;

			public int HeldProjectile;
		}

		private static readonly Dictionary<EMode, ModeData> s_ModeData = new()
		{
			[EMode.JudgmentOfShamash] = new ModeData
			{
				UseStyle = ItemUseStyleID.Shoot,
				UseTime = 15,
				UseAnimation = 15,
				AutoReuse = true,
				Channel = true,
				NoMelee = true,
				NoUseGraphic = true,
				Damage = 30,
				KnockBack = 6f,
				ShootSpeed = 24f,
				UseSound = SoundID.Item1,
				UseProjectile = ModContent.ProjectileType<JudgmentOfShamash>(),
				HeldProjectile = ModContent.ProjectileType<JudgmentOfShamashHeld>(),
			},
			[EMode.CleaverOfShamash] = new ModeData
			{
				UseStyle = ItemUseStyleID.Swing,
				UseTime = 5,
				UseAnimation = 25,
				AutoReuse = false,
				Channel = true,
				NoMelee = true,
				NoUseGraphic = true,
				Damage = 50,
				KnockBack = 6f,
				ShootSpeed = 24f,
				UseSound = SoundID.Item1,
				UseProjectile = ModContent.ProjectileType<CleaverOfShamash>(),
				HeldProjectile = ModContent.ProjectileType<CleaverOfShamashHeld>(),
			},
			[EMode.MightOfAnUtu] = new ModeData
			{
				UseStyle = ItemUseStyleID.Swing,
				UseTime = 5,
				UseAnimation = 25,
				AutoReuse = false,
				Channel = true,
				NoMelee = true,
				NoUseGraphic = true,
				Damage = 120,
				KnockBack = 6f,
				ShootSpeed = 24f,
				UseSound = SoundID.Item1,
				UseProjectile = ModContent.ProjectileType<MightOfAnUtu>(),
				HeldProjectile = ModContent.ProjectileType<MightOfAnUtuHeld>(),
			},
			[EMode.ShuhadakuOfUriel] = new ModeData
			{
				UseStyle = ItemUseStyleID.Swing,
				UseTime = 5,
				UseAnimation = 25,
				AutoReuse = false,
				Channel = true,
				NoMelee = true,
				NoUseGraphic = true,
				Damage = 220,
				KnockBack = 6f,
				ShootSpeed = 24f,
				UseSound = SoundID.Item1,
				UseProjectile = ModContent.ProjectileType<ShuhadakuOfUriel>(),
				HeldProjectile = ModContent.ProjectileType<ShuhadakuOfUrielhHeld>(),
			},
		};
	}
}
