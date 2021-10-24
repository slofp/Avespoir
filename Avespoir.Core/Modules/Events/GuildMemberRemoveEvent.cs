using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using Avespoir.Core.Database.DatabaseMethods;

namespace Avespoir.Core.Modules.Events {

	class GuildMemberRemoveEvent {

		private static async Task BotProcess(GuildMemberRemoveEventArgs MemberObjects, GetLanguage Get_Language) {
			ulong Guild_ChannelID = GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

			if (Guild_ChannelID != 0) {
				DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
				DiscordEmbed​Builder LogChannelEmbed = new DiscordEmbed​Builder()
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
						string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
					)
					.WithAuthor(Get_Language.Language_Data.BotRemoved);
				await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
			}
			else Log.Warning("Could not send from log channel");

			Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is Bot");
			return;
		}

		private static async Task UserProcess(GuildMemberRemoveEventArgs MemberObjects, GetLanguage Get_Language) {
			bool SelfLeave = false;

			if (AllowUsersMethods.AllowUserFind(MemberObjects.Guild.Id, MemberObjects.Member.Id, out AllowUsers DBAllowUserID)) {
				AllowUsersMethods.AllowUserDelete(DBAllowUserID);
				SelfLeave = true;
			}

			ulong Guild_ChannelID = GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

			if (!GuildConfigMethods.LeaveBanFind(MemberObjects.Guild.Id)) {
				if (Guild_ChannelID != 0) {
					DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
					DiscordEmbed​Builder LogChannelEmbed = new DiscordEmbed​Builder()
						.WithTitle(
							SelfLeave ? string.Empty :
							GuildConfigMethods.WhitelistFind(MemberObjects.Guild.Id) ? Get_Language.Language_Data.DBDeleteLeave :
							Get_Language.Language_Data.DisableLeave
						)
						.WithDescription(
							string.Format(
								Get_Language.Language_Data.Bot_BanDescription,
								MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
								MemberObjects.Member.Id
							)
						)
						.WithColor(new DiscordColor(SelfLeave ? 0xFF4B00 : 0xF6AA00))
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
				await MemberObjects.Member.BanAsync(default, Get_Language.Language_Data.BanReason);

				if (Guild_ChannelID != 0) {
					DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
					DiscordEmbed​Builder LogChannelEmbed = new DiscordEmbed​Builder()
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
							string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
						)
						.WithAuthor(Get_Language.Language_Data.Leaved);
					await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
				}
				else Log.Warning("Could not send from log channel");
			}

			Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} has leave the server");
			return;
		}

		internal static async Task Main(DiscordClient Bot, GuildMemberRemoveEventArgs MemberObjects) {
			Log.Debug("GuildMemberRemoveEvent " + "Start...");

			GetLanguage Get_Language = new GetLanguage(GuildConfigMethods.LanguageFind(MemberObjects.Guild.Id));

			if (MemberObjects.Member.IsBot) await BotProcess(MemberObjects, Get_Language).ConfigureAwait(false);
			else await UserProcess(MemberObjects, Get_Language).ConfigureAwait(false);

			Log.Debug("GuildMemberRemoveEvent " + "End...");
		}
	}
}
