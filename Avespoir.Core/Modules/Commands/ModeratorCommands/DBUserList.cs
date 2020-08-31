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
			IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
			IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
			FilterDefinition<Roles> DBRolesGuildIDFilter = Builders<Roles>.Filter.Eq(Role => Role.GuildID, CommandObject.Guild.Id);

			FilterDefinition<AllowUsers> DBAllowUsersGuildIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.GuildID, CommandObject.Guild.Id);
			List<AllowUsers> DBAllowUsersList = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersGuildIDFilter).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);
			
			List<object> DBAllowUsersObjects = new List<object> { };
			foreach (AllowUsers DBAllowUser in DBAllowUsersList) {
				FilterDefinition<Roles> DBRolesFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUser.RoleNum);
				FilterDefinition<Roles> DBRolesGuildID_Filter = Builders<Roles>.Filter.And(DBRolesGuildIDFilter, DBRolesFilter);
				Roles DBRole = await (await DBRolesCollection.FindAsync(DBRolesGuildID_Filter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
				
				DiscordRole GuildRole = CommandObject.Guild.GetRole(DBRole.uuid);

				DBAllowUsersObjects.Add(new { RegisteredName = DBAllowUser.Name, UserID = DBAllowUser.uuid, Role = GuildRole.Name });
			}

			object[] DBAllowUsersArray = DBAllowUsersObjects.ToArray();
			string DBAllowUsersTableText = new TableFormatter().FormatObjects(DBAllowUsersArray);

			await CommandObject.Message.Channel.SendMessageAsync(string.Format(CommandObject.Language.DMMention, CommandObject.Message.Author.Mention));
			if (string.IsNullOrWhiteSpace(DBAllowUsersTableText)) await CommandObject.Member.SendMessageAsync(CommandObject.Language.ListNothing);
			else await CommandObject.Member.SendMessageAsync(DBAllowUsersTableText);
		}
	}
}
