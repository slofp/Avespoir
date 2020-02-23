using AvespoirTest.Core.Configs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System;
using DSharpPlus.Entities;

namespace AvespoirTest.Core.Modules.Commands {

	class PublicCommands {

		static string PublicPrefix = CommandConfig.PublicPrefixTag; 

		[Command("test")]
		public async Task Test(CommandContext context) {
			await context.Channel.SendMessageAsync("gomi");
		}

		// コマンドを送信した時間からWaitを送信した時間をPingとして使用
		[Command("ping")]
		public async Task Ping(CommandContext Context) {
			DiscordMessage BotResponse = await Context.Channel.SendMessageAsync("Wait...");
			
			long MessageTick = Context.Message.CreationTimestamp.Ticks;
			long ResponseTick = BotResponse.CreationTimestamp.Ticks;
			long PingTick = ResponseTick - MessageTick;

			TimeSpan PingSpan = new TimeSpan(PingTick);
			double Ping = PingSpan.TotalMilliseconds;

			await BotResponse.ModifyAsync($"{Ping}ms");
		}

		[Command("emoji")]
		public async Task Emoji(CommandContext Context) {
			
		}
	}
}
