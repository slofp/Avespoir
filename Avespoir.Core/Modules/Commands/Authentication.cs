using Avespoir.Core.Extends;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.DiscordMessageExtension;
using static Avespoir.Core.Modules.Utils.RandomCodeGenerator;
using Avespoir.Core.Modules.Logger;

namespace Avespoir.Core.Modules.Commands {

	class Authentication {

		internal static async Task<bool> Confirmation(CommandObject Command_Object) {
			string AuthCode = RandomCodeGenerate();
			try {
				await Command_Object.Member.SendMessageAsync(AuthCode);
			}
			catch {
				Log.Warning("Could not send DM.");
				await Command_Object.Channel.SendMessageAsync("DMに送信ができないため処理を取り消しします");
				return false;
			}
			DiscordMessage BotSendMessage = await Command_Object.Channel.SendMessageAsync("処理を実行する前に確認のためDMに送信した6文字のコードを入力してください");

			DiscordMessage AuthSend = await BotSendMessage.AwaitMessage(Command_Object.Author.Id, 1 * 60 * 1000);

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
