using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-roleadd")]
		public async Task DBRoleAdd(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
					return;
				}

				RoleLevel intRoleLevel;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[0], out ulong msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyRoleNumber);
					return;
				}
				if (!uint.TryParse(msgs[1], out uint msgs_RoleNum)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberNotNumber);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[2])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyRoleLevel);
					return;
				}
				// Next Update
				MatchCollection DenyRoleLevels = Regex.Matches(msgs[2], @"\D+");
				if (DenyRoleLevels.Count != 0) {
					List<string> DenyStrings = new List<string>();
					for (int i = 0; i < DenyRoleLevels.Count; i++) {
						Match DenyRoleLevel = DenyRoleLevels[i];
						DenyStrings.Add(DenyRoleLevel.Value);
					}

					await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.RoleLevelDenyText, "`" + string.Join(@"` `", DenyStrings) + "`"));
					return;
				}
				if (!int.TryParse(msgs[2], out int msgs_RoleLevel)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleLevelNotNumber);
					return;
				}
				
				intRoleLevel = (RoleLevel) Enum.ToObject(typeof(RoleLevel), msgs_RoleLevel);
				if (string.IsNullOrWhiteSpace(Enum.GetName(typeof(RoleLevel), intRoleLevel))) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleLevelNotFound);
					return;
				}

				if (Database.DatabaseMethods.RolesMethods.RoleExist(CommandObject.Guild.Id, msgs_RoleNum)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberRegisted);
					return;
				}

				if (Database.DatabaseMethods.RolesMethods.RoleExist(CommandObject.Guild.Id, msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdRegisted);
					return;
				}

				if (!await Authentication.Confirmation(CommandObject)) {
					await CommandObject.Channel.SendMessageAsync(CommandObject.Language.AuthFailure);
					return;
				}

				Roles InsertRoleData = Database.DatabaseMethods.RolesMethods.RoleInsert(CommandObject.Guild.Id, msgs_ID, msgs_RoleNum, Enum.GetName(typeof(RoleLevel), intRoleLevel));

				DiscordRole GuildRole = CommandObject.Guild.GetRole(InsertRoleData.Uuid);
				string ResultText = string.Format(CommandObject.Language.DBRoleAddSuccess, InsertRoleData.Uuid, GuildRole.Name, InsertRoleData.RoleNum, InsertRoleData.RoleLevel);
				await CommandObject.Message.Channel.SendMessageAsync(ResultText);
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.TypingMissed);
			}
		}
	}
}
