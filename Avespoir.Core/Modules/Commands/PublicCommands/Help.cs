using System.Collections.Generic;
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
using Avespoir.Core.Modules.Visualize;
using Avespoir.Core.Modules.Assets;

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

			VisualGenerator Visual = new VisualGenerator();

			Dictionary<string, string> PublicFields = new Dictionary<string, string>();
			Dictionary<string, string> ModeratorFields = new Dictionary<string, string>();
			Dictionary<string, string> BotownerFields = new Dictionary<string, string>();
			foreach (CommandInfo Command_Info in CommandInfo.GetCommandInfo()) {
				if (Command_Info.Command_Attribute.CommandName == null) continue;

				switch (Command_Info.Command_Attribute.CommandRoleLevel) {
					case RoleLevel.Public:
						PublicFields.Add(
							Command_Info.Command.Description[Command_Object.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[Command_Object.LanguageType]}`", GuildPrefix)
						);
						break;
					case RoleLevel.Moderator:
						ModeratorFields.Add(
							Command_Info.Command.Description[Command_Object.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[Command_Object.LanguageType]}`", GuildPrefix)
						);
						break;
					case RoleLevel.Owner:
						BotownerFields.Add(
							Command_Info.Command.Description[Command_Object.LanguageType],
							string.Format($"`{Command_Info.Command.Usage[Command_Object.LanguageType]}`", CommandConfig.Prefix)
						);
						break;
				}
			}

			RoleLevel DBRoleLevel =
					Command_Object.Author.Id == Command_Object.Guild.Owner.Id ||
					Command_Object.Author.Id == ClientConfig.BotownerId ? RoleLevel.Moderator :
					AllowUsersMethods.AllowUserFind(Command_Object.Guild.Id, Command_Object.Author.Id, out AllowUsers DBAllowUsersID) &&
					RolesMethods.RoleFind(Command_Object.Guild.Id, DBAllowUsersID.RoleNum, out Roles DBRolesNum) ? DBRolesNum.RoleLevel : RoleLevel.Public;

			Visual.AddEmbed(
				Command_Object.Language.HelpPublicCommand,
				string.Format(Command_Object.Language.HelpCommandPrefix, GuildPrefix),
				EmbedColorAsset.SuccessColor,
				Embed => {
					foreach ((string Name, string Value) in PublicFields)
						Embed.AddField(Name, Value);
				}
			);

			if (DBRoleLevel == RoleLevel.Moderator) { // CommandObject.Message.Author.Id == CommandObject.Guild.Owner.Id
				Visual.AddEmbed(
					Command_Object.Language.HelpModeratorCommand,
					string.Format(Command_Object.Language.HelpCommandPrefix, GuildPrefix),
					EmbedColorAsset.DangerColor,
					Embed => {
						foreach ((string Name, string Value) in ModeratorFields)
							Embed.AddField(Name, Value);

						Embed.AddField(
							Command_Object.Language.HelpConfigArgs,
							"`" + "whitelist" + " | " + "leaveban" + " | " + "prefix" + " | " + "logchannel" + " | " + "language" + " | " + "level" + "`"
						);
					}
				);
			}

			if (Command_Object.Author.Id == ClientConfig.BotownerId) {
				Visual.AddEmbed(
					"Botowner Commands",
					string.Format("Prefix is {0}", CommandConfig.Prefix),
					EmbedColorAsset.NormalColor,
					Embed => {
						foreach ((string Name, string Value) in BotownerFields)
							Embed.AddField(Name, Value);
					}
				);
			}

			await Command_Object.Member.SendMessageAsync(Visual.Generate());
		}
	}
}
