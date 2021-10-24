using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class LogEvents {

		internal static Task UnknownEvent(DiscordClient Bot, UnknownEventArgs Args) {
			Log.Info($"ShardID: {Bot.ShardId}, Unknown Event: {Args.EventName}\n{Args.Json}");
			return Task.CompletedTask;
		}

		internal static Task SocketOpened(DiscordClient Bot, SocketEventArgs Args) {
			Log.Info($"ShardID: {Bot.ShardId}, Socket opened!");
			return Task.CompletedTask;
		}

		internal static Task SocketErrored(DiscordClient Bot, SocketErrorEventArgs Args) {
			Log.Error($"ShardID: {Bot.ShardId}, Socket errored", Args.Exception);
			return Task.CompletedTask;
		}

		internal static Task SocketClosed(DiscordClient Bot, SocketCloseEventArgs Args) {
			Log.Info($"ShardID: {Bot.ShardId}, Socket closed: CloseCode:{Args.CloseCode}\n{Args.CloseMessage}");
			return Task.CompletedTask;
		}

		internal static Task Resumed(DiscordClient Bot, ReadyEventArgs Args) {
			Log.Info($"ShardID: {Bot.ShardId}, Resumed!");
			return Task.CompletedTask;
		}

		internal static Task Heartbeated(DiscordClient Bot, HeartbeatEventArgs Args) {
			Log.Info($"ShardID: {Bot.ShardId}, Ping: {Args.Ping}ms");
			return Task.CompletedTask;
		}
	}
}
