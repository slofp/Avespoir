using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Utils {

	static class DiscordMessageExtension {

		#region AwaitMessage Methods

		internal static async Task<DiscordMessage> AwaitMessage(this DiscordMessage Discord_Message, int Timeout) {
			Task<DiscordMessage> GetMessageTask = GetMessage(Discord_Message);

			await Task.WhenAny(GetMessageTask, Task.Delay(Timeout)).ConfigureAwait(false);

			if (GetMessageTask.IsCompleted) return GetMessageTask.Result;
			else return null;
		}

		internal static async Task<DiscordMessage> AwaitMessage(this DiscordMessage Discord_Message, ulong AllowAuthorID, int Timeout) {
			Task<DiscordMessage> GetMessageTask = GetMessage(Discord_Message, AllowAuthorID);

			await Task.WhenAny(GetMessageTask, Task.Delay(Timeout)).ConfigureAwait(false);

			if (GetMessageTask.IsCompleted) return GetMessageTask.Result;
			else return null;
		}
		#endregion

		#region GetMessage Methods

		static async Task<DiscordMessage> GetMessage(DiscordMessage Discord_Message) {
			while (true) {
				DiscordMessage LastMessage = await Discord_Message.Channel.GetMessageAsync(Discord_Message.Channel.LastMessageId);

				if (LastMessage.Id != Discord_Message.Id) continue;
				
				return LastMessage;
			}
		}

		static async Task<DiscordMessage> GetMessage(DiscordMessage Discord_Message, ulong AllowAuthorID) {
			while (true) {
				DiscordMessage LastMessage = await Discord_Message.Channel.GetMessageAsync(Discord_Message.Channel.LastMessageId);

				if (LastMessage.Id == Discord_Message.Id) continue;
				if (LastMessage.Author.Id != AllowAuthorID) continue;
				
				return LastMessage;
			}
		}
		#endregion
	}
}
