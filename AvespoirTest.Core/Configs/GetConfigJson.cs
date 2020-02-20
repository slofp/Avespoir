using Newtonsoft.Json;

namespace AvespoirTest.Core.Configs {

	[JsonObject("Config")]
	public class GetConfigJson {

		[JsonProperty("Token")]
		public string Token {
			get {
				return ClientConfig.Token;
			}
			set {
				ClientConfig.Token = value;
			}
		}

		[JsonProperty("MainPrefix")]
		public string MainPrefix {
			get {
				return CommandConfig.MainPrefix;
			}
			set {
				CommandConfig.MainPrefix = value;
			}
		}

		[JsonProperty("PublicPrefixTag")]
		public string PublicPrefixTag {
			get {
				return CommandConfig.PublicPrefixTag;
			}
			set {
				CommandConfig.PublicPrefixTag = value;
			}
		}

		[JsonProperty("ModeratorPrefixTag")]
		public string ModeratorPrefixTag {
			get {
				return CommandConfig.ModeratorPrefixTag;
			}
			set {
				CommandConfig.ModeratorPrefixTag = value;
			}
		}

		[JsonProperty("BotownerPrefixTag")]
		public string BotownerPrefixTag {
			get {
				return CommandConfig.BotownerPrefixTag;
			}
			set {
				CommandConfig.BotownerPrefixTag = value;
			}
		}
	}
}
