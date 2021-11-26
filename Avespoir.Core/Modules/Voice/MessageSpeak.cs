using Avespoir.AITalk;
using Avespoir.Core.Extends;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.VoiceNext;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Voice {

	class MessageSpeak {

		internal static async Task Load(MessageObject Message_Object) {
			// GC...
			if (Message_Object.IsPrivate || Message_Object.Member.IsBot || Message_Object.IsTTS) return;

			string Message_Text = PatternMatch.ReplaceToEmojiName(Message_Object.Content);
			if (string.IsNullOrWhiteSpace(Message_Text)) {
				Log.Error("No context. couldnt speaking.");
				return;
			}
			Log.Debug($"Speak Text: {Message_Text}");

			VoiceNextConnection Connection = Client.Bot.GetVoiceNext().GetConnection(Message_Object.Guild);
			if (Connection is null) {
				Log.Error("No Voice connection.");
				return;
			}

			if (!VoiceStatus.GuildsVoiceStatus.TryGetValue(Message_Object.Guild.Id, out VoiceStatus GuildVoiceStatus)) {
				Log.Error("Connected, but no VoiceStatus");
				return;
			}

			if (!GuildVoiceStatus.IsTTS) {
				Log.Info("Guild VoiceType is not TTS");
				return;
			}

			bool IsAllow = false;
			foreach(ulong id in GuildVoiceStatus.ReadChannels) {
				if (Message_Object.Channel.Id == id) {
					IsAllow = true;
					break;
				}
			}

			if (!IsAllow) {
				Log.Info("Not allow read channel");
				return;
			}

			SpeakParameter Param = new SpeakParameter {
				Text = Message_Text
			};
			if (!Client.Voiceroid.TextToKana(Param)) {
				Log.Error("couldnt text to kana");
				return;
			}

			byte[] VoiceBytes = Client.Voiceroid.KanaToDiscordPCM(Param, "ffmpeg");
			if (VoiceBytes is null) {
				Log.Error("VoiceBytes id null");
				return;
			}

			VoiceTransmitSink Transmit = Connection.GetTransmitSink();

			await Transmit.WriteAsync(VoiceBytes).ConfigureAwait(false);
			//await VoiceStream.CopyToAsync(Transmit);
			await Transmit.FlushAsync().ConfigureAwait(false);
		}
	}
}
