using Avespoir.Core.Modules.Commands;
using Avespoir.Core.Modules.LevelSystems;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class MessageEvent {

		// ulong -> Channel ID
		private static readonly List<ulong> LevelSystem_Queue = new List<ulong>();

		// ulong -> Channel ID, 
		internal static readonly Dictionary<ulong, List<AwaitMessageInfo>> AwaitMessageInfo_List_Dict = new Dictionary<ulong, List<AwaitMessageInfo>>();

		internal static Task Main(DiscordClient Bot, MessageCreateEventArgs Message_Objects) {
			Log.Debug("MessageEvent " + "Start...");

			MessageLog.Main(Message_Objects).ConfigureAwait(false);

			AwaitMessageProcess(Message_Objects).ConfigureAwait(false);

			// If someone talks to you while you are waiting for a message, it will count as Level.
			if (!Message_Objects.Message.Author.IsBot &&
				!Message_Objects.Channel.IsPrivate &&
				!LevelSystem_Queue.Contains(Message_Objects.Channel.Id) &&
				Database.DatabaseMethods.GuildConfigMethods.LevelSwitchFind(Message_Objects.Guild.Id))
				Task.Run(() => LevelSystemInit(Bot, Message_Objects)).ConfigureAwait(false);
			else Log.Debug("Exist Task");

			CommandRegister.Start(Bot, Message_Objects).ConfigureAwait(false);

			Log.Debug("MessageEvent " + "End...");

			return Task.CompletedTask;
		}

		private static void LevelSystemInit(DiscordClient Bot, MessageCreateEventArgs Message_Objects) {
			LevelSystem_Queue.Add(Message_Objects.Channel.Id);
			LevelSystem.Main(Bot, Message_Objects).ContinueWith(_ => {
				LevelSystem_Queue.Remove(Message_Objects.Channel.Id);
			}).ConfigureAwait(false);
		}

		private static Task AwaitMessageProcess(MessageCreateEventArgs Message_Objects) {
			if (AwaitMessageInfo_List_Dict.TryGetValue(Message_Objects.Channel.Id, out List<AwaitMessageInfo> AwaitMessageInfo_List)) {
				IEnumerable<AwaitMessageInfo> FindAwaitMessageInfo_Enum =
					from AwaitMessage_Info in AwaitMessageInfo_List
					where AwaitMessage_Info.UserID == 0 || AwaitMessage_Info.UserID == Message_Objects.Author.Id
					select AwaitMessage_Info
				;

				foreach (AwaitMessageInfo FindAwaitMessageInfo in FindAwaitMessageInfo_Enum) {
					if (FindAwaitMessageInfo.Status == AwaitMessageStatus.Success) {
						Log.Warning("AwaitMessage for this user has already succeeded.");
						return Task.CompletedTask;
					}

					lock (FindAwaitMessageInfo) {
						FindAwaitMessageInfo.MessageID = Message_Objects.Message.Id;
						FindAwaitMessageInfo.Status = AwaitMessageStatus.Success;
					}
				}
			}

			return Task.CompletedTask;
		}
	}
}
