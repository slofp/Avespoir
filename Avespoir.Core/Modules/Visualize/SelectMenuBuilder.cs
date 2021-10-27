using DSharpPlus.Entities;
using System.Collections.Generic;

namespace Avespoir.Core.Modules.Visualize {

	class SelectMenuBuilder {

		private readonly List<DiscordSelectComponentOption> SelectMenuComponects;

		internal SelectMenuBuilder() {
			SelectMenuComponects = new List<DiscordSelectComponentOption>();
		}

		internal DiscordSelectComponent Build(string CustomID, string PlaceHolder,
		bool Disabled = false, int MinOptions = 1, int MaxOptions = 1)
			=> new DiscordSelectComponent(CustomID, PlaceHolder, SelectMenuComponects, Disabled, MinOptions, MaxOptions);

		internal void AddSelectMenu(string SelectTitle, string Value,
		string Description = null, bool IsDefault = false, DiscordComponentEmoji Emoji = null)
			=> SelectMenuComponects.Add(
				new DiscordSelectComponentOption(SelectTitle, Value, Description, IsDefault, Emoji)
			);
	}
}
