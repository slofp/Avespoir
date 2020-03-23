using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("find")]
		public async Task Find(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			try {
				ulong Userid = ulong.Parse(msgs[0]);

				DiscordMember FoundMember = await CommandObject.Guild.GetMemberAsync(Userid);
				string ResultString = string.Format("Username: {0}\nJoinAt: {1}\nGuildOwner: {2}\nAvaterURL: {3}", FoundMember.Username + "#" + FoundMember.Discriminator, FoundMember.JoinedAt, FoundMember.IsOwner ? "yes" : "no", FoundMember.AvatarUrl);
				await CommandObject.Channel.SendMessageAsync(ResultString);
			}
			catch (NotFoundException) {
				await CommandObject.Channel.SendMessageAsync("見つかりませんでした！");
			}
			catch (FormatException) {
				if (msgs[0] == null || msgs[0] == "") await CommandObject.Channel.SendMessageAsync("入力してください！");
				else await CommandObject.Channel.SendMessageAsync("数字で入力してください！");
			}
		}
	}
}
