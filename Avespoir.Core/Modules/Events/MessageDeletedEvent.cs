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

	class MessageDeletedEvent {

		internal static async Task Main(DiscordClient _, MessageDeleteEventArgs Args) {
			if (PendingUsersMethods.PendingUserFind(Args.Message.Id, out PendingUsers PendingUser)) {
				if (DateTime.Now > PendingUser.PendingStart.AddDays(7)) return;
				else {
					PendingUsersMethods.PendingUserDelete(PendingUser);

					DiscordEmbedBuilder SubmitEmbed = new DiscordEmbedBuilder();

					SubmitEmbed
						.WithTitle("ユーザー登録申請が却下されました")
						.WithDescription("申請メッセージが削除されたため申請が却下されました")
						.WithColor(new DiscordColor(0xFF4B00))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));
					await Args.Channel.SendMessageAsync(embed: SubmitEmbed.Build());
				}
			}
		}
	}
}
