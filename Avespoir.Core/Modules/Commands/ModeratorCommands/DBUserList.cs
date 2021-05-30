using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tababular;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("db-userlist", RoleLevel.Moderator)]
	class DBUserList : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Userデータベースに登録されているユーザー情報をリストにして表示します") {
			{ Database.Enums.Language.en_US, "Show a list of user information registered in the User database" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}db-userlist") {
			{ Database.Enums.Language.en_US, "{0}db-userlist" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			Database.DatabaseMethods.AllowUsersMethods.AllowUsersListFind(CommandObject.Guild.Id, out List<AllowUsers> DBAllowUsersList);

			List<object> DBAllowUsersObjects = new List<object> { };
			foreach (AllowUsers DBAllowUser in DBAllowUsersList) {
				Database.DatabaseMethods.RolesMethods.RoleFind(CommandObject.Guild.Id, DBAllowUser.RoleNum, out Roles DBRole);

				DiscordRole GuildRole = CommandObject.Guild.GetRole(DBRole.Uuid);

				DBAllowUsersObjects.Add(new { RegisteredName = DBAllowUser.Name, UserID = DBAllowUser.Uuid, Role = GuildRole.Name });
			}

			object[] DBAllowUsersArray = DBAllowUsersObjects.ToArray();
			string DBAllowUsersTableText = new TableFormatter().FormatObjects(DBAllowUsersArray);

			await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.DMMention, CommandObject.Message.Author.Mention));
			if (string.IsNullOrWhiteSpace(DBAllowUsersTableText)) await CommandObject.Member.SendMessageAsync(CommandObject.Language.ListNothing);
			else await CommandObject.Member.SendMessageAsync(DBAllowUsersTableText);
		}
	}
}
