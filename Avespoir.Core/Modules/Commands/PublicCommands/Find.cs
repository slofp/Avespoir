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
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
				return;
			}

			if (string.IsNullOrWhiteSpace(msgs[0])) {
				await CommandObject.Message.Channel.SendMessageAsync("IDが空白またはNullです");
				return;
			}
			if (!ulong.TryParse(msgs[0], out ulong Userid)) {
				await CommandObject.Message.Channel.SendMessageAsync("IDは数字でなければいけません");
				return;
			}

			try {
				DiscordMember FoundMember = await CommandObject.Guild.GetMemberAsync(Userid);
				string ResultString = string.Format("Username: {0}\nJoinAt: {1}\nGuildOwner: {2}\nAvaterURL: {3}", FoundMember.Username + "#" + FoundMember.Discriminator, FoundMember.JoinedAt, FoundMember.IsOwner ? "yes" : "no", FoundMember.AvatarUrl);
				await CommandObject.Channel.SendMessageAsync(ResultString);
			}
			catch (NotFoundException) {
				await CommandObject.Channel.SendMessageAsync("見つかりませんでした！");
			}
		}
	}
}
