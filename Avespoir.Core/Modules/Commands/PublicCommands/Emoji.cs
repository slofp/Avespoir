using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Exceptions;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.EmojiConverter;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("emoji", RoleLevel.Public)]
	class Emoji : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("画像をもとに絵文字を作成します") {
			{ Database.Enums.Language.en_US, "Create a picture based on an image" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}emoji [名前] (画像アップロード)") {
			{ Database.Enums.Language.en_US, "{0}emoji [Name] (Upload a picture)" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
				return;
			}

			if (string.IsNullOrWhiteSpace(msgs[0])) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyName);
				return;
			}
			string EmojiName = msgs[0];

			try {
				IEnumerator<DiscordAttachment> Attachment = CommandObject.Message.Attachments.GetEnumerator();
				Uri ImageUrl;
				if (Attachment.MoveNext()) ImageUrl = new Uri(Attachment.Current.Url);
				else throw new UrlNotFoundException();

				WebClient GetImage = new WebClient();
				byte[] Imagebyte = await GetImage.DownloadDataTaskAsync(ImageUrl);
				Stream Image = new MemoryStream(Imagebyte);

				DiscordGuildEmoji Emoji = await CommandObject.Guild.CreateEmojiAsync(EmojiName, Image);
				await CommandObject.Channel.SendMessageAsync(string.Format(CommandObject.Language.EmojiSuccess, ConvertEmoji(Emoji), Emoji.Name));
			}
			catch (UrlNotFoundException) {
				await CommandObject.Channel.SendMessageAsync(CommandObject.Language.ImageNotFound);
			}
		}
	}
}
