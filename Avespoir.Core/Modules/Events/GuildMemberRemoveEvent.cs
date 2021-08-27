using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class GuildMemberRemoveEvent {

		private static async Task BotProcess(SocketGuildUser MemberObjects, GetLanguage Get_Language) {
			ulong Guild_ChannelID = Database.DatabaseMethods.GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

			if (Guild_ChannelID != 0) {
				SocketTextChannel GuildLogChannel = MemberObjects.Guild.GetTextChannel(Guild_ChannelID);
				Embed​Builder LogChannelEmbed = new Embed​Builder()
					.WithTitle(Get_Language.Language_Data.IsBot)
					.WithDescription(
						string.Format(
							Get_Language.Language_Data.Bot_BanDescription,
							MemberObjects.Username + "#" + MemberObjects.Discriminator,
							MemberObjects.Id
						)
					)
					.WithColor(new Color(0x1971FF))
					.WithTimestamp(DateTime.Now)
					.WithFooter(
						string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
					)
					.WithAuthor(Get_Language.Language_Data.BotRemoved);
				await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
			}
			else Log.Warning("Could not send from log channel");

			Log.Debug($"{MemberObjects.Username + "#" + MemberObjects.Discriminator} is Bot");
			return;
		}

		private static async Task UserProcess(SocketGuildUser MemberObjects, GetLanguage Get_Language) {
			bool SelfLeave = false;

			if (Database.DatabaseMethods.AllowUsersMethods.AllowUserFind(MemberObjects.Guild.Id, MemberObjects.Id, out AllowUsers DBAllowUserID)) {
				Database.DatabaseMethods.AllowUsersMethods.AllowUserDelete(DBAllowUserID);
				SelfLeave = true;
			}

			ulong Guild_ChannelID = Database.DatabaseMethods.GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

			if (!Database.DatabaseMethods.GuildConfigMethods.LeaveBanFind(MemberObjects.Guild.Id)) {
				if (Guild_ChannelID != 0) {
					SocketTextChannel GuildLogChannel = MemberObjects.Guild.GetTextChannel(Guild_ChannelID);
					Embed​Builder LogChannelEmbed = new Embed​Builder()
						.WithTitle(
							SelfLeave ? string.Empty :
							Database.DatabaseMethods.GuildConfigMethods.WhitelistFind(MemberObjects.Guild.Id) ? Get_Language.Language_Data.DBDeleteLeave :
							Get_Language.Language_Data.DisableLeave
						)
						.WithDescription(
							string.Format(
								Get_Language.Language_Data.Bot_BanDescription,
								MemberObjects.Username + "#" + MemberObjects.Discriminator,
								MemberObjects.Id
							)
						)
						.WithColor(new Color(SelfLeave ? 0xFF4B00u : 0xF6AA00u))
						.WithTimestamp(DateTime.Now)
						.WithFooter(
							string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
						)
						.WithAuthor(Get_Language.Language_Data.Leaved);
					await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
				}
				else Log.Warning("Could not send from log channel");
			}
			else {
				await MemberObjects.BanAsync(default, Get_Language.Language_Data.BanReason);

				if (Guild_ChannelID != 0) {
					SocketTextChannel GuildLogChannel = MemberObjects.Guild.GetTextChannel(Guild_ChannelID);
					Embed​Builder LogChannelEmbed = new Embed​Builder()
						.WithTitle(Get_Language.Language_Data.Baned)
						.WithDescription(
							string.Format(
								Get_Language.Language_Data.Bot_BanDescription,
								MemberObjects.Username + "#" + MemberObjects.Discriminator,
								MemberObjects.Id
							)
						)
						.WithColor(new Color(0xFF4B00))
						.WithTimestamp(DateTime.Now)
						.WithFooter(
							string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
						)
						.WithAuthor(Get_Language.Language_Data.Leaved);
					await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
				}
				else Log.Warning("Could not send from log channel");
			}

			Log.Debug($"{MemberObjects.Username + "#" + MemberObjects.Discriminator} has leave the server");
			return;
		}

		internal static async Task Main(SocketGuildUser MemberObjects) {
			Log.Debug("GuildMemberRemoveEvent " + "Start...");

			GetLanguage Get_Language;
			string GuildLanguageString = Database.DatabaseMethods.GuildConfigMethods.LanguageFind(MemberObjects.Guild.Id);
			if (GuildLanguageString == null) Get_Language = new GetLanguage(Database.Enums.Language.ja_JP);
			else {
				if (!Enum.TryParse(GuildLanguageString, true, out Database.Enums.Language GuildLanguage))
					Get_Language = new GetLanguage(Database.Enums.Language.ja_JP);
				else Get_Language = new GetLanguage(GuildLanguage);
			}

			if (MemberObjects.IsBot) await BotProcess(MemberObjects, Get_Language).ConfigureAwait(false);
			else await UserProcess(MemberObjects, Get_Language).ConfigureAwait(false);

			Log.Debug("GuildMemberRemoveEvent " + "End...");
		}
	}
}
