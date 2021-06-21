using Discord;

namespace Avespoir.Core.Modules.Utils {

	class EmojiConverter {

		internal static string ConvertEmoji(GuildEmote Emoji) => $"<:{Emoji.Name}:{Emoji.Id}>";
	}
}
