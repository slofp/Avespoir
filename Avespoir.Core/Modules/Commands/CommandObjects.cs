using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;

namespace Avespoir.Core.Modules.Commands {
	sealed class CommandObjects {

		internal MessageCreateEventArgs MessageObjects { get; }

		internal string[] CommandArgs { get; }

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
