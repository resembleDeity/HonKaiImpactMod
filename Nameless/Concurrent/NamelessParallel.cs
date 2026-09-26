using Terraria;
using Terraria.Utilities;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nameless.Concurrent
{
	public static class NamelessParallel
	{
		public static bool CanParallel() => s_bEnable;

		public static bool IsInParallel() => s_bInParallel;

		public static void ClearDeferred()
		{
			s_bInParallel = false;
			s_CurrentBuffer = null;
			s_CurrentRandom = null;

			lock (s_Lock)
			{
				s_CommandBuffers.Clear();
			}
		}

		public static void DeferCommand(Action InCommand)
		{
			if (InCommand is null)
			{
				return;
			}

			if (s_bInParallel && s_CurrentBuffer is not null)
			{
				s_CurrentBuffer.Commands.Add(InCommand);
				return;
			}

			if (Program.IsMainThread && s_bInParallel)
			{
				InCommand();
				return;
			}

			Main.QueueMainThreadAction(InCommand);
		}

		public static void BeginParallel()
		{
			lock (s_Lock)
			{
				s_CommandBuffers.Clear();
			}

			s_bInParallel = true;
		}

		public static void EndParallel()
		{
			s_bInParallel = false;
			s_CurrentBuffer = null;
			s_CurrentRandom = null;
		}

		public static void ExecuteBatch<T>(IReadOnlyList<T> InItems, Action<T> InCommand)
		{
			if (InItems.Count == 0)
			{
				return;
			}

			Parallel.For(0, InItems.Count, BuildOption(),
				LocalInit, 
				(index, _, buffer) =>
				{
					s_CurrentBuffer = buffer;
					s_CurrentRandom = buffer.Random;
					InCommand(InItems[index]);
					return buffer;
				}, 
				LocalFinally
			);
		}

		public static void ExecuteGroups<T>(List<List<T>> InGroups, Action<T> InCommand)
		{
			if (InGroups.Count == 0)
			{
				return;
			}

			InGroups.Sort(static (left, right) => right.Count - left.Count);
			OrderablePartitioner<List<T>> partitioner = Partitioner.Create(InGroups, EnumerablePartitionerOptions.NoBuffering);
			Parallel.ForEach(partitioner, BuildOption(),
				LocalInit,
				(group, _, buffer) =>
				{
					s_CurrentBuffer = buffer;
					s_CurrentRandom = buffer.Random;
					foreach (var item in group)
					{
						InCommand(item);
					}
					return buffer;
				},
				LocalFinally
			);
		}

		public static void ExecuteDeferred()
		{
			foreach (var commandBuffer in s_CommandBuffers)
			{
				foreach (var command in commandBuffer.Commands)
				{
					command();
				}
			}

			foreach (var commandBuffer in s_CommandBuffers)
			{
				s_CommandPool.Push(commandBuffer);
			}

			lock (s_Lock)
			{
				s_CommandBuffers.Clear();
			}
		}



		private static ParallelOptions BuildOption()
		{
			ParallelOptions options = new();
			if (MaxDegreeOfParallelism > 0)
			{
				options.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
			}
			return options;
		}

		private static CommandBuffer LocalInit()
		{
			if (!s_CommandPool.TryPop(out var buffer))
			{
				buffer = new CommandBuffer();
			}

			buffer.Reset();
			buffer.Random = new UnifiedRandom(unchecked(Environment.TickCount * 397 + Interlocked.Increment(ref s_RandomSeed)));
			lock (s_Lock)
			{
				s_CommandBuffers.Add(buffer);
			}
			s_CurrentBuffer = buffer;
			s_CurrentRandom = buffer.Random;

			return buffer;
		}

		private static void LocalFinally(CommandBuffer InBuffer)
		{
			s_CurrentBuffer = null;
			s_CurrentRandom = null;
		}



		public static int MaxDegreeOfParallelism { get; internal set; } = -1;

		public static IReadOnlyList<CommandBuffer> DeferredCommandBuffers => s_CommandBuffers;



		[ThreadStatic]
		internal static CommandBuffer s_CurrentBuffer;

		[ThreadStatic]
		internal static UnifiedRandom s_CurrentRandom;



		private static bool s_bEnable = true;

		private static volatile bool s_bInParallel;

		private static readonly object s_Lock = new();

		private static readonly ConcurrentStack<CommandBuffer> s_CommandPool = new();

		private static readonly List<CommandBuffer> s_CommandBuffers = new(32);

		private static int s_RandomSeed;
	}
}
