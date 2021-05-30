using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("invite", RoleLevel.Public)]
	class Invite : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("招待URLを作成します") {
			{ Database.Enums.Language.en_US, "Create invite URL" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}invite (チャンネル)") {
			{ Database.Enums.Language.en_US, "{0}invite (Channel)" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);

			if (msgs.Length == 0) {
				Log.Debug("Channel is null");

				DiscordChannel DefaultChannel = CommandObject.Guild.GetDefaultChannel();
				Console.WriteLine(DefaultChannel.Name);
				DiscordInvite Invite = await DefaultChannel.CreateInviteAsync();

				string InviteUrl = "https://discord.gg/" + Invite.Code;
				string Message = string.Format(CommandObject.Language.InviteResult, InviteUrl);
				await CommandObject.Message.Channel.SendMessageAsync(Message);
				return;
			}

			string InviteText = msgs[0];
			string InviteIDString = InviteText.TrimStart('<', '#').TrimEnd('>');
			if (!ulong.TryParse(InviteIDString, out ulong InviteID)) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.InviteChannelNotFound);
				return;
			}

			try {
				DiscordChannel GetChannel = CommandObject.Guild.GetChannel(InviteID);
				DiscordInvite Invite = await GetChannel.CreateInviteAsync();

				string InviteUrl = "https://discord.gg/" + Invite.Code;
				string Message = string.Format(CommandObject.Language.InviteResult, InviteUrl);
				await CommandObject.Message.Channel.SendMessageAsync(Message);
			}
			catch (NullReferenceException) {
				Log.Warning("Channel ID is not found");
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.InviteChannelIdNotFound);
			}
		}
	}
}
