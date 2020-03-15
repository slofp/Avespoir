using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Modules.Logger;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("help")]
		public async Task Help(CommandObjects CommandObject) {
			await CommandObject.Channel.SendMessageAsync($"{CommandObject.Member.Mention} DMをご確認ください！");
			await CommandObject.Member.SendMessageAsync("テストだよ");
		}
	}
}
