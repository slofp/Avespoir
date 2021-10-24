using Avespoir.Core.Extends;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Logger {

	class MessageLog {

		// Debug only
		internal static Task Main(MessageObject Message_Object) {
			StringBuilder Message_Log = new StringBuilder("Message Log");

			if (Message_Object.IsPrivate) {
				Message_Log.AppendFormat("\n[Channel] Name: {0}, ID: {1}", Message_Object.Channel.Name ?? "Direct Message", Message_Object.Channel.Id);
				Message_Log.AppendFormat("\n[Author] Name: {0}#{1}, ID: {2}, Bot: {3}", Message_Object.Author.Username, Message_Object.Author.Discriminator, Message_Object.Author.Id, Message_Object.Author.IsBot);
				Message_Log.AppendFormat("\n[Message] Content: {0}, ID: {1}", Message_Object.Content, Message_Object.Id);
			}
			else {
				Message_Log.AppendFormat("\n[Guild] Name: {0}, ID: {1}", Message_Object.Guild.Name, Message_Object.Guild.Id);
				Message_Log.AppendFormat("\n[Channel] Name: {0}, ID: {1}", Message_Object.Channel.Name, Message_Object.Channel.Id);
				Message_Log.AppendFormat("\n[Author] Name: {0}#{1}, ID: {2}, Bot: {3}", Message_Object.Author.Username, Message_Object.Author.Discriminator, Message_Object.Author.Id, Message_Object.Author.IsBot);
				Message_Log.AppendFormat("\n[Message] Content: {0}, ID: {1}", Message_Object.Content, Message_Object.Id);

				if (Message_Object.MentionedUsers.Count > 0) {
					IEnumerator<DiscordUser> MentionedUsers = Message_Object.MentionedUsers.GetEnumerator();

					StringBuilder MentionedUsers_Log = new StringBuilder("\n[MentionedUsers] ");
					bool MentionedUser_Next = MentionedUsers.MoveNext();
					int UserCount = 0;
					while (MentionedUser_Next) {
						try {
							DiscordUser MentionedUser = MentionedUsers.Current;
							StringBuilder MentionedUser_Log = new StringBuilder().AppendFormat("{0}#{1}", MentionedUser.Username, MentionedUser.Discriminator);

							MentionedUser_Next = MentionedUsers.MoveNext();
							if (MentionedUser_Next) {
								MentionedUser_Log.Append(", ");
								UserCount++;
							}

							MentionedUsers_Log.Append(MentionedUser_Log);
						}
						catch (NullReferenceException) {
							Log.Warning(string.Format("MentionedUser {0} is null", UserCount));

							StringBuilder MentionedUser_Log = new StringBuilder("unknown-user");

							MentionedUser_Next = MentionedUsers.MoveNext();
							if (MentionedUser_Next) {
								MentionedUser_Log.Append(", ");
								UserCount++;
							}

							MentionedUsers_Log.Append(MentionedUser_Log);
						}
					}

					Message_Log.Append(MentionedUsers_Log);
				}

				if (Message_Object.MentionedRoles.Count > 0) {
					IEnumerator<DiscordRole> MentionedRoles = Message_Object.MentionedRoles.GetEnumerator();

					StringBuilder MentionedRoles_Log = new StringBuilder("\n[MentionedRoles] ");
					bool MentionedRole_Next = MentionedRoles.MoveNext();
					int RoleCount = 0;
					while (MentionedRole_Next) {
						try {
							DiscordRole MentionedRole = MentionedRoles.Current;
							StringBuilder MentionedRole_Log = new StringBuilder(MentionedRole.Name);

							MentionedRole_Next = MentionedRoles.MoveNext();
							if (MentionedRole_Next) {
								MentionedRole_Log.Append(", ");
								RoleCount++;
							}

							MentionedRoles_Log.Append(MentionedRole_Log);
						}
						catch (NullReferenceException) {
							Log.Warning(string.Format("MentionedRole {0} is null", RoleCount));

							StringBuilder MentionedRole_Log = new StringBuilder("deleted-role");

							MentionedRole_Next = MentionedRoles.MoveNext();
							if (MentionedRole_Next) {
								MentionedRole_Log.Append(", ");
								RoleCount++;
							}

							MentionedRoles_Log.Append(MentionedRole_Log);
						}

					}

					Message_Log.Append(MentionedRoles_Log);
				}

				if (Message_Object.MentionedChannels.Count > 0) {
					IEnumerator<DiscordChannel> MentionedChannels = Message_Object.MentionedChannels.GetEnumerator();

					StringBuilder MentionedChannels_Log = new StringBuilder("\n[MentionedChannels] ");
					bool MentionedChannel_Next = MentionedChannels.MoveNext();
					int ChannelCount = 0;
					while (MentionedChannel_Next) {
						try {
							DiscordChannel MentionedChannel = MentionedChannels.Current;
							StringBuilder MentionedChannel_Log = new StringBuilder(MentionedChannel.Name);

							MentionedChannel_Next = MentionedChannels.MoveNext();
							if (MentionedChannel_Next) {
								MentionedChannel_Log.Append(", ");
								ChannelCount++;
							}

							MentionedChannels_Log.Append(MentionedChannel_Log);
						}
						catch (NullReferenceException) {
							Log.Warning($"MentionedChannel {ChannelCount} is null");

							StringBuilder MentionedChannel_Log = new StringBuilder("deleted-channel");

							MentionedChannel_Next = MentionedChannels.MoveNext();
							if (MentionedChannel_Next) {
								MentionedChannel_Log.Append(", ");
								ChannelCount++;
							}

							MentionedChannels_Log.Append(MentionedChannel_Log);
						}
					}

					Message_Log.Append(MentionedChannels_Log);
				}
			}

			if (Message_Object.Attachments.Count > 0) {
				IEnumerator<DiscordAttachment> MessageAttachments = Message_Object.Attachments.GetEnumerator();

				StringBuilder MessageAttachments_Log = new StringBuilder("\n[MessageAttachments] ");
				bool MessageAttachment_Next = MessageAttachments.MoveNext();
				while (MessageAttachment_Next) {
					DiscordAttachment MessageAttachment = MessageAttachments.Current;
					StringBuilder MessageAttachment_Log = new StringBuilder().AppendFormat("Url: {0} ProxyUrl: {1}", MessageAttachment.Url, MessageAttachment.ProxyUrl);
					MessageAttachment_Next = MessageAttachments.MoveNext();
					if (MessageAttachment_Next) MessageAttachment_Log.Append(", ");

					MessageAttachments_Log.Append(MessageAttachment_Log);
				}

				Message_Log.Append(MessageAttachments_Log);
			}

			Log.Debug(Message_Log.ToString());

			return Task.CompletedTask;
		}
	}
}
