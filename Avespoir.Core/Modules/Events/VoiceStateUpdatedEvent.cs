using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class VoiceStateUpdatedEvent {

		internal static Task Main(DiscordClient _, VoiceStateUpdateEventArgs Args) {
			Log.Debug("VoiceStateUpdated Event Start");
			Log.Debug($"Name: {Args.User.Username}");
			string BeforeId;
			if (Args.Before?.Channel is null) BeforeId = "New";
			else BeforeId = Args.Before.Channel.Id.ToString();
			string AfterId;
			if (Args.After?.Channel is null) AfterId = "Exit";
			else AfterId = Args.After.Channel.Id.ToString();
			Log.Debug($"Move: {BeforeId} -> {AfterId}");
			Log.Debug("VoiceStateUpdated Event End");

			return Task.CompletedTask;
		}
	}
}
