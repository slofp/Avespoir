using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Avespoir.Core.Extends {

	class MessageObject /*: IMessage*/ {

		internal MessageCreateEventArgs SourceMessageCreateEventArgs { get; }

		internal enum MessageSource {
			Bot,

			System,

			User,

			Webhook
		}

		//internal MessageType Type { get; }

		internal MessageSource Source { get; }

		internal bool IsTTS { get; }

		internal bool IsPinned { get; }

		internal bool MentionedEveryone { get; }

		internal bool Handled { get; }

		internal string Content { get; }

		internal DateTimeOffset Timestamp { get; }

		internal DateTimeOffset? EditedTimestamp { get; }

		internal DiscordChannel Channel { get; }

		internal DiscordUser Author { get; }

		internal IReadOnlyList<DiscordAttachment> Attachments { get; }

		internal IReadOnlyList<DiscordEmbed> Embeds { get; }

		internal IReadOnlyList<DiscordChannel> MentionedChannels { get; }

		internal IReadOnlyList<ulong> MentionedChannelIds => MentionedChannels.Select(x => x.Id).ToImmutableArray();

		internal IReadOnlyList<DiscordRole> MentionedRoles { get; }

		internal IReadOnlyList<ulong> MentionedRoleIds => MentionedRoles.Select(x => x.Id).ToImmutableArray();

		internal IReadOnlyList<DiscordUser> MentionedUsers { get; }

		internal IReadOnlyList<ulong> MentionedUserIds => MentionedUsers.Select(x => x.Id).ToImmutableArray();

		internal DiscordMessageActivity Activity { get; }

		internal DiscordMessageApplication Application { get; }

		internal DiscordMessageReference Reference { get; }

		internal IReadOnlyList<DiscordReaction> Reactions { get; }

		internal IReadOnlyList<DiscordMessageSticker> Stickers { get; }

		internal MessageFlags? Flags { get; }

		internal DateTimeOffset CreatedAt { get; }

		internal ulong Id { get; }

		internal Task CreateReactionAsync(DiscordEmoji Emoji)
			=> SourceMessageCreateEventArgs.Message.CreateReactionAsync(Emoji);

		internal Task DeleteAsync(string Reason = null)
			=> SourceMessageCreateEventArgs.Message.DeleteAsync(Reason);

		internal Task<IReadOnlyList<DiscordUser>> GetReactionsAsync(DiscordEmoji Emoji, int Limit, ulong? After = null)
			=> SourceMessageCreateEventArgs.Message.GetReactionsAsync(Emoji, Limit, After);

		internal Task RemoveAllReactionsAsync(string Reason = null)
			=> SourceMessageCreateEventArgs.Message.DeleteAllReactionsAsync(Reason);

		internal Task RemoveAllReactionsForEmoteAsync(DiscordEmoji Emoji)
			=> SourceMessageCreateEventArgs.Message.DeleteReactionsEmojiAsync(Emoji);

		internal Task RemoveReactionAsync(DiscordEmoji Emoji, DiscordUser User, string Reason = null)
			=> SourceMessageCreateEventArgs.Message.DeleteReactionAsync(Emoji, User, Reason);

		internal bool IsPrivate { get; }

		internal DiscordGuild Guild { get; }

		internal DiscordMember Member { get; }

		internal MessageObject(MessageCreateEventArgs Args) {
			SourceMessageCreateEventArgs = Args;
			Activity = Args.Message.Activity;
			Application = Args.Message.Application;
			Attachments = Args.Message.Attachments;
			Author = Args.Author;
			Channel = Args.Channel;
			Content = Args.Message.Content;
			CreatedAt = Args.Message.CreationTimestamp;
			EditedTimestamp = Args.Message.EditedTimestamp;
			Embeds = Args.Message.Embeds;
			Flags = Args.Message.Flags;
			Id = Args.Message.Id;
			IsPinned = Args.Message.Pinned;
			IsTTS = Args.Message.IsTTS;
			MentionedChannels = Args.MentionedChannels;
			MentionedEveryone = Args.Message.MentionEveryone;
			MentionedRoles = Args.MentionedRoles;
			MentionedUsers = Args.MentionedUsers;
			Reactions = Args.Message.Reactions;
			Reference = Args.Message.Reference;
			Stickers = Args.Message.Stickers;
			Timestamp = Args.Message.Timestamp;
			Handled = Args.Handled;


			if (Args.Message.Author.IsSystem is bool IsSystem && IsSystem)
				Source = MessageSource.System;
			else if (Args.Message.Author.IsBot is bool IsBot && IsBot)
				Source = MessageSource.Bot;
			else if (Args.Message.Author.IsCurrent is bool IsCurrent && IsCurrent)
				Source = MessageSource.System;
			else if (Args.Message.WebhookMessage is bool IsWebhook && IsWebhook)
				Source = MessageSource.Webhook;

			IsPrivate = Channel.IsPrivate;
			Guild = Args.Guild;
			Member = Args.Guild?.Members.Where(MemberData => MemberData.Key == Args.Author.Id).Select(MemberData => MemberData.Value).FirstOrDefault();
		}

		protected MessageObject(MessageObject SourceMessageObject) {
			SourceMessageCreateEventArgs = SourceMessageObject.SourceMessageCreateEventArgs;
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
			IsTTS = SourceMessageObject.IsTTS;
			MentionedChannels = SourceMessageObject.MentionedChannels;
			MentionedEveryone = SourceMessageObject.MentionedEveryone;
			MentionedRoles = SourceMessageObject.MentionedRoles;
			MentionedUsers = SourceMessageObject.MentionedUsers;
			Reactions = SourceMessageObject.Reactions;
			Reference = SourceMessageObject.Reference;
			Source = SourceMessageObject.Source;
			Stickers = SourceMessageObject.Stickers;
			Timestamp = SourceMessageObject.Timestamp;

			IsPrivate = SourceMessageObject.IsPrivate;
			Guild = SourceMessageObject.Guild;
			Member = SourceMessageObject.Member;
		}
	}
}
