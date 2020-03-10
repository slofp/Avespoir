using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Modules.Logger;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("help")]
		public async Task Help(CommandObjects CommandObject) {
			try {
				await CommandObject.Channel.SendMessageAsync($"{CommandObject.Member.Mention} DMをご確認ください！");
				await CommandObject.Member.SendMessageAsync("テストだよ");
			}
			catch (Exception Error) {
				new ErrorLog(Error.Message);
			}
		}
	}
}
