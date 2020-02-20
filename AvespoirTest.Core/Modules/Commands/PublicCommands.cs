using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	class PublicCommands {

		[Command("test")]
		public async Task Test(CommandContext context) {
			await context.Channel.SendMessageAsync("gomi");
			//await context.RespondAsync("gomi");
		}
	}
}
