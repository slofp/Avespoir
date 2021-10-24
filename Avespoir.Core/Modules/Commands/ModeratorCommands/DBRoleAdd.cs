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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("db-roleadd", RoleLevel.Moderator)]
	class DBRoleAdd : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Roleデータベースに役職を追加します") {
			{ Database.Enums.Language.en_US, "Add a role to the Role database" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}db-roleadd [役職ID] [役職登録番号] [役職レベル(一般: 0, モデレーター: 1, Bot: 2)]") {
			{ Database.Enums.Language.en_US, "{0}db-roleadd [RoleID] [Role number] [Role level(Public: 0, Moderator: 1, Bot: 2)]" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			try {
				string[] msgs = Command_Object.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyText);
					return;
				}

				RoleLevel intRoleLevel;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[0], out ulong msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdCouldntParse);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyRoleNumber);
					return;
				}
				if (!uint.TryParse(msgs[1], out uint msgs_RoleNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleNumberNotNumber);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[2])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyRoleLevel);
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

					await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.RoleLevelDenyText, "`" + string.Join(@"` `", DenyStrings) + "`"));
					return;
				}
				if (!int.TryParse(msgs[2], out int msgs_RoleLevel)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleLevelNotNumber);
					return;
				}

				intRoleLevel = (RoleLevel) Enum.ToObject(typeof(RoleLevel), msgs_RoleLevel);
				if (string.IsNullOrWhiteSpace(Enum.GetName(typeof(RoleLevel), intRoleLevel))) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleLevelNotFound);
					return;
				}

				if (Database.DatabaseMethods.RolesMethods.RoleExist(Command_Object.Guild.Id, msgs_RoleNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleNumberRegisted);
					return;
				}

				if (Database.DatabaseMethods.RolesMethods.RoleExist(Command_Object.Guild.Id, msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdRegisted);
					return;
				}

				if (!await Authentication.Confirmation(Command_Object)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.AuthFailure);
					return;
				}

				Roles InsertRoleData = Database.DatabaseMethods.RolesMethods.RoleInsert(Command_Object.Guild.Id, msgs_ID, msgs_RoleNum, intRoleLevel);

				DiscordRole GuildRole = Command_Object.Guild.GetRole(InsertRoleData.Uuid);
				string ResultText = string.Format(Command_Object.Language.DBRoleAddSuccess, InsertRoleData.Uuid, GuildRole.Name, InsertRoleData.RoleNum, InsertRoleData.RoleLevel);
				await Command_Object.Channel.SendMessageAsync(ResultText);
			}
			catch (IndexOutOfRangeException) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.TypingMissed);
			}
		}
	}
}
