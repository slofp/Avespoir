﻿using Avespoir.Core.Extends;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.LevelSystems {

	class StackMessage {

		internal List<DiscordMessage> StackMessages = new List<DiscordMessage>();

		internal bool AllowExp { get; set; } = true;

		private long LastMessageTick = 0;

		private readonly DiscordMessage FirstMessageObject;

		internal StackMessage(MessageObject FirstMessageObject) {
			this.FirstMessageObject = FirstMessageObject.SourceMessageCreateEventArgs.Message;

			AddStackMessage(this.FirstMessageObject);
		}

		internal async Task<StackMessage> Start() {
			Log.Debug("Stack Start");

			AwaitMessageInfo AwaitMessage_Info = new AwaitMessageInfo();

			AwaitMessage_Info.Init(FirstMessageObject);

			try {
				while (DateTime.Now.Ticks - LastMessageTick < Math.Pow(10, 7) * 60) {
					if (AwaitMessage_Info.Status == AwaitMessageStatus.Pending) continue;

					DiscordMessage WaitedMessage = await FirstMessageObject.Channel.GetMessageAsync(AwaitMessage_Info.MessageID).ConfigureAwait(false);

					if (WaitedMessage.Author.Id != StackMessages[0].Author.Id) {
						Log.Debug("Stack End"); // It's responding, but it's not working.
						if (WaitedMessage.Author.IsBot) AllowExp = false;
						AddStackMessage(WaitedMessage);
						return this;
					}
					else {
						Log.Debug("Stack Add"); // It's responding but not working after the first time.
						AddStackMessage(WaitedMessage);
						lock (AwaitMessage_Info) {
							AwaitMessage_Info.MessageID = 0;
							AwaitMessage_Info.Status = AwaitMessageStatus.Pending;
						}
					}
				}

				AllowExp = false;

				return this;
			}
			finally {
				AwaitMessage_Info.Finalize_(FirstMessageObject);
			}
		}

		private void AddStackMessage(DiscordMessage Message) {
			LastMessageTick = Message.Timestamp.Ticks;

			StackMessages.Add(Message);
		}
	}
}
