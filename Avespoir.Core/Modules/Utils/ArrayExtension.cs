using System.Collections.Generic;

namespace Avespoir.Core.Modules.Utils {

	static class ArrayExtension {

		internal static string[] Remove(this string[] StringArray, int RemoveValue, int RemoveCount = 1) {
			List<string> ArgList = new List<string>();
			
			ArgList.AddRange(StringArray);
			ArgList.RemoveRange(RemoveValue, RemoveCount);

			return ArgList.ToArray();
		}
	}
}
