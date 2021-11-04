using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Exceptions;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.EmojiConverter;
using Avespoir.Core.Modules.Visualize;
using Avespoir.Core.Modules.Assets;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("emoji", RoleLevel.Public)]
	class Emoji : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("画像をもとに絵文字を作成します") {
			{ Database.Enums.Language.en_US, "Create a picture based on an image" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}emoji [名前] (画像アップロード)") {
			{ Database.Enums.Language.en_US, "{0}emoji [Name] (Upload a picture)" }
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
				Visual.AddEmbed(Command_Object.Language.EmptyName, EmbedColorAsset.FailedColor);

				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
				return;
			}
			string EmojiName = msgs[0];

			try {
				IEnumerator<DiscordAttachment> Attachment = Command_Object.Attachments.GetEnumerator();
				Uri ImageUrl;
				if (Attachment.MoveNext()) ImageUrl = new Uri(Attachment.Current.Url);
				else throw new UrlNotFoundException();

				WebClient GetImage = new WebClient();
				byte[] Imagebyte = await GetImage.DownloadDataTaskAsync(ImageUrl);
				Stream EmojiImage = new MemoryStream(Imagebyte);

				DiscordGuildEmoji Emoji = await Command_Object.Guild.CreateEmojiAsync(EmojiName, EmojiImage).ConfigureAwait(false);

				Visual.AddEmbed(string.Format(Command_Object.Language.EmojiSuccess, ConvertEmoji(Emoji), Emoji.Name), EmbedColorAsset.SuccessColor);
				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
			}
			catch (UrlNotFoundException) {
				Visual.AddEmbed(Command_Object.Language.ImageNotFound, EmbedColorAsset.FailedColor);

				await Command_Object.Channel.SendMessageAsync(Visual.Generate());
			}
		}
	}
}
