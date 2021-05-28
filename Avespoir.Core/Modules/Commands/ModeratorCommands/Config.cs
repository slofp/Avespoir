using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Configs;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("config", RoleLevel.Moderator)]
	class Config : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			Log.Debug("Start Config");
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
				return;
			}

			string config_arg = msgs[0].ToLower();

			switch (config_arg) {
				case "whitelist":
					bool BeforeWhitelist = Database.DatabaseMethods.GuildConfigMethods.WhitelistFind(CommandObject.Guild.Id);
					bool AfterWhitelist = !BeforeWhitelist;
					Database.DatabaseMethods.GuildConfigMethods.WhitelistUpsert(CommandObject.Guild.Id, AfterWhitelist);

					await CommandObject.Message.Channel.SendMessageAsync(AfterWhitelist ? CommandObject.Language.ConfigWhitelistTrue : CommandObject.Language.ConfigWhitelistFalse);
					break;
				case "leaveban":
					bool BeforeLeaveBan = Database.DatabaseMethods.GuildConfigMethods.LeaveBanFind(CommandObject.Guild.Id);
					bool AfterLeaveBan = !BeforeLeaveBan;
					Database.DatabaseMethods.GuildConfigMethods.LeaveBanUpsert(CommandObject.Guild.Id, AfterLeaveBan);

					await CommandObject.Message.Channel.SendMessageAsync(AfterLeaveBan ? CommandObject.Language.ConfigLeaveBanTrue : CommandObject.Language.ConfigLeaveBanFalse);
					break;
				case "publicprefix":
					if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigEmptyValue, config_arg));
						return;
					}

					string AfterPublicPrefix = msgs[1];
					string BeforePublicPrefix = Database.DatabaseMethods.GuildConfigMethods.PublicPrefixFind(CommandObject.Guild.Id);
					if (BeforePublicPrefix == null) BeforePublicPrefix = CommandConfig.PublicPrefix;
					Database.DatabaseMethods.GuildConfigMethods.PublicPrefixUpsert(CommandObject.Guild.Id, AfterPublicPrefix);

					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigPublicPrefixChange, BeforePublicPrefix, AfterPublicPrefix));
					break;
				case "moderatorprefix":
					if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigEmptyValue, config_arg));
						return;
					}

					string AfterModeratorPrefix = msgs[1];
					string BeforeModeratorPrefix = Database.DatabaseMethods.GuildConfigMethods.ModeratorPrefixFind(CommandObject.Guild.Id);
					if (BeforeModeratorPrefix == null) BeforeModeratorPrefix = CommandConfig.ModeratorPrefix;
					Database.DatabaseMethods.GuildConfigMethods.ModeratorPrefixUpsert(CommandObject.Guild.Id, AfterModeratorPrefix);

					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigModeratorPrefixChange, BeforeModeratorPrefix, AfterModeratorPrefix));
					break;
				case "logchannel":
					if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigEmptyValue, config_arg));
						return;
					}
					if (!ulong.TryParse(msgs[1], out ulong AfterLogChannelID)) {
						await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
						return;
					}

					ulong BeforeLogChannelID = Database.DatabaseMethods.GuildConfigMethods.LogChannelFind(CommandObject.Guild.Id);
					Database.DatabaseMethods.GuildConfigMethods.LogChannelIdUpsert(CommandObject.Guild.Id, AfterLogChannelID);

					if (BeforeLogChannelID == 0) {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigLogChannelIDSet, AfterLogChannelID));
					}
					else {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigLogChannelIDChange, BeforeLogChannelID, AfterLogChannelID));
					}
					break;
				case "language":
					if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigEmptyValue, config_arg));
						return;
					}

					string AfterLanguageString = msgs[1].ToLower();

					if (!Enum.TryParse(AfterLanguageString.Replace('-', '_'), true, out Database.Enums.Language AfterLanguage)) {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigLanguageNotFound, AfterLanguageString));
						return;
					}

					string BeforeLanguage = Database.DatabaseMethods.GuildConfigMethods.LanguageFind(CommandObject.Guild.Id);
					Database.DatabaseMethods.GuildConfigMethods.LanguageUpsert(CommandObject.Guild.Id, AfterLanguage);

					GetLanguage GetAfterLanguage = new GetLanguage(AfterLanguage);

					if (BeforeLanguage == null) {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(GetAfterLanguage.Language_Data.ConfigLanguageSet, Enum.GetName(typeof(Database.Enums.Language), AfterLanguage).Replace('_', '-')));
					}
					else {
						await CommandObject.Message.Channel.SendMessageAsync(string.Format(GetAfterLanguage.Language_Data.ConfigLanguageChange, BeforeLanguage.Replace('_', '-'), Enum.GetName(typeof(Database.Enums.Language), AfterLanguage).Replace('_', '-')));
					}
					break;
				case "level":
					bool BeforeLevelSwitch = Database.DatabaseMethods.GuildConfigMethods.LevelSwitchFind(CommandObject.Guild.Id);
					bool AfterLevelSwitch = !BeforeLevelSwitch;
					Database.DatabaseMethods.GuildConfigMethods.LevelSwitchUpsert(CommandObject.Guild.Id, AfterLevelSwitch);

					await CommandObject.Message.Channel.SendMessageAsync(AfterLevelSwitch ? CommandObject.Language.ConfigLevelSwitchTrue : CommandObject.Language.ConfigLevelSwitchFalse);
					break;
				default:
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.ConfigArgsNotFound);
					break;
			}
		}
	}
}
