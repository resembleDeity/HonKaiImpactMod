using Terraria.Utilities;

using System;
using System.Collections.Generic;

namespace Nameless.Concurrent
{
	public sealed class CommandBuffer
	{
		public void Add(Action InCommand) => Commands.Add(InCommand);



		internal void Reset()
		{
			Commands.Clear();
		}



		public UnifiedRandom Random;

		internal readonly List<Action> Commands = new(16);
	}
}
