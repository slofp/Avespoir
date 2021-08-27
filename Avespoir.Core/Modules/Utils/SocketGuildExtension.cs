using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avespoir.Core.Modules.Utils {

	static class SocketGuildExtension {

		internal static Union<SocketTextChannel, SocketVoiceChannel> GetTextOrVoiceChannel(this SocketGuild Guild, ulong Id) {
			SocketTextChannel TextChannel = Guild.GetTextChannel(Id);
			if (!(TextChannel is null)) return TextChannel;

			SocketVoiceChannel VoiceChannel = Guild.GetVoiceChannel(Id);
			if (!(VoiceChannel is null)) return VoiceChannel;

			return new Union<SocketTextChannel, SocketVoiceChannel>(null);
		}
	}
}
