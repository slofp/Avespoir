using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("$find")]
		public async Task Find(CommandContext Context) {
			string[] msgs = Context.Message.Content.Substring(CommandConfig.MainPrefix.Length + Context.Command.Name.Length).Trim().Split(" ");
			try {
				ulong Userid = ulong.Parse(msgs[0]);

				DiscordMember FoundMember = await Context.Guild.GetMemberAsync(Userid);
				await Context.Channel.SendMessageAsync($"{FoundMember.Username}#{FoundMember.Discriminator}");
			}
			catch (NotFoundException) {
				await Context.Channel.SendMessageAsync("見つかりませんでした！");
			}
			catch (FormatException) {
				if (msgs[0] == null || msgs[0] == "") await Context.Channel.SendMessageAsync("入力してください！");
				else await Context.Channel.SendMessageAsync("数字で入力してください！");
			}
			catch (Exception Error) {
				new ErrorLog(Error.Message);
			}
		}
	}
}
