using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
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
			FilterDefinition<AllowUsers> DBAllowUsersGuildIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.GuildID, MemberObjects.Guild.Id);

			GetLanguage Get_Language;
			string GuildLanguageString = await DatabaseMethods.LanguageFind(MemberObjects.Guild.Id).ConfigureAwait(false);
			if (GuildLanguageString == null) Get_Language = new GetLanguage(Database.Enums.Language.ja_JP);
			else {
				if (!Enum.TryParse(GuildLanguageString, true, out Database.Enums.Language GuildLanguage))
					Get_Language = new GetLanguage(Database.Enums.Language.ja_JP);
				else Get_Language = new GetLanguage(GuildLanguage);
			}

			try {
				if (MemberObjects.Member.IsBot) {
					ulong Guild_ChannelID = await DatabaseMethods.LogChannelFind(MemberObjects.Guild.Id).ConfigureAwait(false);

					if (Guild_ChannelID != 0) {
						DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
						DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
							.WithTitle(Get_Language.Language_Data.IsBot)
							.WithDescription(
								string.Format(
									Get_Language.Language_Data.Bot_BanDescription,
									MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
									MemberObjects.Member.Id
								)
							)
							.WithColor(new DiscordColor(0x1971FF))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
							)
							.WithAuthor(Get_Language.Language_Data.BotRemoved);
						await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
					}
					else Log.Warning("Could not send from log channel");

					Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is Bot");
					return;
				}

				try {
					FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, MemberObjects.Member.Id);
					FilterDefinition<AllowUsers> DBAllowUsersGuildIDIDFilter = Builders<AllowUsers>.Filter.And(DBAllowUsersGuildIDFilter, DBAllowUsersIDFilter);
					AllowUsers DBAllowUserID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersGuildIDIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

					await DBAllowUsersCollection.DeleteOneAsync(DBAllowUsersGuildIDIDFilter).ConfigureAwait(false);

					ulong Guild_ChannelID = await DatabaseMethods.LogChannelFind(MemberObjects.Guild.Id).ConfigureAwait(false);

					if (!await DatabaseMethods.LeaveBanFind(MemberObjects.Guild.Id).ConfigureAwait(false)) {
						if (Guild_ChannelID != 0) {
							DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
							DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
								.WithDescription(
									string.Format(
										Get_Language.Language_Data.Bot_BanDescription,
										MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
										MemberObjects.Member.Id
									)
								)
								.WithColor(new DiscordColor(0xFF4B00))
								.WithTimestamp(DateTime.Now)
								.WithFooter(
									string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
								)
								.WithAuthor(Get_Language.Language_Data.Leaved);
							await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
						}
						else Log.Warning("Could not send from log channel");
					}
					else {
						await MemberObjects.Member.BanAsync(default, Get_Language.Language_Data.BanReason);

						if (Guild_ChannelID != 0) {
							DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
							DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
								.WithTitle(Get_Language.Language_Data.Baned)
								.WithDescription(
									string.Format(
										Get_Language.Language_Data.Bot_BanDescription,
										MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
										MemberObjects.Member.Id
									)
								)
								.WithColor(new DiscordColor(0xFF4B00))
								.WithTimestamp(DateTime.Now)
								.WithFooter(
									string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
								)
								.WithAuthor(Get_Language.Language_Data.Leaved);
							await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
						}
						else Log.Warning("Could not send from log channel");
					}

					Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} has leave the server");
					return;
				}
				catch (InvalidOperationException) {
					ulong Guild_ChannelID = await DatabaseMethods.LogChannelFind(MemberObjects.Guild.Id).ConfigureAwait(false);

					if (!await DatabaseMethods.LeaveBanFind(MemberObjects.Guild.Id).ConfigureAwait(false)) {
						if (Guild_ChannelID != 0) {
							DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
							DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
								.WithTitle(await DatabaseMethods.WhitelistFind(MemberObjects.Guild.Id).ConfigureAwait(false) ? Get_Language.Language_Data.DBDeleteLeave : Get_Language.Language_Data.DisableLeave)
								.WithDescription(
									string.Format(
										Get_Language.Language_Data.Bot_BanDescription,
										MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
										MemberObjects.Member.Id
									)
								)
								.WithColor(new DiscordColor(0xF6AA00))
								.WithTimestamp(DateTime.Now)
								.WithFooter(
									string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
								)
								.WithAuthor(Get_Language.Language_Data.Leaved);
							await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
						}
						else Log.Warning("Could not send from log channel");
					}
					else {
						await MemberObjects.Member.BanAsync(default, Get_Language.Language_Data.BanReason);

						if (Guild_ChannelID != 0) {
							DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
							DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
								.WithTitle(Get_Language.Language_Data.Baned)
								.WithDescription(
									string.Format(
										Get_Language.Language_Data.Bot_BanDescription,
										MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
										MemberObjects.Member.Id
									)
								)
								.WithColor(new DiscordColor(0xFF4B00))
								.WithTimestamp(DateTime.Now)
								.WithFooter(
									string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
								)
								.WithAuthor(Get_Language.Language_Data.Leaved);
							await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
						}
						else Log.Warning("Could not send from log channel");
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
