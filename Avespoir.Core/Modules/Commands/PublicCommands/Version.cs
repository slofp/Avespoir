using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("ver", RoleLevel.Public)]
	class Version : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			string VersionString = string.Format(CommandObject.Language.Version, Client.Bot.CurrentUser.Username, Client.Version);
			await CommandObject.Message.Channel.SendMessageAsync(VersionString);
		}
	}
}
