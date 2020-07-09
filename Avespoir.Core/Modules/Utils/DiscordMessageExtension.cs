using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Utils {

	static class DiscordMessageExtension {

		#region AwaitMessage Methods

		internal static async Task<DiscordMessage> AwaitMessage(this DiscordMessage Discord_Message, int Timeout) {
			Task TimeoutTask = Task.Delay(Timeout);
			while (true) {
				if (TimeoutTask.IsCompletedSuccessfully) return null;

				DiscordMessage LastMessage = await Discord_Message.Channel.GetMessageAsync(Discord_Message.Channel.LastMessageId).ConfigureAwait(false);

				if (LastMessage.Id == Discord_Message.Id) continue;

				return LastMessage;
			}
		}

		internal static async Task<DiscordMessage> AwaitMessage(this DiscordMessage Discord_Message, ulong AllowAuthorID, int Timeout) {
			Task TimeoutTask = Task.Delay(Timeout);
			while (true) {
				if (TimeoutTask.IsCompletedSuccessfully) return null;

				DiscordMessage LastMessage = await Discord_Message.Channel.GetMessageAsync(Discord_Message.Channel.LastMessageId).ConfigureAwait(false);

				if (LastMessage.Id == Discord_Message.Id) continue;
				if (LastMessage.Author.Id != AllowAuthorID) continue;

				return LastMessage;
			}
		}
		#endregion
	}
}

