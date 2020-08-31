using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Utils;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tababular;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-rolelist")]
		public async Task DBRoleList(CommandObjects CommandObject) {
			IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

			FilterDefinition<Roles> DBGuildIDFilter = Builders<Roles>.Filter.Eq(Role => Role.GuildID, CommandObject.Guild.Id);
			List<Roles> DBRolesList = await (await DBRolesCollection.FindAsync(DBGuildIDFilter).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

			List<object> DBRolesObjects = new List<object> { };
			foreach (Roles DBRole in DBRolesList) {
				DBRolesObjects.Add(new { RegisteredID = DBRole.uuid, RegisteredNumber = DBRole.RoleNum, RoleLevel = DBRole.RoleLevel });
			}

			object[] DBRolesArray = DBRolesObjects.ToArray();
			string DBRolesTableText = new TableFormatter().FormatObjects(DBRolesArray);

			await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.DMMention, CommandObject.Message.Author.Mention));
			if (string.IsNullOrWhiteSpace(DBRolesTableText)) await CommandObject.Member.SendMessageAsync(CommandObject.Language.ListNothing);
			else await CommandObject.Member.SendMessageAsync(DBRolesTableText);
		}
	}
}
