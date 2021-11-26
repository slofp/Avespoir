using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using Avespoir.Core.Modules.Voice;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("join", RoleLevel.Public)]
	class Join : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override Task Execute(CommandObject Command_Object)
			=> JoinExecute(Command_Object);

		internal static async Task<bool> JoinExecute(CommandObject Command_Object) {
			VoiceNextConnection Connection = Client.Bot.GetVoiceNext().GetConnection(Command_Object.Guild);
			if (!(Connection is null)) {
				await Command_Object.Channel.SendMessageAsync("すでに入っています").ConfigureAwait(false);
				return false;
			}

			DiscordChannel VoiceChannel = Command_Object.Member.VoiceState?.Channel;

			if (VoiceChannel is null) {
				await Command_Object.Channel.SendMessageAsync("入ってません").ConfigureAwait(false);

				return false;
			}

			return await JoinVC(VoiceChannel, Command_Object.Guild.Id);
		}

		internal static async Task<bool> JoinVC(DiscordChannel VoiceChannel, ulong GuildID) {
			try {
				await VoiceChannel.ConnectAsync();
				new VoiceStatus(GuildID, VoiceChannel.Id);
				return true;
			}
			catch (Exception error) {
				Log.Error(error);
				return false;
			}
		}
	}
}
