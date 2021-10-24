using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("db-useradd", RoleLevel.Moderator)]
	class DBUserAdd : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Userデータベースにユーザーを追加します") {
			{ Database.Enums.Language.en_US, "Add a user to the User database" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}db-useradd [名前] [ユーザーID] [役職登録番号]") {
			{ Database.Enums.Language.en_US, "{0}db-useradd [Name] [UserID] [Role Number]" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			try {
				string[] msgs = Command_Object.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyText);
					return;
				}

				string msgs_Name;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyName);
					return;
				}
				msgs_Name = msgs[0];

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[1], out ulong msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdCouldntParse);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[2])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyRoleNumber);
					return;
				}
				if (!uint.TryParse(msgs[2], out uint msgs_RoleNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleNumberNotNumber);
					return;
				}

				if (Database.DatabaseMethods.AllowUsersMethods.AllowUserExist(Command_Object.Guild.Id, msgs_Name)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.NameRegisted);
					return;
				}

				if (Database.DatabaseMethods.AllowUsersMethods.AllowUserExist(Command_Object.Guild.Id, msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdRegisted);
					return;
				}

				if (!Database.DatabaseMethods.RolesMethods.RoleFind(Command_Object.Guild.Id, msgs_RoleNum, out Roles DBRolesNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleNumberNotFound);
					return;
				}

				if (!await Authentication.Confirmation(Command_Object)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.AuthFailure);
					return;
				}

				AllowUsers InsertAllowUserData = Database.DatabaseMethods.AllowUsersMethods.AllowUserInsert(Command_Object.Guild.Id, msgs_ID, msgs_Name, msgs_RoleNum);

				DiscordRole GuildRole = Command_Object.Guild.GetRole(DBRolesNum.Uuid);
				string ResultText = string.Format(Command_Object.Language.DBUserAddSuccess, InsertAllowUserData.Name, InsertAllowUserData.Uuid, InsertAllowUserData.RoleNum, GuildRole.Name);
				await Command_Object.Channel.SendMessageAsync(ResultText);
			}
			catch (IndexOutOfRangeException) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.TypingMissed);
			}
		}
	}
}
