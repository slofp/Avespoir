using Avespoir.Core.Database.DatabaseMethods;
using Avespoir.Core.Database.Schemas;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class MessageReactionAddedEvent {

		internal static async Task Main(DiscordClient _, MessageReactionAddEventArgs Args) {
			if (PendingUsersMethods.PendingUserFind(Args.Message.Id, out PendingUsers PendingUser)) {
				if (DateTime.Now > PendingUser.PendingStart.AddDays(7)) return;
				else {
					if (Args.Emoji == DiscordEmoji.FromUnicode("❌")) {
						PendingUsersMethods.PendingUserDelete(PendingUser);

						DiscordEmbedBuilder SubmitEmbed = new DiscordEmbedBuilder();

						SubmitEmbed
							.WithTitle("ユーザー登録申請が却下されました")
							.WithDescription(string.Format("{0}によって申請が却下されました", Args.User.Mention))
							.WithColor(new DiscordColor(0xFF4B00))
							.WithTimestamp(DateTime.Now)
							.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));
						await Args.Message.ModifyAsync(embed: SubmitEmbed.Build());
					}
				}
			}
		}
	}
}
