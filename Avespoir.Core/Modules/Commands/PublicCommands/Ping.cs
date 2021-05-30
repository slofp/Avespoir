using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("ping", RoleLevel.Public)]
	class Ping : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Pingを測ります") {
			{ Database.Enums.Language.en_US, "Pinging" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}ping") {
			{ Database.Enums.Language.en_US, "{0}ping" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			DiscordMessage BotResponse = await CommandObject.Channel.SendMessageAsync(CommandObject.Language.PingWait);

			long MessageTick = CommandObject.Message.CreationTimestamp.Ticks;
			long ResponseTick = BotResponse.CreationTimestamp.Ticks;
			long PingTick = ResponseTick - MessageTick;

			TimeSpan PingSpan = new TimeSpan(PingTick);
			double Ping = PingSpan.TotalMilliseconds;

			await BotResponse.ModifyAsync($"{Ping}ms");
		}
	}
}
