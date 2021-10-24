using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Configs;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using Avespoir.Core.Database.DatabaseMethods;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("help", RoleLevel.Public)]
	class Help : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("コマンド一覧を表示します") {
			{ Database.Enums.Language.en_US, "Show command list" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}help") {
			{ Database.Enums.Language.en_US, "{0}help" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.DMMention, Command_Object.Member.Mention));

			string GuildPrefix = GuildConfigMethods.PrefixFind(Command_Object.Guild.Id);
			if (GuildPrefix == null) GuildPrefix = CommandConfig.Prefix;

			DiscordEmbedBuilder PublicEmbed = new DiscordEmbedBuilder();
			DiscordEmbedBuilder ModeratorEmbed = new DiscordEmbedBuilder();
			DiscordEmbedBuilder BotownerEmbed = new DiscordEmbedBuilder();
			foreach (CommandInfo Command_Info in CommandInfo.GetCommandInfo()) {
				if (Command_Info.Command_Attribute.CommandName == null) continue;

				switch (Command_Info.Command_Attribute.CommandRoleLevel) {
					case RoleLevel.Public:
						PublicEmbed.AddField(
							Command_Info.Command.Description[Command_Object.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[Command_Object.LanguageType]}`", GuildPrefix)
						);
						break;
					case RoleLevel.Moderator:
						ModeratorEmbed.AddField(
							Command_Info.Command.Description[Command_Object.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[Command_Object.LanguageType]}`", GuildPrefix)
						);
						break;
					case RoleLevel.Owner:
						BotownerEmbed.AddField(
							Command_Info.Command.Description[Command_Object.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[Command_Object.LanguageType]}`", CommandConfig.Prefix)
						);
						break;
					default:
						break;
				}
			}

			PublicEmbed
				.WithTitle(Command_Object.Language.HelpPublicCommand)
				.WithDescription(string.Format(Command_Object.Language.HelpCommandPrefix, GuildPrefix))
				.WithColor(new DiscordColor(0x00B06B))
				.WithTimestamp(DateTime.Now)
				.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));
			await Command_Object.Member.SendMessageAsync(embed: PublicEmbed.Build());

			RoleLevel DBRoleLevel =
					Command_Object.Author.Id == Command_Object.Guild.Owner.Id ||
					Command_Object.Author.Id == ClientConfig.BotownerId ? RoleLevel.Moderator :
					AllowUsersMethods.AllowUserFind(Command_Object.Guild.Id, Command_Object.Author.Id, out AllowUsers DBAllowUsersID) &&
					RolesMethods.RoleFind(Command_Object.Guild.Id, DBAllowUsersID.RoleNum, out Roles DBRolesNum) ? DBRolesNum.RoleLevel : RoleLevel.Public;

			if (DBRoleLevel == RoleLevel.Moderator) { // CommandObject.Message.Author.Id == CommandObject.Guild.Owner.Id
				ModeratorEmbed
					.WithTitle(Command_Object.Language.HelpModeratorCommand)
					.WithDescription(string.Format(Command_Object.Language.HelpCommandPrefix, GuildPrefix))
					.AddField(
						Command_Object.Language.HelpConfigArgs,
						"`" + "whitelist" + " | " + "leaveban" + " | " + "prefix" + " | " + "logchannel" + " | " + "language" + " | " + "level" + "`"
					)
					.WithColor(new DiscordColor(0xF6AA00))
					.WithTimestamp(DateTime.Now)
					.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));
				await Command_Object.Member.SendMessageAsync(embed: ModeratorEmbed.Build());
			}

			if (Command_Object.Author.Id == ClientConfig.BotownerId) {
				BotownerEmbed
					.WithTitle("Botowner Commands")
					.WithDescription(string.Format("Prefix is {0}", CommandConfig.Prefix))
					.WithColor(new DiscordColor(0x1971FF))
					.WithTimestamp(DateTime.Now)
					.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));
				await Command_Object.Member.SendMessageAsync(embed: BotownerEmbed.Build());
			}
		}
	}
}
