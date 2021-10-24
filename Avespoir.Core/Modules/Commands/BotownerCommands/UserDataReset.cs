using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.BotownerCommands {

	[Command("userdata_reset", RoleLevel.Owner)]
	class UserDataReset : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Reset all levels");

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}userdata_reset");

		internal override async Task Execute(CommandObject Command_Object) {
			DiscordMessage RespondMessage = await Command_Object.Channel.SendMessageAsync("Level resetting...").ConfigureAwait(false);
			Log.Info("Level resetting...");
			try {
				await MongoDBClient.DropUserData().ConfigureAwait(false);

				await RespondMessage.ModifyAsync(MessagePropertie => MessagePropertie.Content = "UserData removed").ConfigureAwait(false);
			}
			catch {

				await RespondMessage.ModifyAsync(MessagePropertie => MessagePropertie.Content = "UserData couldn't removed").ConfigureAwait(false);
			}

			Log.Info("UserData Removed");
		}
	}
}
