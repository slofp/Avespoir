using Avespoir.Core.Modules.Commands;
using Avespoir.Core.Modules.LevelSystems;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class MessageEvent {

		// ulong -> Channel ID
		private static readonly List<ulong> LevelSystem_Queue = new List<ulong>();

		internal static Task Main(MessageCreateEventArgs Message_Objects) {
			Log.Debug("MessageEvent " + "Start...");

			MessageLog.Main(Message_Objects).ConfigureAwait(false);

			bool LevelSystemExist = LevelSystem_Queue.Contains(Message_Objects.Channel.Id);

			if (!Message_Objects.Message.Author.IsBot &&
				!Message_Objects.Channel.IsPrivate &&
				!LevelSystemExist &&
				Database.DatabaseMethods.GuildConfigMethods.LevelSwitchFind(Message_Objects.Guild.Id))
				Task.Run(() => LevelSystemInit(Message_Objects)).ConfigureAwait(false);
			else Log.Debug("Exist Task");

			CommandRegister.PublicCommands(Message_Objects).ConfigureAwait(false);

			CommandRegister.ModeratorCommands(Message_Objects).ConfigureAwait(false);

			CommandRegister.BotownerCommands(Message_Objects).ConfigureAwait(false);

			Log.Debug("MessageEvent " + "End...");

			return Task.CompletedTask;
		}

		private static void LevelSystemInit(MessageCreateEventArgs Message_Objects) {
			LevelSystem_Queue.Add(Message_Objects.Channel.Id);
			LevelSystem.Main(Message_Objects).ContinueWith(_ => {
				Log.Critical(Message_Objects.Channel.Id);
				LevelSystem_Queue.Remove(Message_Objects.Channel.Id);
			}).ConfigureAwait(false);
		}
	}
}
