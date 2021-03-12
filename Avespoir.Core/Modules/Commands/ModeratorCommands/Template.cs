using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.Moderator_Commands {

	[Command()]
	class Template : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Excute(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
				return;
			}
			await Task.Delay(0);
		}
	}
}
