using Avespoir.Core.Database.DatabaseMethods;
using Avespoir.Core.Language;
using System;

namespace Avespoir.Core.Extends {

	sealed class CommandObject : MessageObject {

		internal string[] CommandArgs { get; }

		internal JsonScheme.Language Language { get; }

		internal Database.Enums.Language LanguageType { get; }

		internal CommandObject(MessageObject Message_Object) : base(Message_Object) {
			CommandArgs = Content.Trim().Split(" ");
			Database.Enums.Language GuildLanguage = !IsPrivate ? GuildConfigMethods.LanguageFind(Guild.Id) : Database.Enums.Language.ja_JP;
			Language = new GetLanguage(GuildLanguage).Language_Data;
			LanguageType = GuildLanguage;
		}
	}
}
