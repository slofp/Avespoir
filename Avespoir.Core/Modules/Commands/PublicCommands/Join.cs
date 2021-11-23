using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("join", RoleLevel.Public)]
	class Join : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await Command_Object.Channel.SendMessageAsync("何も入力されていません");
				return;
			}

			DiscordChannel VoiceChannel = Command_Object.Member.VoiceState?.Channel;

			if (VoiceChannel is null) {
				await Command_Object.Channel.SendMessageAsync("入ってません");

				return;
			}

			try {
				VoiceNextConnection Connection = await VoiceChannel.ConnectAsync();

				using var voiceroid = new AITalk.Voiceroid2(@"C:/Program Files (x86)/AHS/VOICEROID2", "ORXJC6AIWAUKDpDbH2al");
				var Param = new AITalk.SpeakParameter {
					Text = msgs[0]
				};

				byte[] VoiceBytes = voiceroid.KanaToDiscordPCM(Param, "ffmpeg");
				if (VoiceBytes is null) {
					await Command_Object.Channel.SendMessageAsync("nullでした");
					Client.Bot.GetVoiceNext().GetConnection(Command_Object.Guild).Disconnect();
					return;
				}

				VoiceTransmitSink Transmit = Connection.GetTransmitSink();

				await Transmit.WriteAsync(VoiceBytes).ConfigureAwait(false);
				//await VoiceStream.CopyToAsync(Transmit);
				await Transmit.FlushAsync().ConfigureAwait(false);

				await Connection.WaitForPlaybackFinishAsync();

				Client.Bot.GetVoiceNext().GetConnection(Command_Object.Guild).Disconnect();
			}
			catch (Exception error) {
				Log.Error(error);
			}

			//Stream TextVoice = new MemoryStream(Voicebyte);



			await Task.Delay(0);
		}
	}
}
