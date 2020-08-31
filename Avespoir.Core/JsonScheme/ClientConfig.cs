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

		public string MainPrefix {
			get {
				return CommandConfig.MainPrefix;
			}
			set {
				CommandConfig.MainPrefix = value;
			}
		}

		public string PublicPrefixTag {
			get {
				return CommandConfig.PublicPrefixTag;
			}
			set {
				CommandConfig.PublicPrefixTag = value;
			}
		}

		public string ModeratorPrefixTag {
			get {
				return CommandConfig.ModeratorPrefixTag;
			}
			set {
				CommandConfig.ModeratorPrefixTag = value;
			}
		}

		public string BotownerPrefixTag {
			get {
				return CommandConfig.BotownerPrefixTag;
			}
			set {
				CommandConfig.BotownerPrefixTag = value;
			}
		}
		#endregion
	}
}
