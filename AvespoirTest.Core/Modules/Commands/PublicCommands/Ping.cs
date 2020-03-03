using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		// コマンドを送信した時間からWaitを送信した時間をPingとして使用
		[Command("$ping")]
		public async Task Ping(CommandContext Context) {
			DiscordMessage BotResponse = await Context.Channel.SendMessageAsync("Wait...");

			long MessageTick = Context.Message.CreationTimestamp.Ticks;
			long ResponseTick = BotResponse.CreationTimestamp.Ticks;
			long PingTick = ResponseTick - MessageTick;

			TimeSpan PingSpan = new TimeSpan(PingTick);
			double Ping = PingSpan.TotalMilliseconds;

			await BotResponse.ModifyAsync($"{Ping}ms");
		}
	}
}