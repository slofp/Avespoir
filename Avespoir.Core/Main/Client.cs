using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Audio;
using Avespoir.Core.Modules.Events;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core {

	class Client {

		internal static Dictionary<ulong, VCInfo> ConnectedVoiceChannel_Dict = new Dictionary<ulong, VCInfo>();

		internal static readonly DiscordShardedClient Bot = new DiscordShardedClient(ClientConfig.WebSocketConfig());

		internal static async Task Main() {
			Bot.ShardReady += ReadyEvent.Main;

			Bot.MessageReceived += MessageEvent.Main;

			Bot.UserJoined += GuildMemberAddEvent.Main;

			Bot.UserLeft += GuildMemberRemoveEvent.Main;

			Bot.Log += LogEvents.LogEvent;

			Bot.LoggedIn += LogEvents.LoggedInEvent;

			Bot.LoggedOut += LogEvents.LoggedOutEvent;

			Bot.ShardConnected += LogEvents.ConnectedEvent;

			Bot.ShardDisconnected += LogEvents.DisconnectedEvent;

			Bot.ShardLatencyUpdated += LogEvents.LatencyUpdated;

			await Bot.LoginAsync(Discord.TokenType.Bot, ClientConfig.Token).ConfigureAwait(false);

			await Bot.StartAsync().ConfigureAwait(false);

			AppDomain.CurrentDomain.ProcessExit += ConsoleExitEvent.Main;

			Console.CancelKeyPress += ConsoleExitEvent.Main;

			await Task.Delay(-1);
		}
	}
}
