using DSharpPlus.Entities;

namespace AvespoirTest.Core.Modules.Utils {

	class EmojiConverter {

		internal static string ConvertEmoji(DiscordGuildEmoji Emoji) {
			string StringEmoji = "<";
			StringEmoji += Emoji.GetDiscordName();
			StringEmoji += Emoji.Id;
			StringEmoji += ">";

			return StringEmoji;
		}
	}
}
