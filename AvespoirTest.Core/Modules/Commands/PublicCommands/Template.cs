using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Modules.Logger;
using AvespoirTest.Core.Exceptions;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("$")]
		public async Task Name(CommandContext Context) {
			try {
				string[] msgs = Context.Message.Content.Substring(CommandConfig.MainPrefix.Length + Context.Command.Name.Length).Trim().Split(" ");
				await Task.Delay(0);
			}
			catch (Exception Error) {
				new ErrorLog(Error.Message);
			}
		}
	}
}
