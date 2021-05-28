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

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
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
