namespace AvespoirTest.Core.Configs {

	class CommandConfig {

		#region Prefix Configs

		internal static string MainPrefix { get; set; } = "";

		internal static string PublicPrefixTag { get; set; } = "$";

		internal static string ModeratorPrefixTag { get; set; } = "@";

		internal static string BotownerPrefixTag { get; set; } = ">";

		#endregion

		#region MixPrefixs
		internal static string PublicPrefix {
			get {
				return MainPrefix + PublicPrefixTag;
			}
		}

		internal static string ModeratorPrefix {
			get {
				return MainPrefix + ModeratorPrefixTag;
			}
		}

		internal static string BotownerPrefix {
			get {
				return MainPrefix + BotownerPrefixTag;
			}
		}
		#endregion
	}
}
