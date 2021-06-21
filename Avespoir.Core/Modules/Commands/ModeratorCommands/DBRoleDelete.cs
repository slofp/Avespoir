using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("db-roledel", RoleLevel.Moderator)]
	class DBRoleDelete : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Roleデータベースから役職を削除します") {
			{ Database.Enums.Language.en_US, "Removes Role from the Role database" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}db-roledel [役職ID]") {
			{ Database.Enums.Language.en_US, "{0}db-roledel [RoleID]" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			try {
				string[] msgs = Command_Object.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyText);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[0], out ulong msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdCouldntParse);
					return;
				}

				if (!Database.DatabaseMethods.RolesMethods.RoleFind(Command_Object.Guild.Id, msgs_ID, out Roles DBRolesID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdNotRegisted);
					return;
				}
				// if DBRolesID is null, processes will not be executed from here.

				if (!await Authentication.Confirmation(Command_Object)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.AuthFailure);
					return;
				}

				Database.DatabaseMethods.RolesMethods.RoleDelete(DBRolesID);

				SocketRole GuildRole = Command_Object.Guild.GetRole(DBRolesID.Uuid);
				string ResultText = string.Format(Command_Object.Language.DBRoleDeleteSuccess, GuildRole.Name, DBRolesID.Uuid);
				await Command_Object.Channel.SendMessageAsync(ResultText);
			}
			catch (IndexOutOfRangeException) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.TypingMissed);
			}
		}
	}
}
