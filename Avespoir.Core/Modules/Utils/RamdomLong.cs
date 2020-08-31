using System;

namespace Avespoir.Core.Modules.Utils {

	static class RamdomLong {

		/*
		 This code base -> https://gist.github.com/subena22jf/c7bb027ea99127944981#file-random
		 */

		private static long Generate(ref Random rd, long min, long max) {
			if (max <= min) throw new ArgumentOutOfRangeException("max", "max must be > min!");

			ulong uRange = (ulong) (max - min);
			ulong ulongRand;
			do {
				byte[] buf = new byte[8];
				rd.NextBytes(buf);
				ulongRand = (ulong) BitConverter.ToInt64(buf, 0);
			}
			while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

			return (long) (ulongRand % uRange) + min;
		}

		internal static long NextLong(this Random rd, long min, long max) => Generate(ref rd, min, max);

		internal static long NextLong(this Random rd, long max) => Generate(ref rd, 0, max);
	}
}
