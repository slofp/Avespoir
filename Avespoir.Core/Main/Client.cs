using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Events;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core {

	class Client {

		//internal static Dictionary<ulong, VCInfo> ConnectedVoiceChannel_Dict = new Dictionary<ulong, VCInfo>();

		internal static readonly DiscordClient Bot = new DiscordClient(ClientConfig.WebSocketConfig());

		internal static async Task Main() {
			Bot.Ready += ReadyEvent.Main;

			Bot.MessageCreated += MessageEvent.Main;

			Bot.GuildMemberAdded += GuildMemberAddEvent.Main;

			Bot.GuildMemberRemoved += GuildMemberRemoveEvent.Main;

			Bot.Resumed += LogEvents.Resumed;

			Bot.SocketClosed += LogEvents.SocketClosed;

			Bot.SocketErrored += LogEvents.SocketErrored;

			Bot.SocketOpened += LogEvents.SocketOpened;

			Bot.UnknownEvent += LogEvents.UnknownEvent;

			Bot.Heartbeated += LogEvents.Heartbeated;

			Bot.MessageReactionAdded += MessageReactionAddedEvent.Main;

			Bot.MessageDeleted += MessageDeletedEvent.Main;

			await Bot.ConnectAsync().ConfigureAwait(false);

			AppDomain.CurrentDomain.ProcessExit += ConsoleExitEvent.Main;

			Console.CancelKeyPress += ConsoleExitEvent.Main;

			await Task.Delay(-1);
		}
	}
}
