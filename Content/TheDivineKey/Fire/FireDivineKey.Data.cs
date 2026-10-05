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

		private static readonly Dictionary<EMode, DivineKeyModeData> s_ModeData = new()
		{
			[EMode.JudgmentOfShamash] = new DivineKeyModeData
			{
				UseStyle = ItemUseStyleID.Shoot,
				UseTime = 15,
				UseAnimation = 15,
				AutoReuse = true,
				NoUseGraphic = false,
				Damage = 30,
				KnockBack = 6f,
				ShootSpeed = 24f,
				UseSound = SoundID.Item1,
				UseProjectile = ModContent.ProjectileType<JudgmentOfShamash>(),
				HeldProjectile = ModContent.ProjectileType<JudgmentOfShamashHeld>(),
			},
			[EMode.CleaverOfShamash] = new DivineKeyModeData
			{
				UseStyle = ItemUseStyleID.Swing,
				UseTime = 5,
				UseAnimation = 25,
				Damage = 50,
				KnockBack = 6f,
				ShootSpeed = 24f,
				UseSound = SoundID.Item1,
				UseProjectile = ModContent.ProjectileType<CleaverOfShamash>(),
				HeldProjectile = ModContent.ProjectileType<CleaverOfShamashHeld>(),
			},
			[EMode.MightOfAnUtu] = new DivineKeyModeData
			{
				UseStyle = ItemUseStyleID.Swing,
				UseTime = 5,
				UseAnimation = 25,
				Damage = 120,
				KnockBack = 6f,
				ShootSpeed = 24f,
				UseSound = SoundID.Item1,
				UseProjectile = ModContent.ProjectileType<MightOfAnUtu>(),
				HeldProjectile = ModContent.ProjectileType<MightOfAnUtuHeld>(),
			},
			[EMode.ShuhadakuOfUriel] = new DivineKeyModeData
			{
				UseStyle = ItemUseStyleID.Swing,
				UseTime = 5,
				UseAnimation = 25,
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
