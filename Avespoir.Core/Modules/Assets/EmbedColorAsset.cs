using DSharpPlus.Entities;

namespace Avespoir.Core.Modules.Assets {

	static class EmbedColorAsset {

		private const int Success = 0x00B06B;

		private const int Failed = 0xFF4B00;

		private const int Warning = 0xF2E700;

		private const int Normal = 0x1971FF;

		private const int Critical = 0x990099;

		private const int Danger = 0xF6AA00;

		internal static DiscordColor SuccessColor => new DiscordColor(Success);

		internal static DiscordColor FailedColor => new DiscordColor(Failed);

		internal static DiscordColor WarningColor => new DiscordColor(Warning);

		internal static DiscordColor NormalColor => new DiscordColor(Normal);

		internal static DiscordColor CriticalColor => new DiscordColor(Critical);

		internal static DiscordColor DangerColor => new DiscordColor(Danger);
	}
}
