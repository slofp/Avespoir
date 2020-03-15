using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Modules.Logger;
using AvespoirTest.Core.Modules.Utils;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("find")]
		public async Task Find(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			try {
				ulong Userid = ulong.Parse(msgs[0]);

				DiscordMember FoundMember = await CommandObject.Guild.GetMemberAsync(Userid);
				await CommandObject.Channel.SendMessageAsync($"{FoundMember.Username}#{FoundMember.Discriminator}");
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
