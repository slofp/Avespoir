using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.BotownerCommands {

	[Command("userdata_reset", RoleLevel.Owner)]
	class UserDataReset : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			DiscordMessage RespondMessage = await CommandObject.Message.RespondAsync("Level resetting...").ConfigureAwait(false);
			Log.Info("Level resetting...");

			if (LiteDBClient.Database.DropCollection(typeof(UserData).Name))
				await RespondMessage.ModifyAsync("UserData removed").ConfigureAwait(false);
			else await RespondMessage.ModifyAsync("UserData couldn't removed").ConfigureAwait(false);

			Log.Info("UserData Removed");
		}
	}
}
