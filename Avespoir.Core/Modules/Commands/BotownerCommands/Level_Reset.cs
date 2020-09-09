using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class BotownerCommands {

		[Command("userdata_reset")]
		public async Task Level_Reset(CommandObjects CommandObject) {
			DiscordMessage RespondMessage = await CommandObject.Message.RespondAsync("Level resetting...").ConfigureAwait(false);
			Log.Info("Level resetting...");

			await MongoDBClient.Database.DropCollectionAsync(typeof(UserData).Name).ConfigureAwait(false);

			await RespondMessage.ModifyAsync("UserData Removed").ConfigureAwait(false);
			Log.Info("UserData Removed");
		}
	}
}
