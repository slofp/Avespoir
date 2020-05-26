using Avespoir.Core.Modules.Commands;
using Avespoir.Core.Modules.LevelSystems;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class MessageEvent {

		internal static Task Main(MessageCreateEventArgs Message_Objects) {
			Log.Debug("MessageEvent " + "Start...");

			MessageLog.Main(Message_Objects).ConfigureAwait(false);

			if (!Message_Objects.Message.Author.IsBot) LevelSystem.Main(Message_Objects).ConfigureAwait(false);

			CommandRegister.PublicCommands(Message_Objects).ConfigureAwait(false);

			CommandRegister.ModeratorCommands(Message_Objects).ConfigureAwait(false);

			CommandRegister.BotownerCommands(Message_Objects).ConfigureAwait(false);

			Log.Debug("MessageEvent " + "End...");

			return Task.CompletedTask;
		}
	}
}
