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

	[Command("db-userdel", RoleLevel.Moderator)]
	class DBUserDelete : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Userデータベースからユーザーを削除します") {
			{ Database.Enums.Language.en_US, "Remove a user from the User database" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}db-userdel [ユーザーID]") {
			{ Database.Enums.Language.en_US, "{0}db-userdel [UserID]" }
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

				if (Database.DatabaseMethods.AllowUsersMethods.AllowUserFind(Command_Object.Guild.Id, msgs_ID, out AllowUsers DBAllowUsersID)) {
					if (!await Authentication.Confirmation(Command_Object)) {
						await Command_Object.Channel.SendMessageAsync(Command_Object.Language.AuthFailure);
						return;
					}

					Database.DatabaseMethods.AllowUsersMethods.AllowUserDelete(DBAllowUsersID);
					try {
						DiscordMember DeleteGuildMember = await Command_Object.Guild.GetMemberAsync(DBAllowUsersID.Uuid).ConfigureAwait(false);
						string KickReason = string.Format(Command_Object.Language.KickReason, Command_Object.Member.Username + "#" + Command_Object.Member.Discriminator);
						await DeleteGuildMember.RemoveAsync(KickReason).ConfigureAwait(false);
					}
					finally {
						string ResultText = string.Format(Command_Object.Language.DBUserDeleteSuccess, DBAllowUsersID.Name, DBAllowUsersID.Uuid);
						await Command_Object.Channel.SendMessageAsync(ResultText);
					}
				}
				else {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdNotRegisted);
					return;
				}
			}
			catch (IndexOutOfRangeException) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.TypingMissed);
			}
		}
	}
}
