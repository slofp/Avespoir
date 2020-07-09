using Avespoir.Core.Modules.Commands;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Avespoir.Core.Modules.Lunetrip {

	class ScriptMethods {

		private CommandObjects Command_Object;

		public ScriptMethods(CommandObjects CommandObject) {
			Command_Object = CommandObject;
		}

		public DiscordMessage Print(object Message, bool TTS = false, DiscordEmbed Embed = null) =>
			Command_Object.Channel.SendMessageAsync(Message.ToString(), TTS, Embed).ConfigureAwait(false).GetAwaiter().GetResult();

		#region GetCodeSender

		public DiscordUser GetCodeSender_Author() => Command_Object.Author;

		public DiscordChannel GetCodeSender_Channel() => Command_Object.Channel;

		public DiscordGuild GetCodeSender_Guild() => Command_Object.Guild;

		public DiscordMember GetCodeSender_Member() => Command_Object.Member;

		public DiscordMessage GetCodeSender_Message() => Command_Object.Message;

		#endregion

		#region User

		public string User_GetAvatarUrl(DiscordUser User, ImageFormat Format, ushort Size = 1024) => User.GetAvatarUrl(Format, Size);

		public void User_Unban(DiscordUser User, DiscordGuild Guild, string Reason = null) => User.UnbanAsync(Guild, Reason).ConfigureAwait(false).GetAwaiter().GetResult();

		#endregion

		#region Channel

		public DiscordMessage Channel_SendMessage(DiscordChannel Channel, object Message = null) =>
			Channel.SendMessageAsync(Message.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();

		public DiscordInvite Channel_CreateInvite(DiscordChannel Channel, int MaxAge = 86400, int MaxUses = 0, bool Temporary = false, bool Unique = false, string Reason = null) =>
			Channel.CreateInviteAsync(MaxAge, MaxUses, Temporary, Unique, Reason).ConfigureAwait(false).GetAwaiter().GetResult();

		#endregion

		#region Guild



		#endregion

		#region Member



		#endregion

		#region Message

		public void Message_Delete(DiscordMessage Message) =>
			Message.DeleteAsync().ConfigureAwait(false).GetAwaiter().GetResult();

		#endregion

		#region Property(Main Table Name: discord)

		public ImageFormat Discord_ImageFormat_Gif() => ImageFormat.Gif;

		public ImageFormat Discord_ImageFormat_Jpeg() => ImageFormat.Jpeg;

		public ImageFormat Discord_ImageFormat_Png() => ImageFormat.Png;

		public ImageFormat Discord_ImageFormat_Unknown() => ImageFormat.Unknown;

		public ImageFormat Discord_ImageFormat_WebP() => ImageFormat.WebP;

		#endregion

		#region Util



		#endregion
	}
}
