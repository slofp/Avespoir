using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using Discord.WebSocket;
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

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyText);
				return;
			}

			if (string.IsNullOrWhiteSpace(msgs[0])) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyId);
				return;
			}
			if (!ulong.TryParse(msgs[0], out ulong Userid)) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdCouldntParse);
				return;
			}

			SocketGuildUser FoundMember = Command_Object.Guild.GetUser(Userid);
			if (FoundMember is null) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.FindNotFound);
			}
			else {
				string ResultString = string.Format(Command_Object.Language.FindResult, FoundMember.Username + "#" + FoundMember.Discriminator, FoundMember.JoinedAt, Command_Object.Guild.OwnerId == FoundMember.Id ? "yes" : "no", FoundMember.GetAvatarUrl(size: 1024));
				await Command_Object.Channel.SendMessageAsync(ResultString);
			}
		}
	}
}
