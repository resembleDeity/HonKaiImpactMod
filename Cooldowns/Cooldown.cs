using Terraria;

using System;

namespace HonKaiImpact.Cooldowns
{
	public abstract class Cooldown
	{
		public virtual void Tick() { }

		public virtual void Completed() { }

		/// <summary>从原型复制一份逐玩家实例。</summary>
		public Cooldown Clone() => (Cooldown)Activator.CreateInstance(GetType());

		/// <summary>是否允许推进倒计时；子类可覆盖来实现「某些状态下暂停冷却」。<see cref="Tick"/> 仍会每帧调用。</summary>
		public virtual bool CanTick => true;

		/// <summary>登记时由 <see cref="CooldownLoader"/> 赋值的注册 Id；逐玩家副本会带上同一个值。</summary>
		public int Id;

		public Player Owner;

		public int TimeTotal;

		public int TimeLeft;

		/// <summary>
		/// 子类用 <c>public new static string Name =&gt; "Xxx";</c> 声明的注册名（也当持久化键）。
		/// <para/>
		/// 必须声明成 <c>static</c> 且用 <c>new</c> 隐藏本成员 —— 类的静态成员不能 <c>override</c>，
		/// 所以 <see cref="CooldownLoader"/> 是反射读取的；忘记声明时它读到这里的 <c>null</c> 并报错。
		/// 名字一旦定下就当 API 看待：改动会让旧存档里的键失配。
		/// </summary>
		public static string Name => null;
	}
}
