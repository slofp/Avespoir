using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tababular;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-userlist")]
		public async Task DBUserList(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
			IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

			FilterDefinition<AllowUsers> DBAllowUsersFilter = Builders<AllowUsers>.Filter.Empty;
			List<AllowUsers> DBAllowUsersList = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersFilter).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);
			
			List<object> DBAllowUsersObjects = new List<object> { };
			foreach (AllowUsers DBAllowUser in DBAllowUsersList) {
				FilterDefinition<Roles> DBRolesFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUser.RoleNum);
				Roles DBRole = await (await DBRolesCollection.FindAsync(DBRolesFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
				
				DiscordRole GuildRole = CommandObject.Guild.GetRole(DBRole.uuid);

				DBAllowUsersObjects.Add(new { RegisteredName = DBAllowUser.Name, UserID = DBAllowUser.uuid, Role = GuildRole.Name });
			}

			object[] DBAllowUsersArray = DBAllowUsersObjects.ToArray();
			string DBAllowUsersTableText = new TableFormatter().FormatObjects(DBAllowUsersArray);

			await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Message.Author.Mention + "DMをご確認ください！");
			if (string.IsNullOrWhiteSpace(DBAllowUsersTableText)) await CommandObject.Member.SendMessageAsync("何も登録されていません");
			else await CommandObject.Member.SendMessageAsync(DBAllowUsersTableText);
		}
	}
}
