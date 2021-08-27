using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Configs;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("config", RoleLevel.Moderator)]
	class Config : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Bot設定を変更・設定します") {
			{ Database.Enums.Language.en_US, "Modify or configure Bot settings" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}config <下記参考> [値(whitelistは不要)]") {
			{ Database.Enums.Language.en_US, "{0}config <Reference below> [Value(Not required for whitelist)]" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			Log.Debug("Start Config");
			string[] msgs = Command_Object.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyText);
				return;
			}

			string config_arg = msgs[0].ToLower();

			switch (config_arg) {
				case "whitelist":
					bool BeforeWhitelist = Database.DatabaseMethods.GuildConfigMethods.WhitelistFind(Command_Object.Guild.Id);
					bool AfterWhitelist = !BeforeWhitelist;
					Database.DatabaseMethods.GuildConfigMethods.WhitelistUpsert(Command_Object.Guild.Id, AfterWhitelist);

					await Command_Object.Channel.SendMessageAsync(AfterWhitelist ? Command_Object.Language.ConfigWhitelistTrue : Command_Object.Language.ConfigWhitelistFalse);
					break;
				case "leaveban":
					bool BeforeLeaveBan = Database.DatabaseMethods.GuildConfigMethods.LeaveBanFind(Command_Object.Guild.Id);
					bool AfterLeaveBan = !BeforeLeaveBan;
					Database.DatabaseMethods.GuildConfigMethods.LeaveBanUpsert(Command_Object.Guild.Id, AfterLeaveBan);

					await Command_Object.Channel.SendMessageAsync(AfterLeaveBan ? Command_Object.Language.ConfigLeaveBanTrue : Command_Object.Language.ConfigLeaveBanFalse);
					break;
				case "prefix":
					if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
						await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.ConfigEmptyValue, config_arg));
						return;
					}

					string AfterPublicPrefix = msgs[1];
					string BeforePublicPrefix = Database.DatabaseMethods.GuildConfigMethods.PublicPrefixFind(Command_Object.Guild.Id);
					if (BeforePublicPrefix == null) BeforePublicPrefix = CommandConfig.Prefix;
					Database.DatabaseMethods.GuildConfigMethods.PublicPrefixUpsert(Command_Object.Guild.Id, AfterPublicPrefix);

					await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.ConfigPublicPrefixChange, BeforePublicPrefix, AfterPublicPrefix));
					break;
				case "logchannel":
					if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
						await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.ConfigEmptyValue, config_arg));
						return;
					}
					if (!ulong.TryParse(msgs[1], out ulong AfterLogChannelID)) {
						await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdCouldntParse);
						return;
					}

					ulong BeforeLogChannelID = Database.DatabaseMethods.GuildConfigMethods.LogChannelFind(Command_Object.Guild.Id);
					Database.DatabaseMethods.GuildConfigMethods.LogChannelIdUpsert(Command_Object.Guild.Id, AfterLogChannelID);

					if (BeforeLogChannelID == 0) {
						await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.ConfigLogChannelIDSet, AfterLogChannelID));
					}
					else {
						await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.ConfigLogChannelIDChange, BeforeLogChannelID, AfterLogChannelID));
					}
					break;
				case "language":
					if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
						await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.ConfigEmptyValue, config_arg));
						return;
					}

					string AfterLanguageString = msgs[1].ToLower();

					if (!Enum.TryParse(AfterLanguageString.Replace('-', '_'), true, out Database.Enums.Language AfterLanguage)) {
						await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.ConfigLanguageNotFound, AfterLanguageString));
						return;
					}

					string BeforeLanguage = Database.DatabaseMethods.GuildConfigMethods.LanguageFind(Command_Object.Guild.Id);
					Database.DatabaseMethods.GuildConfigMethods.LanguageUpsert(Command_Object.Guild.Id, AfterLanguage);

					GetLanguage GetAfterLanguage = new GetLanguage(AfterLanguage);

					if (BeforeLanguage == null) {
						await Command_Object.Channel.SendMessageAsync(string.Format(GetAfterLanguage.Language_Data.ConfigLanguageSet, Enum.GetName(typeof(Database.Enums.Language), AfterLanguage).Replace('_', '-')));
					}
					else {
						await Command_Object.Channel.SendMessageAsync(string.Format(GetAfterLanguage.Language_Data.ConfigLanguageChange, BeforeLanguage.Replace('_', '-'), Enum.GetName(typeof(Database.Enums.Language), AfterLanguage).Replace('_', '-')));
					}
					break;
				case "level":
					bool BeforeLevelSwitch = Database.DatabaseMethods.GuildConfigMethods.LevelSwitchFind(Command_Object.Guild.Id);
					bool AfterLevelSwitch = !BeforeLevelSwitch;
					Database.DatabaseMethods.GuildConfigMethods.LevelSwitchUpsert(Command_Object.Guild.Id, AfterLevelSwitch);

					await Command_Object.Channel.SendMessageAsync(AfterLevelSwitch ? Command_Object.Language.ConfigLevelSwitchTrue : Command_Object.Language.ConfigLevelSwitchFalse);
					break;
				default:
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.ConfigArgsNotFound);
					break;
			}
		}
	}
}
