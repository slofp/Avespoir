using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("find", RoleLevel.Public)]
	class Find : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("ユーザーの情報を表示します") {
			{ Database.Enums.Language.en_US, "Show user info" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}find [ユーザーID]") {
			{ Database.Enums.Language.en_US, "{0}find [UserID]" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
				return;
			}

			if (string.IsNullOrWhiteSpace(msgs[0])) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyId);
				return;
			}
			if (!ulong.TryParse(msgs[0], out ulong Userid)) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
				return;
			}

			try {
				DiscordMember FoundMember = await CommandObject.Guild.GetMemberAsync(Userid);
				string ResultString = string.Format(CommandObject.Language.FindResult, FoundMember.Username + "#" + FoundMember.Discriminator, FoundMember.JoinedAt, FoundMember.IsOwner ? "yes" : "no", FoundMember.AvatarUrl);
				await CommandObject.Channel.SendMessageAsync(ResultString);
			}
			catch (NotFoundException) {
				await CommandObject.Channel.SendMessageAsync(CommandObject.Language.FindNotFound);
			}
		}
	}
}
