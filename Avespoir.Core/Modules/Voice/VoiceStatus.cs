using System;
using System.Collections.Generic;
using System.Text;

namespace Avespoir.Core.Modules.Voice {

	class VoiceStatus {

		internal static Dictionary<ulong, VoiceStatus> GuildsVoiceStatus = new Dictionary<ulong, VoiceStatus>();

		internal List<ulong> ReadChannels { get; }

		internal VoiceType CurrentVoiceType { get; private set; }

		internal bool IsTTS => CurrentVoiceType.HasFlag(VoiceType.TTS);

		internal bool IsMusic => CurrentVoiceType.HasFlag(VoiceType.Music);

		internal bool IsRecord => CurrentVoiceType.HasFlag(VoiceType.Record);

		internal ulong GuildId { get; }

		internal ulong CurrentChannelId { get; private set; }

		internal VoiceStatus(ulong GuildId, ulong CurrentChannelId) {
			this.GuildId = GuildId;
			this.CurrentChannelId = CurrentChannelId;
			CurrentVoiceType = VoiceType.None;

			ReadChannels = new List<ulong>();

			GuildsVoiceStatus.Add(GuildId, this);
		}

		internal bool SetTTS() {
			if (CurrentVoiceType != VoiceType.None)
				return false;
			CurrentVoiceType |= VoiceType.TTS;
			return true;
		}

		internal bool SetMusic() {
			return false;
		}

		internal bool SetRecord() {
			return false;
		}

		internal bool RemoveTTS() {
			if (!IsTTS) return false;
			ReadChannels.Clear();
			CurrentVoiceType &= ~VoiceType.TTS;
			return true;
		}

		internal bool RemoveMusic() {
			if (!IsMusic) return false;
			CurrentVoiceType &= ~VoiceType.Music;
			return true;
		}

		internal bool RemoveRecord() {
			if (!IsRecord) return false;
			CurrentVoiceType &= ~VoiceType.Record;
			return true;
		}

		internal bool RemoveAll() {
			if (IsTTS) return RemoveTTS();
			else if (IsMusic) return RemoveMusic();
			else if (IsRecord) return RemoveRecord();
			else return false;
		}
	}
}
