using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Avespoir.Core.Extends {

	class MessageObject /*: IMessage*/ {

		internal SocketMessage SourceSocketMessage { get; }

		#region IMessage Extend

		//internal MessageType Type { get; }

		internal MessageSource Source { get; }

		internal bool IsTTS { get; }

		internal bool IsPinned { get; }

		internal bool IsSuppressed { get; }

		internal bool MentionedEveryone { get; }

		internal string Content { get; }

		internal DateTimeOffset Timestamp { get; }

		internal DateTimeOffset? EditedTimestamp { get; }

		internal ISocketMessageChannel Channel { get; }

		internal SocketUser Author { get; }

		internal IReadOnlyCollection<Attachment> Attachments { get; }

		internal IReadOnlyCollection<Embed> Embeds { get; }

		internal IReadOnlyCollection<ITag> Tags { get; }

		internal IReadOnlyCollection<SocketGuildChannel> MentionedChannels { get; }

		internal IReadOnlyCollection<ulong> MentionedChannelIds => MentionedChannels.Select(x => x.Id).ToImmutableArray();

		internal IReadOnlyCollection<SocketRole> MentionedRoles { get; }

		internal IReadOnlyCollection<ulong> MentionedRoleIds => MentionedRoles.Select(x => x.Id).ToImmutableArray();

		internal IReadOnlyCollection<SocketUser> MentionedUsers { get; }

		internal IReadOnlyCollection<ulong> MentionedUserIds => MentionedUsers.Select(x => x.Id).ToImmutableArray();

		internal MessageActivity Activity { get; }

		internal MessageApplication Application { get; }

		internal MessageReference Reference { get; }

		internal IReadOnlyDictionary<IEmote, ReactionMetadata> Reactions { get; }

		internal IReadOnlyCollection<Sticker> Stickers { get; }

		internal MessageFlags? Flags { get; }

		internal DateTimeOffset CreatedAt { get; }

		internal ulong Id { get; }

		internal Task AddReactionAsync(IEmote emote, RequestOptions options = null)
			=> SourceSocketMessage.AddReactionAsync(emote, options);

		internal Task DeleteAsync(RequestOptions options = null)
			=> SourceSocketMessage.DeleteAsync(options);

		internal IAsyncEnumerable<IReadOnlyCollection<IUser>> GetReactionUsersAsync(IEmote emoji, int limit, RequestOptions options = null)
			=> SourceSocketMessage.GetReactionUsersAsync(emoji, limit, options);

		internal Task RemoveAllReactionsAsync(RequestOptions options = null)
			=> SourceSocketMessage.RemoveAllReactionsAsync(options);

		internal Task RemoveAllReactionsForEmoteAsync(IEmote emote, RequestOptions options = null)
			=> SourceSocketMessage.RemoveAllReactionsForEmoteAsync(emote, options);

		internal Task RemoveReactionAsync(IEmote emote, IUser user, RequestOptions options = null)
			=> SourceSocketMessage.RemoveReactionAsync(emote, user, options);

		internal Task RemoveReactionAsync(IEmote emote, ulong userId, RequestOptions options = null)
			=> SourceSocketMessage.RemoveReactionAsync(emote, userId, options);

		#endregion

		internal bool IsPrivate { get; }

		internal SocketGuild Guild { get; }

		internal SocketGuildUser Member { get; }

		internal MessageObject(SocketMessage Message) {
			SourceSocketMessage = Message;
			Activity = Message.Activity;
			Application = Message.Application;
			Attachments = Message.Attachments;
			Author = Message.Author;
			Channel = Message.Channel;
			Content = Message.Content;
			CreatedAt = Message.CreatedAt;
			EditedTimestamp = Message.EditedTimestamp;
			Embeds = Message.Embeds;
			Flags = Message.Flags;
			Id = Message.Id;
			IsPinned = Message.IsPinned;
			IsSuppressed = Message.IsSuppressed;
			IsTTS = Message.IsTTS;
			MentionedChannels = Message.MentionedChannels;
			MentionedEveryone = Message.MentionedEveryone;
			MentionedRoles = Message.MentionedRoles;
			MentionedUsers = Message.MentionedUsers;
			Reactions = Message.Reactions;
			Reference = Message.Reference;
			Source = Message.Source;
			Stickers = Message.Stickers;
			Tags = Message.Tags;
			Timestamp = Message.Timestamp;

			IsPrivate = !SourceSocketMessage.Reference.GuildId.IsSpecified;
			Guild = IsPrivate ? null : Client.Bot.GetGuild(SourceSocketMessage.Reference.GuildId.GetValueOrDefault());
			Member = Guild?.GetUser(Author.Id);
		}

		protected MessageObject(MessageObject SourceMessageObject) {
			SourceSocketMessage = SourceMessageObject.SourceSocketMessage;
			Activity = SourceMessageObject.Activity;
			Application = SourceMessageObject.Application;
			Attachments = SourceMessageObject.Attachments;
			Author = SourceMessageObject.Author;
			Channel = SourceMessageObject.Channel;
			Content = SourceMessageObject.Content;
			CreatedAt = SourceMessageObject.CreatedAt;
			EditedTimestamp = SourceMessageObject.EditedTimestamp;
			Embeds = SourceMessageObject.Embeds;
			Flags = SourceMessageObject.Flags;
			Id = SourceMessageObject.Id;
			IsPinned = SourceMessageObject.IsPinned;
			IsSuppressed = SourceMessageObject.IsSuppressed;
			IsTTS = SourceMessageObject.IsTTS;
			MentionedChannels = SourceMessageObject.MentionedChannels;
			MentionedEveryone = SourceMessageObject.MentionedEveryone;
			MentionedRoles = SourceMessageObject.MentionedRoles;
			MentionedUsers = SourceMessageObject.MentionedUsers;
			Reactions = SourceMessageObject.Reactions;
			Reference = SourceMessageObject.Reference;
			Source = SourceMessageObject.Source;
			Stickers = SourceMessageObject.Stickers;
			Tags = SourceMessageObject.Tags;
			Timestamp = SourceMessageObject.Timestamp;

			IsPrivate = SourceMessageObject.IsPrivate;
			Guild = SourceMessageObject.Guild;
			Member = SourceMessageObject.Member;
		}
	}
}
