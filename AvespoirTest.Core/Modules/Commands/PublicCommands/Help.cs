using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("$help")]
		public async Task Help(CommandContext Context) {
			try {
				await Context.Channel.SendMessageAsync($"{Context.Member.Mention} DMをご確認ください！");
				await Context.Member.SendMessageAsync("テストだよ");
			}
			catch (Exception Error) {
				new ErrorLog(Error.Message);
			}
		}
	}
}
