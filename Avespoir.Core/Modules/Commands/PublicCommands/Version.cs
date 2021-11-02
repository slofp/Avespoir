using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Main;
using Avespoir.Core.Modules.Assets;
using Avespoir.Core.Modules.Visualize;
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

		internal override async Task Execute(CommandObject Command_Object) {
			string VersionString = string.Format(Command_Object.Language.Version, Client.Bot.CurrentUser.Username, VersionInfo.Version);
			VisualGenerator Visual = new VisualGenerator();
			Visual.AddEmbed("", VersionString, EmbedColorAsset.NormalColor);
			await Command_Object.Channel.SendMessageAsync(Visual.Generate());
		}
	}
}
