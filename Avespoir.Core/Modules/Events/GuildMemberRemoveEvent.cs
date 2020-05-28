using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class GuildMemberRemoveEvent {

		internal static async Task Main(GuildMemberRemoveEventArgs MemberObjects) {
			Log.Debug("GuildMemberRemoveEvent " + "Start...");

			IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
			IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
			IMongoCollection<LogChannels> DBLogChannelsCollection = MongoDBClient.Database.GetCollection<LogChannels>(typeof(LogChannels).Name);

			try {
				if (MemberObjects.Member.IsBot) {
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
							.WithAuthor("サーバーから削除しました");
						await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
					}
					catch (InvalidOperationException) {
						Log.Warning("Could not send from log channel");
					}

					Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is Bot");
					return;
				}

				try {
					FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, MemberObjects.Member.Id);
					AllowUsers DBAllowUserID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

					await DBAllowUsersCollection.DeleteOneAsync(DBAllowUsersIDFilter).ConfigureAwait(false);
					await MemberObjects.Member.BanAsync(default, "サーバーを抜けたため");

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
							.WithAuthor("サーバーを抜けました");
						await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
					}
					catch (InvalidOperationException) {
						Log.Warning("Could not send from log channel");
					}

					Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} has leave the server");
					return;
				}
				catch (InvalidOperationException) {
					try {
						FilterDefinition<LogChannels> DBLogChannelsGuildIDFilter = Builders<LogChannels>.Filter.Eq(LogChannel => LogChannel.GuildID, MemberObjects.Guild.Id);
						LogChannels DBLogChannelsGuildID = await (await DBLogChannelsCollection.FindAsync(DBLogChannelsGuildIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBLogChannelsGuildID is null, Not Send Log Channel

						DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(DBLogChannelsGuildID.ChannelID);
						DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
							.WithTitle("サーバーに登録されていないかデータベース上から削除されました")
							.WithDescription(
								string.Format(
									"名前: {0}\nID: {1}",
									MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
									MemberObjects.Member.Id
								)
							)
							.WithColor(new DiscordColor(0xF6AA00))
							.WithTimestamp(new DateTime())
							.WithFooter(
								string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
							)
							.WithAuthor("サーバーを抜けました");
						await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
					}
					catch (InvalidOperationException) {
						Log.Warning("Could not send from log channel");
					}

					Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} has leave the server");
					return;
				}
			}
			finally {
				Log.Debug("GuildMemberRemoveEvent " + "End...");
			}
		}
	}
}
