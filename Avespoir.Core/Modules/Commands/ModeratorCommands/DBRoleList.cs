using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
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

		internal override async Task Execute(CommandObjects CommandObject) {
			Database.DatabaseMethods.RolesMethods.RolesListFind(CommandObject.Guild.Id, out List<Roles> DBRolesList);

			List<object> DBRolesObjects = new List<object> { };
			foreach (Roles DBRole in DBRolesList) {
				DBRolesObjects.Add(new { RegisteredID = DBRole.Uuid, RegisteredNumber = DBRole.RoleNum, DBRole.RoleLevel });
			}

			object[] DBRolesArray = DBRolesObjects.ToArray();
			string DBRolesTableText = new TableFormatter().FormatObjects(DBRolesArray);

			await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.DMMention, CommandObject.Message.Author.Mention));
			if (string.IsNullOrWhiteSpace(DBRolesTableText)) await CommandObject.Member.SendMessageAsync(CommandObject.Language.ListNothing);
			else await CommandObject.Member.SendMessageAsync(DBRolesTableText);
		}
	}
}
