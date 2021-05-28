using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.BotownerCommands {

	[Command("exit", RoleLevel.Owner)]
	class Exit : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			await CommandObject.Message.RespondAsync("Logging out...");
			Log.Info("Logging out...");

			await ConsoleExitEvent.Main(0).ConfigureAwait(false);
		}
	}
}
