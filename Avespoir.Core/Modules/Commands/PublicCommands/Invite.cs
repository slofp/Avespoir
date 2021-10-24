using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.SocketGuildExtension;
using DSharpPlus.Exceptions;

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

				DiscordChannel DefaultChannel = Command_Object.Guild.GetDefaultChannel();
				Console.WriteLine(DefaultChannel.Name);
				DiscordInvite Invite = await DefaultChannel.CreateInviteAsync();

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

			try {
				DiscordChannel GetDiscordChannel = Command_Object.Guild.GetChannel(InviteID);
				await SendInvite(Command_Object, await GetDiscordChannel.CreateInviteAsync());
			}
			catch (ServerErrorException) {
				Log.Warning("Channel ID is not found");
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.InviteChannelIdNotFound);
			}
		}

		async Task SendInvite(CommandObject Command_Object, DiscordInvite Invite) {
			string InviteUrl = "https://discord.gg/" + Invite.Code;
			string Message = string.Format(Command_Object.Language.InviteResult, InviteUrl);
			await Command_Object.Channel.SendMessageAsync(Message);
		}
	}
}
