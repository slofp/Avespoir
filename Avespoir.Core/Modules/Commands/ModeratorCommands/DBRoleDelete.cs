using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("db-roledel", RoleLevel.Moderator)]
	class DBRoleDelete : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[0], out ulong msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
					return;
				}

				if (!Database.DatabaseMethods.RolesMethods.RoleFind(CommandObject.Guild.Id, msgs_ID, out Roles DBRolesID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdNotRegisted);
					return;
				}
				// if DBRolesID is null, processes will not be executed from here.

				if (!await Authentication.Confirmation(CommandObject)) {
					await CommandObject.Channel.SendMessageAsync(CommandObject.Language.AuthFailure);
					return;
				}

				Database.DatabaseMethods.RolesMethods.RoleDelete(DBRolesID);

				DiscordRole GuildRole = CommandObject.Guild.GetRole(DBRolesID.Uuid);
				string ResultText = string.Format(CommandObject.Language.DBRoleDeleteSuccess, GuildRole.Name, DBRolesID.Uuid);
				await CommandObject.Message.Channel.SendMessageAsync(ResultText);
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.TypingMissed);
			}
		}
	}
}
