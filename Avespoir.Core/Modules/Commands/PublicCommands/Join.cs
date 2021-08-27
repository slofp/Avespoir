using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Audio;
using Avespoir.Core.Modules.Utils;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	[Command(/*"join", RoleLevel.Public*/)]
	class Join : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("ボイスチャットに入ります") {
			{ Database.Enums.Language.en_US, "Join the voice chat" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}join") {
			{ Database.Enums.Language.en_US, "{0}join" }
		};

		internal bool IsEntryPlay { get; set; } = false;

		internal VCInfo ResultVCInfo { get; private set; }

		internal override async Task Execute(CommandObject Command_Object) {
			if (Client.ConnectedVoiceChannel_Dict.TryGetValue(Command_Object.Guild.Id, out VCInfo VC_Info)) {
				//await Command_Object.Channel.SendMessageAsync("すでにVCに入っているため移動します");
				ResultVCInfo = VC_Info;
			}

			string[] msgs = Command_Object.CommandArgs.Remove(0, IsEntryPlay ? 2 : default);
			if (msgs.Length == 0) {
				if (Command_Object.Member.VoiceChannel is null) {
					if (ResultVCInfo is null) await Command_Object.Channel.SendMessageAsync("VCに入っていないため実行することができません");
					return;
				}

				if (ResultVCInfo is null) {
					ResultVCInfo = await VCInfo.Create(Command_Object.Member.VoiceChannel).ConfigureAwait(false);
					_ = Task.Run(() => ResultVCInfo.Start().ConfigureAwait(false)).ConfigureAwait(false);
				}
				else if (ResultVCInfo.VoiceChannel.Id != Command_Object.Member.VoiceChannel.Id)
					await ResultVCInfo.UpdateVoiceChannel(Command_Object.Member.VoiceChannel);
			}
			else if (msgs.Length >= 1) {
				string VoiceIDString = msgs[0].TrimStart('<', '#').TrimEnd('>');

				if (!ulong.TryParse(VoiceIDString, out ulong VoiceID)) {
					if (Command_Object.Member.VoiceChannel is null) {
						if (ResultVCInfo is null) await Command_Object.Channel.SendMessageAsync("VCに入っていないため実行することができません");
						return;
					}

					VoiceID = Command_Object.Member.VoiceChannel.Id;
				}

				SocketVoiceChannel VoiceChannel =
					VoiceID == Command_Object.Member.VoiceChannel?.Id ?
						Command_Object.Member.VoiceChannel : Command_Object.Guild.GetVoiceChannel(VoiceID);

				if (VoiceChannel is null) {
					if (ResultVCInfo is null) await Command_Object.Channel.SendMessageAsync("VCチャンネルが見つかりませんでした");
					return;
				}

				if (ResultVCInfo is null) {
					ResultVCInfo = await VCInfo.Create(VoiceChannel).ConfigureAwait(false);
					_ = Task.Run(() => ResultVCInfo.Start().ConfigureAwait(false)).ConfigureAwait(false);
				}
				else if (ResultVCInfo.VoiceChannel.Id != VoiceChannel.Id)
					await ResultVCInfo.UpdateVoiceChannel(VoiceChannel);
			}
		}
	}
}
