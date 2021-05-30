using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("ver", RoleLevel.Public)]
	class Version : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Botのバージョンを表示します") {
			{ Database.Enums.Language.en_US, "Show bot version" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}ver") {
			{ Database.Enums.Language.en_US, "{0}ver" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			string VersionString = string.Format(CommandObject.Language.Version, Client.Bot.CurrentUser.Username, Client.Version);
			await CommandObject.Message.Channel.SendMessageAsync(VersionString);
		}
	}
}
