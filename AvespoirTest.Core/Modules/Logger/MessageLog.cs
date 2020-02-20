using DSharpPlus.EventArgs;

namespace AvespoirTest.Core.Modules.Logger {

	class MessageLog {
		
		// Debug only
		internal MessageLog(MessageCreateEventArgs Message_Objects) {
			new DebugLog("Message Log" +
						$"\n[Guild] Name: {Message_Objects.Guild.Name}, ID: {Message_Objects.Guild.Id}" +
						$"\n[Channel] Name: {Message_Objects.Channel.Name}, ID: {Message_Objects.Channel.Id}" +
						$"\n[Author] Name: {Message_Objects.Author.Username}#{Message_Objects.Author.Discriminator}, ID: {Message_Objects.Author.Id}, Bot: {Message_Objects.Author.IsBot}" +
						$"\n[Message] Content: {Message_Objects.Message.Content}, ID: {Message_Objects.Message.Id}");
		}
	}
}
