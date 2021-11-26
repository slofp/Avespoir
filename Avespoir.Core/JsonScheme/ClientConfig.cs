using Avespoir.Core.Configs;
using System;

namespace Avespoir.Core.JsonScheme {

	public class ClientConfig {

		#region Bot Config

		public string Token {
			get {
				return Configs.ClientConfig.Token;
			}
			set {
				Configs.ClientConfig.Token = value;
			}
		}

		public string BotownerId {
			get {
				return Convert.ToString(Configs.ClientConfig.BotownerId);
			}
			set {
				Configs.ClientConfig.BotownerId = Convert.ToUInt64(value);
			}
		}
		#endregion

		#region Prefix Config

		public string Prefix {
			get {
				return CommandConfig.Prefix;
			}
			set {
				CommandConfig.Prefix = value;
			}
		}
		#endregion

		#region Voiceroid Config

		public string VoiceroidDirectoryPath {
			get {
				return Configs.ClientConfig.VoiceroidDirectoryPath;
			}
			set {
				Configs.ClientConfig.VoiceroidDirectoryPath = value;
			}
		}

		public string VoiceroidAuthSeed {
			get {
				return Configs.ClientConfig.VoiceroidAuthSeed;
			}
			set {
				Configs.ClientConfig.VoiceroidAuthSeed = value;
			}
		}
		#endregion
	}
}
