using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.LevelSystems;
using Avespoir.Core.Modules.Utils;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

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
			if (msgs.Length == 0) {
				uint Level = Database.DatabaseMethods.UserDataMethods.LevelFind(Command_Object.Author.Id);
				double Exp = Database.DatabaseMethods.UserDataMethods.ExpFind(Command_Object.Author.Id);
				if (Exp == 0) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.StatusNotRegisted);
					return;
				}
				double NextLevelExp = LevelSystem.ReqNextLevelExp(Level) - Exp;

				SocketGuildUser User = Command_Object.Guild.GetUser(Command_Object.Author.Id);

				EmbedBuilder UserStatusEmbed = new EmbedBuilder()
						.WithTitle(string.Format(Command_Object.Language.StatusEmbed1, string.IsNullOrWhiteSpace(User.Nickname) ? User.Username : User.Nickname))
						.WithDescription(string.Format(Command_Object.Language.StatusEmbed2, User.Username + "#" + User.Discriminator, User.Id, Exp, Level, NextLevelExp))
						.WithColor(new Color(0x00B06B))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));

				await Command_Object.Channel.SendMessageAsync(embed: UserStatusEmbed.Build());
				return;
			}
			else {
				string UserText = msgs[0];
				string UserIDString = UserText.TrimStart('<', '@', '!').TrimEnd('>');
				if (!ulong.TryParse(UserIDString, out ulong UserID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.StatusUserCouldntParse);
					return;
				}

				uint Level = Database.DatabaseMethods.UserDataMethods.LevelFind(UserID);
				double Exp = Database.DatabaseMethods.UserDataMethods.ExpFind(UserID);
				if (Exp == 0) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.StatusNotRegisted);
					return;
				}
				double NextLevelExp = LevelSystem.ReqNextLevelExp(Level) - Exp;

				SocketGuildUser User = Command_Object.Guild.GetUser(UserID);

				if (User is null) {
					EmbedBuilder UserStatusEmbed = new EmbedBuilder()
						.WithTitle(string.Format(Command_Object.Language.StatusEmbed1, UserID.ToString()))
						.WithDescription(string.Format(Command_Object.Language.StatusEmbed2, "Unknown", UserID, Exp, Level, NextLevelExp))
						.WithColor(new Color(0x00B06B))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));

					await Command_Object.Channel.SendMessageAsync(embed: UserStatusEmbed.Build());
				}
				else {
					EmbedBuilder UserStatusEmbed = new EmbedBuilder()
						.WithTitle(string.Format(Command_Object.Language.StatusEmbed1, string.IsNullOrWhiteSpace(User.Nickname) ? User.Username : User.Nickname))
						.WithDescription(string.Format(Command_Object.Language.StatusEmbed2, User.Username + "#" + User.Discriminator, UserID, Exp, Level, NextLevelExp))
						.WithColor(new Color(0x00B06B))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));

					await Command_Object.Channel.SendMessageAsync(embed: UserStatusEmbed.Build());
				}
			}
		}
	}
}
