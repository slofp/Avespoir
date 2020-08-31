using Avespoir.Core.Attributes;
using Avespoir.Core.Configs;
using Avespoir.Core.Database;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("config")]
		public async Task Config(CommandObjects CommandObject) {
			Log.Debug("Start Config");
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
				return;
			}

			bool a = msgs.Length >= 2;

			string config_arg = msgs[0].ToLower();
			if (config_arg == "whitelist") {
				bool BeforeWhitelist = await DatabaseMethods.WhitelistFind(CommandObject.Guild.Id).ConfigureAwait(false);
				if (BeforeWhitelist) {
					bool AfterWhitelist = false;
					await DatabaseMethods.WhitelistUpsert(CommandObject.Guild.Id, AfterWhitelist).ConfigureAwait(false);

					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.ConfigWhitelistFalse);
				}
				else {
					bool AfterWhitelist = true;
					await DatabaseMethods.WhitelistUpsert(CommandObject.Guild.Id, AfterWhitelist).ConfigureAwait(false);

					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.ConfigWhitelistTrue);
				}
			}
			else if (config_arg == "leaveban") {
				bool BeforeLeaveBan = await DatabaseMethods.LeaveBanFind(CommandObject.Guild.Id).ConfigureAwait(false);
				if (BeforeLeaveBan) {
					bool AfterLeaveBan = false;
					await DatabaseMethods.LeaveBanUpsert(CommandObject.Guild.Id, AfterLeaveBan).ConfigureAwait(false);

					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.ConfigLeaveBanFalse);
				}
				else {
					bool AfterLeaveBan = true;
					await DatabaseMethods.LeaveBanUpsert(CommandObject.Guild.Id, AfterLeaveBan).ConfigureAwait(false);

					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.ConfigLeaveBanTrue);
				}
			}
			else if (config_arg == "publicprefix") {
				if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigEmptyValue, config_arg));
					return;
				}

				string AfterPublicPrefix = msgs[1];
				string BeforePublicPrefix = await DatabaseMethods.PublicPrefixFind(CommandObject.Guild.Id).ConfigureAwait(false);
				if (BeforePublicPrefix == null) BeforePublicPrefix = CommandConfig.PublicPrefix;
				await DatabaseMethods.PublicPrefixUpsert(CommandObject.Guild.Id, AfterPublicPrefix).ConfigureAwait(false);

				await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigPublicPrefixChange, BeforePublicPrefix, AfterPublicPrefix));
			}
			else if (config_arg == "moderatorprefix") {
				if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigEmptyValue, config_arg));
					return;
				}

				string AfterModeratorPrefix = msgs[1];
				string BeforeModeratorPrefix = await DatabaseMethods.ModeratorPrefixFind(CommandObject.Guild.Id).ConfigureAwait(false);
				if (BeforeModeratorPrefix == null) BeforeModeratorPrefix = CommandConfig.ModeratorPrefix;
				await DatabaseMethods.ModeratorPrefixUpsert(CommandObject.Guild.Id, AfterModeratorPrefix).ConfigureAwait(false);

				await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigModeratorPrefixChange, BeforeModeratorPrefix, AfterModeratorPrefix));
			}
			else if (config_arg == "logchannel") {
				if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigEmptyValue, config_arg));
					return;
				}
				if (!ulong.TryParse(msgs[1], out ulong AfterLogChannelID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
					return;
				}

				ulong BeforeLogChannelID = await DatabaseMethods.LogChannelFind(CommandObject.Guild.Id).ConfigureAwait(false);
				await DatabaseMethods.LogChannelIdUpsert(CommandObject.Guild.Id, AfterLogChannelID).ConfigureAwait(false);

				if (BeforeLogChannelID == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigLogChannelIDSet, AfterLogChannelID));
				}
				else {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigLogChannelIDChange, BeforeLogChannelID, AfterLogChannelID));
				}
			}
			else if (config_arg == "language") {
				if (msgs.Length < 2 || string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigEmptyValue, config_arg));
					return;
				}

				string AfterLanguageString = msgs[1].ToLower();

				if (!Enum.TryParse(AfterLanguageString.Replace('-', '_'), true, out Database.Enums.Language AfterLanguage)) {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigLanguageNotFound, AfterLanguageString));
					return;
				}

				string BeforeLanguage = await DatabaseMethods.LanguageFind(CommandObject.Guild.Id).ConfigureAwait(false);
				await DatabaseMethods.LanguageUpsert(CommandObject.Guild.Id, AfterLanguage).ConfigureAwait(false);

				if (BeforeLanguage == null) {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigLanguageSet, Enum.GetName(typeof(Database.Enums.Language), AfterLanguage).Replace('_', '-')));
				}
				else {
					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.ConfigLanguageChange, BeforeLanguage.Replace('_', '-'), Enum.GetName(typeof(Database.Enums.Language), AfterLanguage).Replace('_', '-')));
				}
			}
			else {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.ConfigArgsNotFound);
			}
		}
	}
}
