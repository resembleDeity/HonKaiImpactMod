using HonKaiImpact.Content.DamageTypes;
using HonKaiImpact.Content.TheDivineKey.Fire.Projectiles;

using Nameless;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace HonKaiImpact.Content.TheDivineKey.Fire
{
	/// <summary>
	/// The Divine Keys are Held Weapon
	/// Fire Divine Key - Judgment Of Shamash is Gun
	/// </summary>
	public class FireDivineKey : ModItem, ITheDivineKey
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

		public override void SetDefaults()
		{
			Item.DamageType = ImaginaryDamageClass.s_Instance;

			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;

			SwitchMode();
		}

		public override void ModifyWeaponDamage(Player InPlayer, ref StatModifier InDamage)
		{
			Item.ModifyTheDivineKeysDamage(InPlayer.HKI(), 2.4f, ref InDamage);
		}

		public override bool CanUseItem(Player InPlayer)
		{
			// 挥砍形态要等上一次挥砍投射物消失，否则按住不放会一次叠出好几段挥砍。
			// 枪形态的 Item.shoot 是子弹，不能拿它当冷却，否则子弹没消失就不能再开枪。
			if (s_ModeData[Mode].UseStyle != ItemUseStyleID.Swing)
			{
				return true;
			}

			return InPlayer.ownedProjectileCounts[Item.shoot] == 0;
		}

		public override bool PreDrawInInventory(SpriteBatch InSpriteBatch, Vector2 InPosition, Rectangle frame, Color drawColor, Color itemColor, Vector2 InOrigin, float InScale)
		{
			if (Main.gameMenu || !Item.IsAlives())
			{
				return true;
			}

			Player localPlayer = Main.LocalPlayer;
			if (localPlayer.HKI().bHeldDivineKey && Item == localPlayer.GetHeldItem())
			{
				// TODO: 手持武器展开时是否要用武器贴图替换背包里的物品图标，等美术确定后再补。
			}

			InSpriteBatch.Draw(TextureAssets.Item[Type].Value, InPosition, null, Color.White, 0.0f, InOrigin, InScale, SpriteEffects.None, 0.0f);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.Register();
		}

		public void SwitchMode(bool InbSwitch = false)
		{
			if (InbSwitch)
			{
				Mode = Mode.NextEnum();
			}

			ModeData data = s_ModeData[Mode];

			Item.useStyle = data.UseStyle;      // ← 两种动画的分水岭
			Item.useTime = data.UseTime;
			Item.useAnimation = data.UseAnimation;
			Item.autoReuse = data.AutoReuse;
			Item.channel = data.Channel;
			Item.noMelee = data.NoMelee;
			Item.noUseGraphic = data.NoUseGraphic;
			Item.damage = data.Damage;
			Item.knockBack = data.KnockBack;
			Item.shoot = data.UseProjectile;
			Item.shootSpeed = data.ShootSpeed;
			Item.UseSound = data.UseSound;

			Item.DamageType = ImaginaryDamageClass.s_Instance;

			Item.width = s_ItemWidth;
			Item.height = s_ItemHeight;

			HKIItem divineKey = Item.HKI();
			if (divineKey is not null)
			{
				divineKey.bHeld = true;
				divineKey.HeldProjectile = data.HeldProjectile;

				// 形态名直接当武器名用（不再往 tooltip 里追加一行）。
				// 同时记在 GlobalItem 上，由 HKIItem.UpdateInventory 每帧重申，
				// 免得重铸之类会重置物品状态的操作把名字冲掉。
				divineKey.NameOverride = Language.GetTextValue($"Mods.{Mod.Name}.Items.{Name}.Mode.{Mode}");
				Item.SetNameOverride(divineKey.NameOverride);
			}
		}

		public EMode Mode = EMode.JudgmentOfShamash;

		private const int s_ItemWidth = 90;
		private const int s_ItemHeight = 134;

		private static readonly Dictionary<EMode, ModeData> s_ModeData = new()
		{
			[EMode.JudgmentOfShamash] = new ModeData
			{
				UseStyle = ItemUseStyleID.Shoot,
				UseTime = 15,
				UseAnimation = 15,
				// TUNING: useTime 5 是连射意图，所以枪形态允许 autoReuse
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
				// TODO: 天火出鞘 —— 暂时沿用大剑的挥砍动画与手持剑，只改数值
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
				UseProjectile = ModContent.ProjectileType<CleaverOfShamash>(),
				HeldProjectile = ModContent.ProjectileType<CleaverOfShamashHeld>(),
			},
			[EMode.ShuhadakuOfUriel] = new ModeData
			{
				// TODO: 乌列尔之审判 —— 暂时沿用大剑的挥砍动画与手持剑，只改数值
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
				UseProjectile = ModContent.ProjectileType<CleaverOfShamash>(),
				HeldProjectile = ModContent.ProjectileType<CleaverOfShamashHeld>(),
			},
		};
	}
}
