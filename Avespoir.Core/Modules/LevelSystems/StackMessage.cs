using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;

namespace Avespoir.Core.Modules.LevelSystems {

	class StackMessage {

		internal List<DiscordMessage> StackDiscordMessages = new List<DiscordMessage>();

		internal bool AllowExp = true;

		private long LastMessageTick = 0;

		internal StackMessage(DiscordMessage FirstMessage) {
			AddStackMessage(FirstMessage);

			Log.Debug("Stack Start");

			while (DateTime.Now.Ticks - LastMessageTick < Math.Pow(10, 7) * 60) {
				DiscordMessage LastMessage = FirstMessage.Channel.GetMessageAsync(FirstMessage.Channel.LastMessageId).ConfigureAwait(false).GetAwaiter().GetResult();

				if (LastMessage.Id == StackDiscordMessages[StackDiscordMessages.Count - 1].Id) {
					continue;
				}
				else if (LastMessage.Author.Id != StackDiscordMessages[0].Author.Id) {
					Log.Debug("Stack End");
					if (LastMessage.Author.IsBot) AllowExp = false;
					return;
				}
				else {
					AddStackMessage(LastMessage);
					Log.Debug("Stack Added");
				}
			}

			AllowExp = false;
		}

		private void AddStackMessage(DiscordMessage Message) {
			LastMessageTick = Message.Timestamp.Ticks;

			StackDiscordMessages.Add(Message);
		}
	}
}
