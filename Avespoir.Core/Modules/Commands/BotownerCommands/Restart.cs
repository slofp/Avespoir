using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.BotownerCommands {

	[Command("restart", RoleLevel.Owner)]
	class Restart : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			await CommandObject.Message.RespondAsync("Restarting...");
			Log.Info("Restarting...");

			await ConsoleExitEvent.Main(-1);
		}
	}
}
