using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-userdel")]
		public async Task DBUserDelete(CommandObjects CommandObject) {
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

				if (Database.DatabaseMethods.AllowUsersMethods.AllowUserFind(CommandObject.Guild.Id, msgs_ID, out AllowUsers DBAllowUsersID)) {
					if (!await Authentication.Confirmation(CommandObject)) {
						await CommandObject.Channel.SendMessageAsync(CommandObject.Language.AuthFailure);
						return;
					}

					Database.DatabaseMethods.AllowUsersMethods.AllowUserDelete(DBAllowUsersID);
					try {
						DiscordMember DeleteGuildMember = await CommandObject.Guild.GetMemberAsync(DBAllowUsersID.Uuid);
						string KickReason = string.Format(CommandObject.Language.KickReason, CommandObject.Member.Username + "#" + CommandObject.Member.Discriminator);
						await DeleteGuildMember.RemoveAsync(KickReason);
					}
					finally {
						string ResultText = string.Format(CommandObject.Language.DBUserDeleteSuccess, DBAllowUsersID.Name, DBAllowUsersID.Uuid);
						await CommandObject.Message.Channel.SendMessageAsync(ResultText);
					}
				}
				else {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdNotRegisted);
					return;
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.TypingMissed);
			}
		}
	}
}
