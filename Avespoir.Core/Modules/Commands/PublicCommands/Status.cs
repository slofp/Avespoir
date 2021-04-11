using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.LevelSystems;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("status")]
		public async Task Status(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				uint Level = Database.DatabaseMethods.UserDataMethods.LevelFind(CommandObject.Message.Author.Id);
				double Exp = Database.DatabaseMethods.UserDataMethods.ExpFind(CommandObject.Message.Author.Id);
				if(Exp == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.StatusNotRegisted);
					return;
				}
				double NextLevelExp = LevelSystem.ReqNextLevelExp(Level) - Exp;

				DiscordMember User = await CommandObject.Guild.GetMemberAsync(CommandObject.Message.Author.Id);

				DiscordEmbed UserStatusEmbed = new DiscordEmbedBuilder()
						.WithTitle(string.Format(CommandObject.Language.StatusEmbed1, string.IsNullOrWhiteSpace(User.Nickname) ? User.Username : User.Nickname))
						.WithDescription(string.Format(CommandObject.Language.StatusEmbed2, User.Username + "#" + User.Discriminator, User.Id, Exp, Level, NextLevelExp))
						.WithColor(new DiscordColor(0x00B06B))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));

				await CommandObject.Message.Channel.SendMessageAsync(UserStatusEmbed);
				return;
			}
			else {
				string UserText = msgs[0];
				string UserIDString = UserText.TrimStart('<', '@', '!').TrimEnd('>');
				if (!ulong.TryParse(UserIDString, out ulong UserID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.StatusUserCouldntParse);
					return;
				}

				uint Level = Database.DatabaseMethods.UserDataMethods.LevelFind(UserID);
				double Exp = Database.DatabaseMethods.UserDataMethods.ExpFind(UserID);
				if (Exp == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.StatusNotRegisted);
					return;
				}
				double NextLevelExp = LevelSystem.ReqNextLevelExp(Level) - Exp;

				try {
					DiscordMember User = await CommandObject.Guild.GetMemberAsync(UserID);

					DiscordEmbed UserStatusEmbed = new DiscordEmbedBuilder()
							.WithTitle(string.Format(CommandObject.Language.StatusEmbed1, string.IsNullOrWhiteSpace(User.Nickname) ? User.Username : User.Nickname))
							.WithDescription(string.Format(CommandObject.Language.StatusEmbed2, User.Username + "#" + User.Discriminator, UserID, Exp, Level, NextLevelExp))
							.WithColor(new DiscordColor(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));

					await CommandObject.Message.Channel.SendMessageAsync(UserStatusEmbed);
				}
				catch (NotFoundException) {
					DiscordEmbed UserStatusEmbed = new DiscordEmbedBuilder()
							.WithTitle(string.Format(CommandObject.Language.StatusEmbed1, UserID.ToString()))
							.WithDescription(string.Format(CommandObject.Language.StatusEmbed2, "Unknown", UserID, Exp, Level, NextLevelExp))
							.WithColor(new DiscordColor(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));

					await CommandObject.Message.Channel.SendMessageAsync(UserStatusEmbed);
				}
			}
		}
	}
}
