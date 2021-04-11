using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("WaitTest")]
		public async Task WaitTest(CommandObjects CommandObject) {
			await CommandObject.Channel.SendMessageAsync("‰½‚©‚ð“ü—Í").ConfigureAwait(false);

			DiscordMessage WaitSend = await Task.Run(async () => {
				return await CommandObject.Message.AwaitMessage(CommandObject.Message.Author.Id, 1 * 60 * 1000).ConfigureAwait(false);
			}).ConfigureAwait(false);

			if (!(WaitSend is null)) await WaitSend.Channel.SendMessageAsync(WaitSend.Content).ConfigureAwait(false);
			else await CommandObject.Channel.SendMessageAsync("1•ªŠÔ“ü—Í‚³‚ê‚Ü‚¹‚ñ‚Å‚µ‚½").ConfigureAwait(false);
		}
	}
}
