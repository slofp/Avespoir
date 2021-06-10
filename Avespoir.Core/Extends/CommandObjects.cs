using Avespoir.Core.Language;
using System;

namespace Avespoir.Core.Extends {

	sealed class CommandObjects : MessageObject {

		internal string[] CommandArgs { get; }

		internal JsonScheme.Language Language { get; }

		internal Database.Enums.Language LanguageType { get; }

		internal CommandObjects(MessageObject Message_Object) : base(Message_Object) {
			CommandArgs = Content.Trim().Split(" ");
			string GuildLanguageString = !IsPrivate ? Database.DatabaseMethods.GuildConfigMethods.LanguageFind(Guild.Id) : null;
			if (GuildLanguageString != null && Enum.TryParse(GuildLanguageString, true, out Database.Enums.Language GuildLanguage)) {
				Language = new GetLanguage(GuildLanguage).Language_Data;
				LanguageType = GuildLanguage;
			}
			else {
				Language = new GetLanguage(Database.Enums.Language.ja_JP).Language_Data;
				LanguageType = Database.Enums.Language.ja_JP;
			}
		}
	}
}
