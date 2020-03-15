using DSharpPlus.Entities;
using System.Threading.Tasks;
using static AvespoirTest.Core.Modules.Utils.DiscordMessageExtension;
using static AvespoirTest.Core.Modules.Utils.RandomCodeGenerator;

namespace AvespoirTest.Core.Modules.Commands {

	class Authentication {

		internal static async Task<bool> Confirmation(CommandObjects CommandObject) {
			string AuthCode = RandomCodeGenerate();
			await CommandObject.Member.SendMessageAsync(AuthCode);
			DiscordMessage BotSendMessage = await CommandObject.Message.Channel.SendMessageAsync("処理を実行する前に確認のためDMに送信した6文字のコードを入力してください");

			bool check = false;
			while (!check) {
				DiscordMessage LastMessage = await CommandObject.Channel.GetMessageAsync(CommandObject.Channel.LastMessageId);
				if (BotSendMessage.Id == LastMessage.Id) check = true;
			};

			DiscordMessage AuthSend = await BotSendMessage.AwaitMessage(CommandObject.Message.Author.Id, 2 * 60 * 1000);

			if (AuthSend.Content != AuthCode) {
				await CommandObject.Message.Channel.SendMessageAsync("コードが違います");
				return false;
			}

			return true;
		}
	}
}
