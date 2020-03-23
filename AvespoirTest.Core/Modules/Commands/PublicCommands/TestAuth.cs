using AvespoirTest.Core.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {


		[Command("test")]
		public async Task TestAuth(CommandObjects CommandObject) {
			#if DEBUG
			if (!await Authentication.Confirmation(CommandObject)) {
				await CommandObject.Channel.SendMessageAsync("初めからやり直してください");
			}
			#endif
		}
	}
}