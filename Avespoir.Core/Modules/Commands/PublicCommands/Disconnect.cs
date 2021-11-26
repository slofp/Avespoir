using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Voice;
using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("disconnect", RoleLevel.Public)]
	class Disconnect : CommandAbstruct {


		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			VoiceNextConnection Connection = Client.Bot.GetVoiceNext().GetConnection(Command_Object.Guild);
			if (Connection is null) {
				await Command_Object.Channel.SendMessageAsync("ありませんでした").ConfigureAwait(false);
				return;
			}

			Connection.Disconnect();

			VoiceStatus.GuildsVoiceStatus.Remove(Command_Object.Guild.Id);
		}
	}
}
