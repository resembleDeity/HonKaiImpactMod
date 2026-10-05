using Terraria.ModLoader;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace HonKaiImpact.Cooldowns
{
	/// <summary>
	/// 冷却注册表：启动时用 <see cref="Nameless.Utils.GetDerivedInstances{T}"/> 找出所有 <see cref="Cooldown"/>
	/// 子类并各造一份原型，按登记顺序分配 <see cref="Cooldown.Id"/>（运行期用）；
	/// 同时保留 名字 ↔ Id ↔ 类型 的映射（名字用于持久化）。
	/// <para/>
	/// 名字来自子类声明的静态 <c>Name</c>（<c>public new static string Name =&gt; "Xxx";</c>）。
	/// 类的静态成员无法 <c>override</c>，所以这里只能反射读取 —— 子类没声明时读到基类的 <c>null</c>，会被下面的校验拦下。
	/// 逐玩家实例存放在 <see cref="HKIPlayer"/> 的 map 里，由它每帧推进。
	/// </summary>
	public class CooldownLoader : ModSystem
	{
		public override void OnModLoad() => RegisterCooldown();

		public override void OnModUnload()
		{
			IdByName?.Clear();
			IdByName = null;

			IdByType?.Clear();
			IdByType = null;

			Instances?.Clear();
			Instances = null;
		}

		/// <summary>按类型取注册 Id；未注册返回 -1。</summary>
		public static int IdOf<T>() where T : Cooldown => IdByType.TryGetValue(typeof(T), out int id) ? id : -1;

		public static int IdOf(string InName)
			=> !string.IsNullOrEmpty(InName) && IdByName.TryGetValue(InName, out int id) ? id : -1;

		/// <summary>取某个 Id 的注册名；未注册返回 null。</summary>
		public static string NameOf(int InId) => InId >= 0 && InId < Instances.Count ? GetName(Instances[InId].GetType()) : null;

		/// <summary>按 Id 造一份逐玩家实例；未注册返回 null。</summary>
		public static Cooldown CreateInstance(int InId)
		{
			if (Instances is null || InId < 0 || InId >= Instances.Count)
			{
				return null;
			}

			Cooldown instance = Instances[InId].Clone();
			instance.Id = InId;

			return instance;
		}

		public static Cooldown CreateInstance(string InName) => CreateInstance(IdOf(InName));

		/// <summary>
		/// 反射读取子类声明的静态 <c>Name</c>。<c>DeclaredOnly</c> 保证只看该类型自己声明的那个 ——
		/// 否则同名成员会在继承链上产生歧义（<see cref="AmbiguousMatchException"/>）。
		/// </summary>
		private static string GetName(Type InType)
			=> InType.GetProperty(nameof(Cooldown.Name), BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)?.GetValue(null) as string;

		private void RegisterCooldown()
		{
			Instances = [.. Nameless.Utils.GetDerivedInstances<Cooldown>()];

			for (int i = 0; i < Instances.Count; i++)
			{
				Cooldown instance = Instances[i];
				Type type = instance.GetType();
				string name = GetName(type);

				// Id 与下标一致（CreateInstance 依赖这点），名字重复/缺失只影响按名字查找
				instance.Id = i;

				if (string.IsNullOrEmpty(name))
				{
					HKIMod.Instance.Logger.Error($"Cooldown {type.Name} 没有声明静态 Name，无法按名字使用");
					continue;
				}

				if (IdByName.ContainsKey(name))
				{
					HKIMod.Instance.Logger.Error($"Cooldown 名字重复：{name}（{type.Name}）");
					continue;
				}

				IdByName[name] = i;
				IdByType[type] = i;
			}
		}

		/// <summary>所有 <see cref="Cooldown"/> 子类的原型，下标即 <see cref="Cooldown.Id"/>。</summary>
		public static List<Cooldown> Instances { get; private set; } = [];

		/// <summary>注册名 → Id（持久化用名字，不用 Id）。</summary>
		public static Dictionary<string, int> IdByName { get; private set; } = [];

		/// <summary>类型 → Id，供泛型 API 做 O(1) 查找。</summary>
		public static Dictionary<Type, int> IdByType { get; private set; } = [];
	}
}
