using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using DSharpPlus;
using DSharpPlus.EventArgs;
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

		internal override async Task Execute(CommandObject Command_Object) {
			DiscordMessage BotResponse = await Command_Object.Channel.SendMessageAsync(Command_Object.Language.PingWait);

			long MessageTick = Command_Object.Timestamp.Ticks;
			long ResponseTick = BotResponse.Timestamp.Ticks;
			long PingTick = ResponseTick - MessageTick;

			Logger.Log.Debug(Command_Object.Timestamp.Millisecond);
			Logger.Log.Debug(BotResponse.Timestamp.Millisecond);

			TimeSpan PingSpan = new TimeSpan(PingTick);
			double Ping = PingSpan.TotalMilliseconds;

			await BotResponse.ModifyAsync(MessagePropertie => MessagePropertie.Content = $"{Ping}ms");
		}
	}
}
