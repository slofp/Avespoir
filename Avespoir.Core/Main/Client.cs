using Avespoir.AITalk;
using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.VoiceNext;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core {

	class Client {

		//internal static Dictionary<ulong, VCInfo> ConnectedVoiceChannel_Dict = new Dictionary<ulong, VCInfo>();

		internal static readonly DiscordClient Bot = new DiscordClient(ClientConfig.WebSocketConfig());

		internal static Voiceroid2 Voiceroid { get; private set; }

		internal static void VoiceInit() {
			Log.Info("Aitalk initializing...");
			if (string.IsNullOrWhiteSpace(ClientConfig.VoiceroidDirectoryPath))
				throw new ArgumentNullException(nameof(ClientConfig.VoiceroidDirectoryPath));
			if (string.IsNullOrWhiteSpace(ClientConfig.VoiceroidAuthSeed))
				throw new ArgumentNullException(nameof(ClientConfig.VoiceroidAuthSeed));
			Voiceroid = new Voiceroid2(ClientConfig.VoiceroidDirectoryPath, ClientConfig.VoiceroidAuthSeed);
			Log.Info("Aitalk initialized!");
		}

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

			Bot.VoiceStateUpdated += VoiceStateUpdatedEvent.Main;

			Bot.UseVoiceNext();

			await Bot.ConnectAsync().ConfigureAwait(false);

			AppDomain.CurrentDomain.ProcessExit += ConsoleExitEvent.Main;

			Console.CancelKeyPress += ConsoleExitEvent.Main;

			await Task.Delay(-1);
		}
	}
}
