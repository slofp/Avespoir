using AvespoirTest.Core.Database;
using AvespoirTest.Core.Database.Enums;
using AvespoirTest.Core.Database.Schemas;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Events {

	class GuildMemberAddEvent {

		internal static async Task Main(GuildMemberAddEventArgs MemberObjects) {
			new DebugLog("GuildMemberAddEvent " + "Start...");

			IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
			IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
			IMongoCollection<LogChannels> DBLogChannelsCollection = MongoDBClient.Database.GetCollection<LogChannels>(typeof(LogChannels).Name);
			
			try {
				if (MemberObjects.Member.IsBot) {
					try {
						FilterDefinition<Roles> DBRolesRoleLevelFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleLevel, Enum.GetName(typeof(RoleLevel), RoleLevel.Bot));
						Roles DBRoleRoleLevel = await (await DBRolesCollection.FindAsync(DBRolesRoleLevelFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBRoleRoleLevel is null...?

						DiscordRole GuildRole = MemberObjects.Guild.GetRole(DBRoleRoleLevel.uuid);
						await MemberObjects.Member.GrantRoleAsync(GuildRole);
					}
					catch (InvalidOperationException) {
						new WarningLog("Could not grant role");
					}

					try {
						FilterDefinition<LogChannels> DBLogChannelsGuildIDFilter = Builders<LogChannels>.Filter.Eq(LogChannel => LogChannel.GuildID, MemberObjects.Guild.Id);
						LogChannels DBLogChannelsGuildID = await (await DBLogChannelsCollection.FindAsync(DBLogChannelsGuildIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBLogChannelsGuildID is null, Not Send Log Channel

						DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(DBLogChannelsGuildID.ChannelID);
						DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
							.WithTitle("Botです")
							.WithDescription(
								string.Format(
									"名前: {0}\nID: {1}",
									MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
									MemberObjects.Member.Id
								)
							)
							.WithColor(new DiscordColor(0x1971FF))
							.WithTimestamp(new DateTime())
							.WithFooter(
								string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
							)
							.WithAuthor("アクセス完了");
						await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
					}
					catch (InvalidOperationException) {
						new WarningLog("Could not send from log channel");
					}

					new DebugLog($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is Bot");
					return;
				}

				FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, MemberObjects.Member.Id);
				AllowUsers DBAllowUserID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
				// if DBAllowUserID is null, processes will not be executed from here. And kick.

				try {
					FilterDefinition<Roles> DBRolesRoleNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUserID.RoleNum);
					Roles DBRoleRoleNum = await (await DBRolesCollection.FindAsync(DBRolesRoleNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBRoleRoleNum is null...?

					DiscordRole GuildRole = MemberObjects.Guild.GetRole(DBRoleRoleNum.uuid);
					try {
						FilterDefinition<LogChannels> DBLogChannelsGuildIDFilter = Builders<LogChannels>.Filter.Eq(LogChannel => LogChannel.GuildID, MemberObjects.Guild.Id);
						LogChannels DBLogChannelsGuildID = await (await DBLogChannelsCollection.FindAsync(DBLogChannelsGuildIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBLogChannelsGuildID is null, Not Send Log Channel

						DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(DBLogChannelsGuildID.ChannelID);
						DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
							.WithTitle("参加を許可します")
							.WithDescription(
								string.Format(
									"名前: {0}\nID: {1}\nRole: {2}",
									MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
									MemberObjects.Member.Id,
									GuildRole.Name
								)
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithTimestamp(new DateTime())
							.WithFooter(
								string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
							)
							.WithAuthor("アクセス権の確認");
						await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
					}
					catch (InvalidOperationException) {
						new WarningLog("Could not send from log channel");
					}

					RoleLevel DBRoleLevel = Enum.Parse<RoleLevel>(DBRoleRoleNum.RoleLevel);
					if (DBRoleLevel == RoleLevel.Public) {
						await MemberObjects.Member.GrantRoleAsync(GuildRole);
						DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
							.WithTitle(string.Format("Welcome to {0}! {1}では次のことが許可されています！", MemberObjects.Guild.Name, GuildRole.Name))
							.AddField(
								"サーバーの操作ができます",
								"ほぼ自由にサーバーを操作できます"
							)
							.AddField(
								"すべてのチャンネルを読んだり送信することができます",
								"これはあなたが認定された証です"
							)
							.AddField(
								"抜けたらBANされます",
								string.Format("現在{0}は抜けたらBANされます", GuildRole.Name)
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
							);
						await MemberObjects.Member.SendMessageAsync(default, default, WelcomeEmbed);
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						await MemberObjects.Member.GrantRoleAsync(GuildRole);
						DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
							.WithTitle(string.Format("Welcome to {0}! {1}では次のことが許可されています！", MemberObjects.Guild.Name, GuildRole.Name))
							.AddField(
								"サーバーの操作ができます",
								"今日からあなたはここの管理者です"
							)
							.AddField(
								"データベースへの追加、変更、削除等が行えます",
								string.Format("{0}Botのすべてのコマンドを使うことができます", MemberObjects.Client.CurrentUser.Username)
							)
							.AddField(
								"すべてのチャンネルを読んだり送信することができます",
								"これはあなたが認定された証です"
							)
							.AddField(
								"抜けたらBANされます",
								string.Format("現在{0}は抜けたらBANされます", GuildRole.Name)
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
							);
						await MemberObjects.Member.SendMessageAsync(default, default, WelcomeEmbed);
					}

					new DebugLog($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is allowed join");
					return;
				}
				catch (InvalidOperationException) {
					await MemberObjects.Member.SendMessageAsync("申し訳ありません... サーバー管理者にお問い合わせください");
				}
			}
			catch (InvalidOperationException) {
				await MemberObjects.Member.SendMessageAsync("あなたはアクセス権がありません、もしアクセス権があり入れない場合はサーバー管理者にお問い合わせください");
				await MemberObjects.Member.BanAsync(default, "アクセス権がありません");

				try {
					FilterDefinition<LogChannels> DBLogChannelsGuildIDFilter = Builders<LogChannels>.Filter.Eq(LogChannel => LogChannel.GuildID, MemberObjects.Guild.Id);
					LogChannels DBLogChannelsGuildID = await (await DBLogChannelsCollection.FindAsync(DBLogChannelsGuildIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBLogChannelsGuildID is null, Not Send Log Channel

					DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(DBLogChannelsGuildID.ChannelID);
					DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
						.WithTitle("BANされました")
						.WithDescription(
							string.Format(
								"名前: {0}\nID: {1}",
								MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
								MemberObjects.Member.Id
							)
						)
						.WithColor(new DiscordColor(0xFF4B00))
						.WithTimestamp(new DateTime())
						.WithFooter(
							string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
						)
						.WithAuthor("アクセス権がありません");
					await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
				}
				catch (InvalidOperationException) {
					new WarningLog("Could not send from log channel");
				}

				new DebugLog($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is not allowed join");
				return;
			}
			finally {
				new DebugLog("GuildMemberAddEvent " + "End...");
			}
		}
	}
}
