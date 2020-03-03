namespace AvespoirTest.Core.Configs {

	public class GetClientConfigJson {

		public string Token {
			get {
				return ClientConfig.Token;
			}
			set {
				ClientConfig.Token = value;
			}
		}

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
	}
}
