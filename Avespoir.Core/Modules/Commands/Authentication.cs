using Avespoir.Core.Extends;
using Discord;
using Discord.Rest;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.IMessageExtension;
using static Avespoir.Core.Modules.Utils.RandomCodeGenerator;

namespace Avespoir.Core.Modules.Commands {

	class Authentication {

		internal static async Task<bool> Confirmation(CommandObject Command_Object) {
			string AuthCode = RandomCodeGenerate();
			await (await Command_Object.Member.GetOrCreateDMChannelAsync()).SendMessageAsync(AuthCode);
			RestUserMessage BotSendMessage = await Command_Object.Channel.SendMessageAsync("処理を実行する前に確認のためDMに送信した6文字のコードを入力してください");

			IMessage AuthSend = await BotSendMessage.AwaitMessage(Command_Object.Author.Id, 1 * 60 * 1000);

			if (AuthSend == null) {
				return false;
			}

			if (AuthSend.Content != AuthCode) {
				await Command_Object.Channel.SendMessageAsync("コードが違います");
				return false;
			}

			return true;
		}
	}
}
