using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.BotownerCommands {

	[Command("exit", RoleLevel.Owner)]
	class Exit : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Logout This Bot");

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}exit");

		internal override async Task Execute(CommandObject Command_Object) {
			await Command_Object.Channel.SendMessageAsync("Logging out...").ConfigureAwait(false);
			Log.Info("Logging out...");

			await ConsoleExitEvent.Main(0).ConfigureAwait(false);
		}
	}
}
