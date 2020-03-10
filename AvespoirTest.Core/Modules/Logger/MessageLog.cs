using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Collections.Generic;

namespace AvespoirTest.Core.Modules.Logger {

	class MessageLog {

		string Message_Log = "Message Log";
		
		// Debug only
		internal MessageLog(MessageCreateEventArgs Message_Objects) {
			if (Message_Objects.Channel.IsPrivate) {
				string ChannelName = Message_Objects.Channel.Name != null ? Message_Objects.Channel.Name : "Direct Message";
				Message_Log +=
						$"\n[Channel] Name: {ChannelName}, ID: {Message_Objects.Channel.Id}" +
						$"\n[Author] Name: {Message_Objects.Author.Username}#{Message_Objects.Author.Discriminator}, ID: {Message_Objects.Author.Id}, Bot: {Message_Objects.Author.IsBot}" +
						$"\n[Message] Content: {Message_Objects.Message.Content}, ID: {Message_Objects.Message.Id}";
			}
			else
				Message_Log +=
						$"\n[Guild] Name: {Message_Objects.Guild.Name}, ID: {Message_Objects.Guild.Id}" +
						$"\n[Channel] Name: {Message_Objects.Channel.Name}, ID: {Message_Objects.Channel.Id}" +
						$"\n[Author] Name: {Message_Objects.Author.Username}#{Message_Objects.Author.Discriminator}, ID: {Message_Objects.Author.Id}, Bot: {Message_Objects.Author.IsBot}" +
						$"\n[Message] Content: {Message_Objects.Message.Content}, ID: {Message_Objects.Message.Id}";

			if (!Message_Objects.Channel.IsPrivate) {
				if (Message_Objects.Message.MentionedUsers.Count > 0) {
					IEnumerator<DiscordUser> MentionedUsers = Message_Objects.Message.MentionedUsers.GetEnumerator();

					string MentionedUsers_Log = "\n[MentionedUsers] ";
					bool MentionedUser_Next = MentionedUsers.MoveNext();
					while (MentionedUser_Next) {
						DiscordUser MentionedUser = MentionedUsers.Current;
						string MentionedUser_Log = $"{MentionedUser.Username}#{MentionedUser.Discriminator}";

						MentionedUser_Next = MentionedUsers.MoveNext();
						if (MentionedUser_Next) MentionedUser_Log += ", ";

						MentionedUsers_Log += MentionedUser_Log;
					}

					Message_Log += MentionedUsers_Log;
				}

				if (Message_Objects.Message.MentionedRoles.Count > 0) {
					IEnumerator<DiscordRole> MentionedRoles = Message_Objects.Message.MentionedRoles.GetEnumerator();

					string MentionedRoles_Log = "\n[MentionedRoles] ";
					bool MentionedRole_Next = MentionedRoles.MoveNext();
					while (MentionedRole_Next) {
						DiscordRole MentionedRole = MentionedRoles.Current;
						string MentionedRole_Log = $"{MentionedRole.Name}";

						MentionedRole_Next = MentionedRoles.MoveNext();
						if (MentionedRole_Next) MentionedRole_Log += ", ";

						MentionedRoles_Log += MentionedRole_Log;
					}

					Message_Log += MentionedRoles_Log;
				}

				if (Message_Objects.Message.MentionedChannels.Count > 0) {
					IEnumerator<DiscordChannel> MentionedChannels = Message_Objects.Message.MentionedChannels.GetEnumerator();

					string MentionedChannels_Log = "\n[MentionedChannels] ";
					bool MentionedChannel_Next = MentionedChannels.MoveNext();
					while (MentionedChannel_Next) {
						DiscordChannel MentionedChannel = MentionedChannels.Current;
						string MentionedChannel_Log = $"{MentionedChannel.Name}";

						MentionedChannel_Next = MentionedChannels.MoveNext();
						if (MentionedChannel_Next) MentionedChannel_Log += ", ";

						MentionedChannels_Log += MentionedChannel_Log;
					}

					Message_Log += MentionedChannels_Log;
				}
			}

			if (Message_Objects.Message.Attachments.Count > 0) {
				IEnumerator<DiscordAttachment> MessageAttachments = Message_Objects.Message.Attachments.GetEnumerator();

				string MessageAttachments_Log = "\n[MessageAttachments] ";
				bool MessageAttachment_Next = MessageAttachments.MoveNext();
				while (MessageAttachment_Next) {
					DiscordAttachment MessageAttachment = MessageAttachments.Current;
					string MessageAttachment_Log = $"Url: {MessageAttachment.Url} ProxyUrl: {MessageAttachment.ProxyUrl}";
					MessageAttachment_Next = MessageAttachments.MoveNext();
					if (MessageAttachment_Next) MessageAttachment_Log += ", ";

					MessageAttachments_Log += MessageAttachment_Log;
				}

				Message_Log += MessageAttachments_Log;
			}

			new DebugLog(Message_Log);
		}
	}
}
