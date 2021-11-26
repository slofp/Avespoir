using System;

namespace Avespoir.Core.Modules.Voice {

	[Flags]
	internal enum VoiceType {

		None = 0,

		TTS = 1,

		Music = 1 << 1,

		Record = 1 << 2

	}
}
