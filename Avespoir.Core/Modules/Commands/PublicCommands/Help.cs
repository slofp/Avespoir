using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Configs;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("help", RoleLevel.Public)]
	class Help : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("コマンド一覧を表示します") {
			{ Database.Enums.Language.en_US, "Show command list" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}help") {
			{ Database.Enums.Language.en_US, "{0}help" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			await CommandObject.Channel.SendMessageAsync(string.Format(CommandObject.Language.DMMention, CommandObject.Member.Mention));

			string GuildPrefix = Database.DatabaseMethods.GuildConfigMethods.PrefixFind(CommandObject.Guild.Id);
			if (GuildPrefix == null) GuildPrefix = CommandConfig.Prefix;

			DiscordEmbedBuilder PublicEmbed = new DiscordEmbedBuilder();
			DiscordEmbedBuilder ModeratorEmbed = new DiscordEmbedBuilder();
			DiscordEmbedBuilder BotownerEmbed = new DiscordEmbedBuilder();
			foreach (CommandInfo Command_Info in CommandInfo.GetCommandInfo()) {
				if (Command_Info.Command_Attribute.CommandName == null) continue;

				switch (Command_Info.Command_Attribute.CommandRoleLevel) {
					case RoleLevel.Public:
						PublicEmbed.AddField(
							Command_Info.Command.Description[CommandObject.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[CommandObject.LanguageType]}`", GuildPrefix)
						);
						break;
					case RoleLevel.Moderator:
						ModeratorEmbed.AddField(
							Command_Info.Command.Description[CommandObject.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[CommandObject.LanguageType]}`", GuildPrefix)
						);
						break;
					case RoleLevel.Owner:
						BotownerEmbed.AddField(
							Command_Info.Command.Description[CommandObject.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[CommandObject.LanguageType]}`", CommandConfig.Prefix)
						);
						break;
					default:
						break;
				}
			}

			PublicEmbed
				.WithTitle(CommandObject.Language.HelpPublicCommand)
				.WithDescription(string.Format(CommandObject.Language.HelpCommandPrefix, GuildPrefix))
				.WithColor(new DiscordColor(0x00B06B))
				.WithTimestamp(DateTime.Now)
				.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
			await CommandObject.Member.SendMessageAsync(PublicEmbed);

			RoleLevel DBRoleLevel =
					CommandObject.Message.Author.Id == CommandObject.Guild.Owner.Id ||
					CommandObject.Message.Author.Id == ClientConfig.BotownerId ? RoleLevel.Moderator :
					Database.DatabaseMethods.AllowUsersMethods.AllowUserFind(CommandObject.Guild.Id, CommandObject.Message.Author.Id, out AllowUsers DBAllowUsersID) &&
					Database.DatabaseMethods.RolesMethods.RoleFind(CommandObject.Guild.Id, DBAllowUsersID.RoleNum, out Roles DBRolesNum) ? (RoleLevel) Enum.Parse(typeof(RoleLevel), DBRolesNum.RoleLevel) :
					RoleLevel.Public;

			if (DBRoleLevel == RoleLevel.Moderator) { // CommandObject.Message.Author.Id == CommandObject.Guild.Owner.Id
				ModeratorEmbed
					.WithTitle(CommandObject.Language.HelpModeratorCommand)
					.WithDescription(string.Format(CommandObject.Language.HelpCommandPrefix, GuildPrefix))
					.AddField(
						CommandObject.Language.HelpConfigArgs,
						"`" + "whitelist" + " | " + "leaveban" + " | " + "publicprefix" + " | " + "moderatorprefix" + " | " + "logchannel" + " | " + "language" + " | " + "level" + "`"
					)
					.WithColor(new DiscordColor(0xF6AA00))
					.WithTimestamp(DateTime.Now)
					.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
				await CommandObject.Member.SendMessageAsync(ModeratorEmbed);
			}

			if (CommandObject.Message.Author.Id == ClientConfig.BotownerId) {
				BotownerEmbed
					.WithTitle("Botowner Commands")
					.WithDescription(string.Format("Prefix is {0}", CommandConfig.Prefix))
					.WithColor(new DiscordColor(0x1971FF))
					.WithTimestamp(DateTime.Now)
					.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
				await CommandObject.Member.SendMessageAsync(BotownerEmbed);
			}
		}
	}
}
