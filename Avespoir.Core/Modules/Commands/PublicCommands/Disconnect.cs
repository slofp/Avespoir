using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Audio;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	[Command(/*"disconnect", RoleLevel.Public*/)]
	class Disconnect : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("ボイスチャットから切断します") {
			{ Database.Enums.Language.en_US, "Disconnect from voice chat" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}disconnect") {
			{ Database.Enums.Language.en_US, "{0}disconnect" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			if (!Client.ConnectedVoiceChannel_Dict.TryGetValue(Command_Object.Guild.Id, out VCInfo VC_Info)) {
				await Command_Object.Channel.SendMessageAsync("VCに入っていません");
				return;
			}

			await VC_Info.Finalize();
		}
	}
}
