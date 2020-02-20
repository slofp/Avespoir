using DSharpPlus;

namespace AvespoirTest.Core.Modules.Message {

	class MessageCreated {

		DiscordClient Bot = Client.Bot;

		internal MessageCreated() => Bot.MessageCreated += async Message_Objects => await MessageEvent.MainEvent(Message_Objects);
	}
}
