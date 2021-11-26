using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using Avespoir.Core.Modules.Voice;
using DSharpPlus.VoiceNext;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("rmtts", RoleLevel.Public)]
	class RemoveTTS : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyText);
				return;
			}

			VoiceNextConnection Connection = Client.Bot.GetVoiceNext().GetConnection(Command_Object.Guild);
			if (Connection is null) {
				await Command_Object.Channel.SendMessageAsync("VCに入っていません");
				return;
			}

			if (!VoiceStatus.GuildsVoiceStatus.TryGetValue(Command_Object.Guild.Id, out VoiceStatus GuildVoiceStatus)) {
				Log.Error("No exist VoiceStatus.");
				await Command_Object.Channel.SendMessageAsync("エラーが発生しました");
				return;
			}

			if (!GuildVoiceStatus.IsTTS) {
				await Command_Object.Channel.SendMessageAsync("TTS機能が有効ではありません");
				return;
			}

			string Replace_Text = msgs[0];
			if (Replace_Text != "all") {
				if (PatternMatch.MatchID(Replace_Text)) Replace_Text = PatternMatch.ExtructID(msgs[0]);
				if (!ulong.TryParse(Replace_Text, out ulong AddChannelID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdCouldntParse);
					return;
				}

				GuildVoiceStatus.ReadChannels.Remove(AddChannelID);
			}
			else GuildVoiceStatus.ReadChannels.Clear();
		}
	}
}
