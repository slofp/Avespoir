using Avespoir.Core.Database.Enums;
using System;

namespace Avespoir.Core.Attributes {

	// AttributeTargets.Method is retained for compatibility
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
	class CommandAttribute : Attribute {

		internal string CommandName { get; }

		internal RoleLevel CommandRoleLevel { get; } = RoleLevel.Public;

		/// <summary>
		/// Name null. Can't Execute command.
		/// </summary>
		internal CommandAttribute() {
			CommandName = null;
		}

		/// <summary>
		/// Basic use.(RoleLevel is Public) Mainprefix + (CommandLevel)PrefixTag + Command
		/// </summary>
		/// <remarks>Never conflict the name of a command with the name of another command!</remarks>
		/// <param name="Command"></param>
		internal CommandAttribute(string Command) {
			CommandName = Command;
		}

		/// <summary>
		/// Command with role level. Mainprefix + (CommandLevel)PrefixTag + Command
		/// </summary>
		/// <remarks>Never conflict the name of a command with the name of another command!</remarks>
		/// <param name="Command"></param>
		/// <param name="RequiredRoleLevel"></param>
		internal CommandAttribute(string Command, RoleLevel RequiredRoleLevel) {
			CommandName = Command;
			CommandRoleLevel = RequiredRoleLevel;
		}
	}
}
