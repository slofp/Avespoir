using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Avespoir.Core.Modules.Visualize {

	/// <summary>
	/// メッセージのビジュアルを作成します
	/// </summary>
	class VisualGenerator {

		private readonly List<DiscordEmbed> Embeds;

		private readonly List<DiscordComponent> Components;

		private readonly Dictionary<string, Stream> Files;

		internal string Content { get; set; }

		internal bool IsTTS { get; set; } = false;

		internal DiscordMessageSticker Sticker { get; set; }

		private bool SettedReply = false;

		private bool FailOnInvalidReply = false;

		private bool MentionReply = false;

		private ulong ReplyMessageId = 0;

		internal VisualGenerator() {
			Embeds = new List<DiscordEmbed>();
			Components = new List<DiscordComponent>();
			Files = new Dictionary<string, Stream>();
		}

		/// <summary>
		/// 設定を元に<see cref="DiscordMessageBuilder"/>を作成します
		/// </summary>
		/// <returns>設定済みの<see cref="DiscordMessageBuilder"/></returns>
		internal DiscordMessageBuilder Generate() {
			DiscordMessageBuilder MessageBuilder = new DiscordMessageBuilder();

			if (Embeds.Count != 0) MessageBuilder.AddEmbeds(Embeds);

			List<DiscordActionRowComponent> ActionRowComponents = new List<DiscordActionRowComponent>();
			for (int i = 0; i < Components.Count; i += 5) {
				ActionRowComponents.Add(
					new DiscordActionRowComponent(
						Components.GetRange(i, Components.Count - i < 5 ? Components.Count - i : 5)
					)
				);
			}

			if (ActionRowComponents.Count != 0) MessageBuilder.AddComponents(ActionRowComponents);

			if (Files.Count != 0) MessageBuilder.WithFiles(Files);

			if (SettedReply) MessageBuilder.WithReply(ReplyMessageId, MentionReply, FailOnInvalidReply);

			if (!(Sticker is null)) MessageBuilder.WithSticker(Sticker);

			// 今後実装する必要あり？
			// if () MessageBuilder.WithAllowedMentions();

			MessageBuilder.IsTTS = IsTTS;
			MessageBuilder.Content = Content;

			return MessageBuilder;
		}

		/// <summary>
		/// レガシーコマンド用Embedを作成します
		/// </summary>
		/// <remarks>タイムスタンプとフッターは事前に自動挿入されます</remarks>
		/// <param name="Text">テキスト</param>
		/// <param name="Color">色(デフォルトでノーマルカラー)、<seealso cref="Assets.EmbedColorAsset"/>に標準的な色があります</param>
		/// <param name="Custom">それ以外に設定したい項目がある場合にこれで指定できます</param>
		internal VisualGenerator AddEmbed(string Text, DiscordColor? Color = null, Action<DiscordEmbedBuilder> Custom = null) {
			DiscordColor col = Color is DiscordColor NNColor ? NNColor : Assets.EmbedColorAsset.NormalColor;

			DiscordEmbedBuilder EmbedBuilder =
				new DiscordEmbedBuilder()
				.WithDescription(Text)
				.WithColor(col)
				.WithTimestamp(DateTime.Now)
				.WithFooter(
					string.Format("{0} Bot", Client.Bot.CurrentUser.Username),
					Client.Bot.CurrentUser.GetAvatarUrl(ImageFormat.Auto)
				);

			if (!(Custom is null)) Custom(EmbedBuilder);

			Embeds.Add(EmbedBuilder.Build());

			return this;
		}

		/// <summary>
		/// 基本的なEmbedを作成します
		/// </summary>
		/// <remarks>タイムスタンプとフッターは事前に自動挿入されます</remarks>
		/// <param name="Title">タイトル</param>
		/// <param name="Description">説明</param>
		/// <param name="Color">色、<seealso cref="Assets.EmbedColorAsset"/>に標準的な色があります</param>
		/// <param name="Custom">それ以外に設定したい項目がある場合にこれで指定できます</param>
		internal VisualGenerator AddEmbed(string Title, string Description, DiscordColor Color, Action<DiscordEmbedBuilder> Custom = null) {
			DiscordEmbedBuilder EmbedBuilder =
				new DiscordEmbedBuilder()
				.WithTitle(Title)
				.WithDescription(Description)
				.WithColor(Color)
				.WithTimestamp(DateTime.Now)
				.WithFooter(
					string.Format("{0} Bot", Client.Bot.CurrentUser.Username),
					Client.Bot.CurrentUser.GetAvatarUrl(ImageFormat.Auto)
				);

			if (!(Custom is null)) Custom(EmbedBuilder);

			Embeds.Add(EmbedBuilder.Build());

			return this;
		}

		/// <summary>
		/// Embedを作成します
		/// </summary>
		/// <remarks>タイムスタンプとフッターは事前に自動挿入されます</remarks>
		/// <param name="Custom">カスタマイズ用Actionです</param>
		internal VisualGenerator AddEmbed(Action<DiscordEmbedBuilder> Custom) {
			DiscordEmbedBuilder EmbedBuilder =
				new DiscordEmbedBuilder()
				.WithTimestamp(DateTime.Now)
				.WithFooter(
					string.Format("{0} Bot", Client.Bot.CurrentUser.Username),
					Client.Bot.CurrentUser.GetAvatarUrl(ImageFormat.Auto)
				);

			Custom(EmbedBuilder);

			Embeds.Add(EmbedBuilder.Build());

			return this;
		}

		/// <summary>
		/// ボタンを追加します
		/// </summary>
		/// <param name="Style">ボタンの色</param>
		/// <param name="CustomID">コンポーネントイベント時に使う識別子</param>
		/// <param name="Text">ボタンに表示するテキスト</param>
		/// <param name="Disabled">ボタンを押させないか</param>
		/// <param name="Emoji">前後かに入れられる絵文字</param>
		/// <exception cref="IndexOutOfRangeException">コンポーネント数が25個超えた場合発生します</exception>
		internal VisualGenerator AddButton(ButtonStyle Style, string CustomID, string Text, bool Disabled = false, DiscordComponentEmoji Emoji = null) {
			if (Components.Count >= 25) throw new IndexOutOfRangeException("Cannot add more than 25 components.");
			Components.Add(new DiscordButtonComponent(Style, CustomID, Text, Disabled, Emoji));

			return this;
		}

		/// <summary>
		/// メニューを追加します
		/// </summary>
		/// <remarks>作成する際は<seealso cref="SelectMenuBuilder"/>を使用することをおすすめします</remarks>
		/// <param name="SelectMenu"></param>
		internal VisualGenerator AddSelectMenu(DiscordSelectComponent SelectMenu) {
			if (Components.Count >= 25) throw new IndexOutOfRangeException("Cannot add more than 25 components.");
			Components.Add(SelectMenu);

			return this;
		}

		/// <summary>
		/// アップロードするファイルを追加します
		/// </summary>
		/// <param name="FileName">ファイル名</param>
		/// <param name="FileStream">ファイルのストリーム</param>
		internal VisualGenerator AddFile(string FileName, Stream FileStream) {
			Files.Add(FileName, FileStream);

			return this;
		}

		/// <summary>
		/// リプライの設定をします
		/// </summary>
		/// <param name="ReplyMessageId">リプライ元のメッセージID</param>
		/// <param name="MentionReply">リプライ時にメンションをつけるか</param>
		/// <param name="FailOnInvalidReply">メッセージIDが見つからない場合例外を返すか</param>
		internal VisualGenerator SetReply(ulong ReplyMessageId, bool MentionReply = false, bool FailOnInvalidReply = false) {
			SettedReply = true;
			this.ReplyMessageId = ReplyMessageId;
			this.MentionReply = MentionReply;
			this.FailOnInvalidReply = FailOnInvalidReply;

			return this;
		}

		/// <summary>
		/// リプライの設定をリセットします
		/// </summary>
		internal VisualGenerator ResetReply() {
			SettedReply = false;
			ReplyMessageId = 0;
			MentionReply = false;
			FailOnInvalidReply = false;

			return this;
		}
	}
}
