using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Discord;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tababular;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("db-rolelist", RoleLevel.Moderator)]
	class DBRoleList : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Roleデータベースに登録されているロール情報をリストにして表示します") {
			{ Database.Enums.Language.en_US, "Show a list of role information registered in the Role database" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}db-rolelist") {
			{ Database.Enums.Language.en_US, "{0}db-rolelist" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			Database.DatabaseMethods.RolesMethods.RolesListFind(Command_Object.Guild.Id, out List<Roles> DBRolesList);

			List<object> DBRolesObjects = new List<object> { };
			foreach (Roles DBRole in DBRolesList) {
				DBRolesObjects.Add(new { RegisteredID = DBRole.Uuid, RegisteredNumber = DBRole.RoleNum, DBRole.RoleLevel });
			}

			object[] DBRolesArray = DBRolesObjects.ToArray();
			string DBRolesTableText = new TableFormatter().FormatObjects(DBRolesArray);

			await Command_Object.Channel.SendMessageAsync(string.Format(Command_Object.Language.DMMention, Command_Object.Author.Mention));
			if (string.IsNullOrWhiteSpace(DBRolesTableText)) await Command_Object.Member.SendMessageAsync(Command_Object.Language.ListNothing);
			else await Command_Object.Member.SendMessageAsync(DBRolesTableText);
		}
	}
}
