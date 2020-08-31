using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("find")]
		public async Task Find(CommandObjects CommandObject) {
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
