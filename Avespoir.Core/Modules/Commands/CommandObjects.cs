using Avespoir.Core.Language;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;

namespace Avespoir.Core.Modules.Commands {
	class CommandObjects {

		internal MessageCreateEventArgs MessageObjects { get; }

		internal string[] CommandArgs { get; }

		internal JsonScheme.Language Language { get; }

		#region Extend MessageCreateEventArgs

		internal BaseDiscordClient Client { get; }

		internal DiscordMessage Message { get; }

		internal DiscordChannel Channel { get; }

		internal DiscordGuild Guild { get; }

		internal DiscordUser Author { get; }

		internal IReadOnlyList<DiscordUser> MentionedUsers { get; }

		internal IReadOnlyList<DiscordRole> MentionedRoles { get; }

		internal IReadOnlyList<DiscordChannel> MentionedChannels { get; }

		#endregion

		#region Extend CommandContext

		internal DiscordMember Member { get; }

		internal DiscordUser User { get; }

		// Not Use;
		// This is google Translate at the time↓
		internal Lazy<DiscordMember> 怠zyな { get; }
		
		#endregion

		internal CommandObjects(MessageCreateEventArgs Message_Objects) {
			MessageObjects = Message_Objects;
			CommandArgs = MessageObjects.Message.Content.Trim().Split(" ");
			if (!Message_Objects.Channel.IsPrivate) {
				string GuildLanguageString = Database.DatabaseMethods.GuildConfigMethods.LanguageFind(Message_Objects.Guild.Id);
				if (GuildLanguageString == null) Language = new GetLanguage(Database.Enums.Language.ja_JP).Language_Data;
				else {
					if (!Enum.TryParse(GuildLanguageString, true, out Database.Enums.Language GuildLanguage))
						Language = new GetLanguage(Database.Enums.Language.ja_JP).Language_Data;
					else Language = new GetLanguage(GuildLanguage).Language_Data;
				}
			}
			else Language = new GetLanguage(Database.Enums.Language.ja_JP).Language_Data;

			Client = Message_Objects.Client;
			Message = Message_Objects.Message;
			Channel = Message_Objects.Channel;
			Guild = Message_Objects.Guild;
			Author = Message_Objects.Author;
			MentionedUsers = Message_Objects.MentionedUsers;
			MentionedRoles = Message_Objects.MentionedRoles;
			MentionedChannels = Message_Objects.MentionedChannels;

			User = Author;
			Member = Message_Objects.Guild?.GetMemberAsync(User.Id).ConfigureAwait(false).GetAwaiter().GetResult();
		}
	}
}
