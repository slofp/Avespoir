using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using Avespoir.Core.Modules.Visualize;
using Avespoir.Core.Modules.Assets;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("find", RoleLevel.Public)]
	class Find : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("ユーザーの情報を表示します") {
			{ Database.Enums.Language.en_US, "Show user info" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}find [ユーザーID]") {
			{ Database.Enums.Language.en_US, "{0}find [UserID]" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);

			VisualGenerator Visual = new VisualGenerator();
			if (msgs.Length == 0) {
				Visual.AddEmbed(Command_Object.Language.EmptyText, EmbedColorAsset.FailedColor);

				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
				return;
			}

			if (string.IsNullOrWhiteSpace(msgs[0])) {
				Visual.AddEmbed(Command_Object.Language.EmptyId, EmbedColorAsset.FailedColor);

				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
				return;
			}
			if (!ulong.TryParse(msgs[0], out ulong Userid)) {
				Visual.AddEmbed(Command_Object.Language.IdCouldntParse, EmbedColorAsset.FailedColor);

				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
				return;
			}

			DiscordMember FoundMember = await Command_Object.Guild.GetMemberAsync(Userid).ConfigureAwait(false);
			if (FoundMember is null) {
				Visual.AddEmbed(Command_Object.Language.FindNotFound, EmbedColorAsset.FailedColor);

				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
			}
			else {
				string ResultString = string.Format(Command_Object.Language.FindResult, FoundMember.Username + "#" + FoundMember.Discriminator, FoundMember.JoinedAt, Command_Object.Guild.OwnerId == FoundMember.Id ? "yes" : "no", FoundMember.GetAvatarUrl(ImageFormat.Auto ,1024));
				Visual.AddEmbed(ResultString, EmbedColorAsset.SuccessColor);
				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
			}
		}
	}
}
