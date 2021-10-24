using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace Avespoir.Core.Modules.Utils {

	class EmojiConverter {

		internal static string ConvertEmoji(DiscordEmoji Emoji) => $"<:{Emoji.Name}:{Emoji.Id}>";
	}
}
