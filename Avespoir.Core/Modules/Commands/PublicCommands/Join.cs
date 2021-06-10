using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	[Command("join", RoleLevel.Public)]
	class Join : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("ボイスチャットに入ります") {
			{ Database.Enums.Language.en_US, "Join the voice chat" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}join") {
			{ Database.Enums.Language.en_US, "{0}join" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
				return;
			}


			//CommandObject.Member.VoiceState.Channel.
			await Task.Delay(0);
		}
	}
}
