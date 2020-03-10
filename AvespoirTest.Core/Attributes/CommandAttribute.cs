using System;

namespace AvespoirTest.Core.Attributes {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	class CommandAttribute : Attribute {

		internal string CommandName { get; }

		/// <summary>
		/// Name null. Can't excute command.
		/// </summary>
		internal CommandAttribute() {
			CommandName = null;
		}

		/// <summary>
		/// Basic use. Mainprefix + (CommandLevel)PrefixTag + Command
		/// </summary>
		/// <param name="Command"></param>
		internal CommandAttribute(string Command) {
			CommandName = Command;
		}
	}
}
