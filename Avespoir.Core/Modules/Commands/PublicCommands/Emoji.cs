using Avespoir.Core.Attributes;
using Avespoir.Core.Exceptions;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.EmojiConverter;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("emoji")]
		public async Task Emoji(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
				return;
			}

			if (string.IsNullOrWhiteSpace(msgs[0])) {
				await CommandObject.Message.Channel.SendMessageAsync("名前が空白またはNullです");
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
				await CommandObject.Channel.SendMessageAsync(ConvertEmoji(Emoji) + $"を{Emoji.Name}で登録しました");
			}
			catch (UrlNotFoundException) {
				await CommandObject.Channel.SendMessageAsync("画像が指定されていません！");
			}
		}
	}
}
