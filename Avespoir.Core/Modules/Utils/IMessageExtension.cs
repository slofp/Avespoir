using Discord;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Utils {

	static class IMessageExtension {

		internal static async Task<IMessage> AwaitMessage(this IMessage Message, ulong AllowAuthorID, int Timeout) {
			AwaitMessageInfo AwaitMessage_Info = new AwaitMessageInfo(AllowAuthorID);

			AwaitMessage_Info.Init(Message);

			try {
				Task TimeoutTask = Task.Delay(Timeout);
				while (true) {
					if (TimeoutTask.IsCompletedSuccessfully) return null;

					if (AwaitMessage_Info.Status == AwaitMessageStatus.Pending) continue;

					IMessage WaitedMessage = await Message.Channel.GetMessageAsync(AwaitMessage_Info.MessageID).ConfigureAwait(false);

					return WaitedMessage;
				}
			}
			finally {
				AwaitMessage_Info.Finalize_(Message);
			}
		}
	}
}

