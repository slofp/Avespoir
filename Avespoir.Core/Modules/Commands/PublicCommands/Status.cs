using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.LevelSystems;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using Avespoir.Core.Modules.Visualize;
using Avespoir.Core.Modules.Assets;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("status", RoleLevel.Public)]
	class Status : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("ステータスを表示します") {
			{ Database.Enums.Language.en_US, "Show user status" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}status (メンションかユーザーID)") {
			{ Database.Enums.Language.en_US, "{0}status (Mention or UserID)" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);

			VisualGenerator Visual = new VisualGenerator();
			if (msgs.Length == 0) {
				uint Level = Database.DatabaseMethods.UserDataMethods.LevelFind(Command_Object.Author.Id);
				double Exp = Database.DatabaseMethods.UserDataMethods.ExpFind(Command_Object.Author.Id);
				if (Exp == 0) {
					Visual.AddEmbed(Command_Object.Language.StatusNotRegisted, EmbedColorAsset.FailedColor);
					await Command_Object.Channel.SendMessageAsync(Visual.Generate());
					return;
				}
				double NextLevelExp = LevelSystem.ReqNextLevelExp(Level) - Exp;

				DiscordMember User = await Command_Object.Guild.GetMemberAsync(Command_Object.Author.Id).ConfigureAwait(false);

				Visual.AddEmbed(
					string.Format(Command_Object.Language.StatusEmbed1, string.IsNullOrWhiteSpace(User.Nickname) ? User.Username : User.Nickname),
					string.Format(Command_Object.Language.StatusEmbed2, User.Username + "#" + User.Discriminator, User.Id, Exp, Level, NextLevelExp),
					EmbedColorAsset.SuccessColor
				);

				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
				return;
			}
			else {
				string UserText = msgs[0];
				string UserIDString = UserText.TrimStart('<', '@', '!').TrimEnd('>');
				if (!ulong.TryParse(UserIDString, out ulong UserID)) {
					Visual.AddEmbed(Command_Object.Language.StatusUserCouldntParse, EmbedColorAsset.FailedColor);
					await Command_Object.Channel.SendMessageAsync(Visual.Generate());
					return;
				}

				uint Level = Database.DatabaseMethods.UserDataMethods.LevelFind(UserID);
				double Exp = Database.DatabaseMethods.UserDataMethods.ExpFind(UserID);
				if (Exp == 0) {
					Visual.AddEmbed(Command_Object.Language.StatusNotRegisted, EmbedColorAsset.FailedColor);
					await Command_Object.Channel.SendMessageAsync(Visual.Generate());
					return;
				}
				double NextLevelExp = LevelSystem.ReqNextLevelExp(Level) - Exp;

				DiscordMember User = await Command_Object.Guild.GetMemberAsync(UserID).ConfigureAwait(false);

				if (User is null) {
					Visual.AddEmbed(
						string.Format(Command_Object.Language.StatusEmbed1, UserID.ToString()),
						string.Format(Command_Object.Language.StatusEmbed2, "Unknown", UserID, Exp, Level, NextLevelExp),
						EmbedColorAsset.SuccessColor
					);

					await Command_Object.Channel.SendMessageAsync(Visual.Generate());
				}
				else {
					Visual.AddEmbed(
						string.Format(Command_Object.Language.StatusEmbed1, string.IsNullOrWhiteSpace(User.Nickname) ? User.Username : User.Nickname),
						string.Format(Command_Object.Language.StatusEmbed2, User.Username + "#" + User.Discriminator, UserID, Exp, Level, NextLevelExp),
						EmbedColorAsset.SuccessColor
					);

					await Command_Object.Channel.SendMessageAsync(Visual.Generate());
				}
			}
		}
	}
}
