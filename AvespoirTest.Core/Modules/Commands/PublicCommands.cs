using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {
		#if DEBUG
		[Command("$test")]
		public async Task Test(CommandContext context) {
			await context.Channel.SendMessageAsync("send PublicCommand");
		}
		#endif
	}
}
