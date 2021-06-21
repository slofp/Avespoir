using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.SocketGuildExtension;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("invite", RoleLevel.Public)]
	class Invite : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("招待URLを作成します") {
			{ Database.Enums.Language.en_US, "Create invite URL" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}invite (チャンネル)") {
			{ Database.Enums.Language.en_US, "{0}invite (Channel)" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);

			if (msgs.Length == 0) {
				Log.Debug("Channel is null");

				SocketTextChannel DefaultChannel = Command_Object.Guild.DefaultChannel;
				Console.WriteLine(DefaultChannel.Name);
				IInviteMetadata Invite = await DefaultChannel.CreateInviteAsync();

				string InviteUrl = "https://discord.gg/" + Invite.Code;
				string Message = string.Format(Command_Object.Language.InviteResult, InviteUrl);
				await Command_Object.Channel.SendMessageAsync(Message);
				return;
			}

			string InviteText = msgs[0];
			string InviteIDString = InviteText.TrimStart('<', '#').TrimEnd('>');
			if (!ulong.TryParse(InviteIDString, out ulong InviteID)) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.InviteChannelNotFound);
				return;
			}

			Union<SocketTextChannel, SocketVoiceChannel> GetUnionChannel = Command_Object.Guild.GetTextOrVoiceChannel(InviteID); // Get方法がVoiceとTextで分かれているためそこの処理が必要だ

			switch (GetUnionChannel.CurrentType) {
				case Union<SocketTextChannel, SocketVoiceChannel>.BoxedType.T1:
					SocketTextChannel GetTextChannel = GetUnionChannel;
					await SendInvite(Command_Object, await GetTextChannel.CreateInviteAsync());
					break;
				case Union<SocketTextChannel, SocketVoiceChannel>.BoxedType.T2:
					SocketVoiceChannel GetVoiceChannel = GetUnionChannel;
					await SendInvite(Command_Object, await GetVoiceChannel.CreateInviteAsync());
					break;
				case Union<SocketTextChannel, SocketVoiceChannel>.BoxedType.OtherOrNull:
					Log.Warning("Channel ID is not found");
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.InviteChannelIdNotFound);
					break;
			}
		}

		async Task SendInvite(CommandObject Command_Object, IInviteMetadata InviteMetadata) {
			string InviteUrl = "https://discord.gg/" + InviteMetadata.Code;
			string Message = string.Format(Command_Object.Language.InviteResult, InviteUrl);
			await Command_Object.Channel.SendMessageAsync(Message);
		}
	}
}
