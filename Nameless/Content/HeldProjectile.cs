using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;

namespace Nameless.Content
{
	public abstract class HeldProjectile : ModProjectile
	{
		public virtual void Initialize()
		{

		}

		public sealed override bool PreAI()
		{
			UpdateMouseData();

			if (!m_bInitialize)
			{
				Initialize();
				m_bInitialize = true;
			}


			return PreHeldAI();
		}

		public virtual bool PreHeldAI()
		{
			return true;
		}

		public sealed override void PostAI()
		{
			if (!PlayerOwner.IsAlives())
			{
				Projectile.Kill();
				// TODO:... Network
			}
		}

		protected virtual void SetHeldProjectile() => PlayerOwner.heldProj = Projectile.whoAmI;

		private void UpdateMouseData()
		{
			UpdateMouseOffset();

			MouseRotation = MouseOffset.ToRotation();
			MouseDirection = MouseOffset.SafeNormalize(Vector2.UnitY);
			MousePosition = MouseOffset + PlayerOwner.GetStableCenter();
		}

		private void UpdateMouseOffset()
		{
			// Main.MouseWorld 只描述本机光标，所以只有 owner 本机能算出正确值，其余客户端等同步。
			if (Projectile.owner == Main.myPlayer)
			{
				var offset = Main.MouseWorld - PlayerOwner.GetStableCenter();

				// 对齐到玩家本地「上」方向：重力倒转时整体上下镜像。
				offset.Y *= GravDirection < 0 ? -1 : 1;

				// TODO:... UpdateNetWorkd maybe sync and send in same function

				MouseOffset = offset;
			}
		}

		public virtual Texture2D HeldTexture => TextureAssets.Projectile[Type].Value;

		public virtual Player PlayerOwner => Main.player[Projectile.owner];

		public Item HeldItem => PlayerOwner.GetHeldItem();



		/// <summary>
		/// 从 <see cref="PlayerOwner"/> 稳定中心指向鼠标的位移向量，
		/// 已按 <see cref="GravDirection"/> 对齐到玩家本地「上」方向。
		/// 仅由 owner 本机写入，其余客户端待网络同步。
		/// </summary>
		public virtual Vector2 MouseOffset { get; set; }

		/// <summary>
		/// <see cref="MouseOffset"/> 的单位化方向，等价于 <c>MouseOffset.SafeNormalize(Vector2.UnitY)</c>。
		/// </summary>
		public virtual Vector2 MouseDirection { get; set; }

		/// <summary>
		/// <see cref="MouseOffset"/> 的弧度朝向，等价于 <c>MouseOffset.ToRotation()</c>。
		/// </summary>
		public virtual float MouseRotation { get; set; }

		/// <summary>
		/// <see cref="MouseOffset"/> 还原回世界空间后的鼠标位置，等价于 <c>MouseOffset + PlayerOwner.GetStableCenter()</c>。
		/// </summary>
		public virtual Vector2 MousePosition { get; set; }

		public int GravDirection => Math.Sign(PlayerOwner.gravDir);

		public virtual int Direction => PlayerOwner.direction * GravDirection;

		private bool m_bInitialize;
	}
}
