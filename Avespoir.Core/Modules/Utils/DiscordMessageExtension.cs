using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Utils {

	static class DiscordMessageExtension {

		internal static async Task<DiscordMessage> AwaitMessage(this DiscordMessage Discord_Message, ulong AllowAuthorID, int Timeout) {
			AwaitMessageInfo AwaitMessage_Info = new AwaitMessageInfo(AllowAuthorID);

			AwaitMessage_Info.Init(Discord_Message);

			try {
				Task TimeoutTask = Task.Delay(Timeout);
				while (true) {
					if (TimeoutTask.IsCompletedSuccessfully) return null;

					if (AwaitMessage_Info.Status == AwaitMessageStatus.Pending) continue;

					DiscordMessage WaitedMessage = await Discord_Message.Channel.GetMessageAsync(AwaitMessage_Info.MessageID).ConfigureAwait(false);

					return WaitedMessage;
				}
			}
			finally {
				AwaitMessage_Info.Finalize_(Discord_Message);
			}
		}
	}
}

