using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Exceptions;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static AvespoirTest.Core.Modules.Utils.EmojiConverter;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("$emoji")]
		public async Task Emoji(CommandContext Context) {
			try {
				string[] msgs = Context.Message.Content.Substring(CommandConfig.MainPrefix.Length + Context.Command.Name.Length).Trim().Split(" ");
				if (msgs[0] == "" || msgs[0] == null) throw new ArgumentNullException();
				string EmojiName = msgs[0];

				IEnumerator<DiscordAttachment> Attachment = Context.Message.Attachments.GetEnumerator();
				Uri ImageUrl;
				if (Attachment.MoveNext()) ImageUrl = new Uri(Attachment.Current.Url);
				else throw new UrlNotFoundException();

				WebClient GetImage = new WebClient();
				byte[] Imagebyte = await GetImage.DownloadDataTaskAsync(ImageUrl);
				Stream Image = new MemoryStream(Imagebyte);

				DiscordGuildEmoji Emoji = await Context.Guild.CreateEmojiAsync(EmojiName, Image);
				await Context.Channel.SendMessageAsync(ConvertEmoji(Emoji) + $"を{Emoji.Name}で登録しました");
			}
			catch (ArgumentNullException) {
				await Context.Channel.SendMessageAsync("名前が指定されていない、または空白です！");
			}
			catch (UrlNotFoundException) {
				await Context.Channel.SendMessageAsync("画像が指定されていません！");
			}
			catch (Exception Error) {
				new ErrorLog(Error.Message);
			}
		}
	}
}
