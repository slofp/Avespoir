using Avespoir.Core.Extends;
using Avespoir.Core.Modules.Commands;
using Avespoir.Core.Modules.LevelSystems;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avespoir.Core.Modules.Voice;

namespace Avespoir.Core.Modules.Events {

	class MessageEvent {

		// ulong -> Channel ID
		private static readonly List<ulong> LevelSystem_Queue = new List<ulong>();

		// ulong -> Channel ID, 
		internal static readonly Dictionary<ulong, List<AwaitMessageInfo>> AwaitMessageInfo_List_Dict = new Dictionary<ulong, List<AwaitMessageInfo>>();

		internal static Task Main(DiscordClient Bot, MessageCreateEventArgs Message) {
			Log.Debug("MessageEvent " + "Start...");

			MessageObject Message_Object = new MessageObject(Message);

			MessageLog.Main(Message_Object).ConfigureAwait(false);

			Task.Run(() => MessageSpeak.Load(Message_Object)).ConfigureAwait(false);

			AwaitMessageProcess(Message_Object).ConfigureAwait(false);

			// If someone talks to you while you are waiting for a message, it will count as Level.
			Task.Run(() => {
				if (!Message_Object.Author.IsBot &&
				!Message_Object.IsPrivate &&
				!LevelSystem_Queue.Contains(Message_Object.Channel.Id) &&
				Database.DatabaseMethods.GuildConfigMethods.LevelSwitchFind(Message_Object.Guild.Id))
					LevelSystemInit(Message_Object);
				else Log.Debug("Exist Task");
			}).ConfigureAwait(false);

			CommandRegister.Start(Message_Object).ConfigureAwait(false);

			Log.Debug("MessageEvent " + "End...");

			return Task.CompletedTask;
		}

		private static void LevelSystemInit(MessageObject Message_Object) {
			LevelSystem_Queue.Add(Message_Object.Channel.Id);
			LevelSystem.Main(Message_Object).ContinueWith(_ => {
				LevelSystem_Queue.Remove(Message_Object.Channel.Id);
			}).ConfigureAwait(false);
		}

		private static Task AwaitMessageProcess(MessageObject Message_Object) {
			if (AwaitMessageInfo_List_Dict.TryGetValue(Message_Object.Channel.Id, out List<AwaitMessageInfo> AwaitMessageInfo_List)) {
				IEnumerable<AwaitMessageInfo> FindAwaitMessageInfo_Enum =
					from AwaitMessage_Info in AwaitMessageInfo_List
					where AwaitMessage_Info.UserID == 0 || AwaitMessage_Info.UserID == Message_Object.Author.Id
					select AwaitMessage_Info
				;

				foreach (AwaitMessageInfo FindAwaitMessageInfo in FindAwaitMessageInfo_Enum) {
					if (FindAwaitMessageInfo.Status == AwaitMessageStatus.Success) {
						Log.Warning("AwaitMessage for this user has already succeeded.");
						return Task.CompletedTask;
					}

					lock (FindAwaitMessageInfo) {
						FindAwaitMessageInfo.MessageID = Message_Object.Id;
						FindAwaitMessageInfo.Status = AwaitMessageStatus.Success;
					}
				}
			}

			return Task.CompletedTask;
		}
	}
}
