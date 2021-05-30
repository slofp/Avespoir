using System;

namespace Avespoir.Core.JsonScheme {

	public class Language {

		// public string N { get; set; } = "";

		#region PublicCommands

		public string EmptyText { get; set; } = "何も入力されていません";

		public string EmptyName { get; set; } = "名前が空白またはNullです";

		public string EmojiSuccess { get; set; } = "{0}を{1}で登録しました";

		public string ImageNotFound { get; set; } = "画像が指定されていません！";

		public string EmptyId { get; set; } = "IDが空白またはNullです";

		public string IdCouldntParse { get; set; } = "IDは数字でなければいけません";

		public string FindResult { get; set; } = "ユーザー名: {0}\n登録日: {1}\nサーバー管理者: {2}\nアバターURL: {3}";

		public string FindNotFound { get; set; } = "見つかりませんでした！";

		public string InviteResult { get; set; } = "作成しました。24時間有効です\n{0}";

		public string InviteChannelNotFound { get; set; } = "チャンネルが不正です";

		public string InviteChannelIdNotFound { get; set; } = "チャンネルIDは存在しません";

		public string PingWait { get; set; } = "しばらくお待ち下さい...";

		public string RollMaxCouldntParse { get; set; } = "最大値が不適切か範囲外です";

		public string RollMinCouldntParse { get; set; } = "最小値が不適切か範囲外です";

		public string RollMaxMinCouldntParse { get; set; } = "最大値か最小値が不適切か範囲外です";

		public string StatusNotRegisted { get; set; } = "そのユーザーのステータスは登録されていません";

		public string StatusEmbed1 { get; set; } = "{0}のステータス";

		public string StatusEmbed2 { get; set; } = "名前: {0}\nユーザーID: {1}\n経験値: {2}\nレベル: Lv.{3}\n\n次のレベルまであと: {4}";

		public string StatusUserCouldntParse { get; set; } = "ユーザー指定が不正です";

		public string Version { get; set; } = "Bot名: {0}\nBotバージョン: {1}";

		public string DMMention { get; set; } = "{0} DMをご確認ください！";

		public string HelpPublicCommand { get; set; } = "一般コマンド";

		public string HelpCommandPrefix { get; set; } = "プレフィックスは {0} です";

		public string HelpModeratorCommand { get; set; } = "モデレーターコマンド";

		public string HelpConfigArgs { get; set; } = "Config 第一引数";
		#endregion

		#region ModeratorCommands

		public string ConfigEmptyValue { get; set; } = "{0}の値が入力されていません";

		public string ConfigWhitelistFalse { get; set; } = "ホワイトリストを無効にしました";

		public string ConfigWhitelistTrue { get; set; } = "ホワイトリストを有効にしました";

		public string ConfigLeaveBanFalse { get; set; } = "脱退時のBanを無効にしました";

		public string ConfigLeaveBanTrue { get; set; } = "脱退時のBanを有効にしました";

		public string ConfigPublicPrefixChange { get; set; } = "PublicPrefixを``{0}``から``{1}``に変更しました";

		public string ConfigModeratorPrefixChange { get; set; } = "ModeratorPrefixを``{0}``から``{1}``に変更しました";

		public string ConfigLogChannelIDSet { get; set; } = "LogChannelIDを``{0}``に設定しました";

		public string ConfigLogChannelIDChange { get; set; } = "LogChannelIDを``{0}``から``{1}``に変更しました";

		public string ConfigLanguageNotFound { get; set; } = "{0}は存在しませせん";

		public string ConfigLanguageSet { get; set; } = "言語を``{0}``に設定しました";

		public string ConfigLanguageChange { get; set; } = "言語を``{0}``から``{1}``に変更しました";

		public string ConfigLevelSwitchFalse { get; set; } = "レベリングを無効にしました";

		public string ConfigLevelSwitchTrue { get; set; } = "レベリングを有効にしました";

		public string ConfigArgsNotFound { get; set; } = "その項目はありません";

		public string EmptyRoleNumber { get; set; } = "RoleNumberが空白またはNullです";

		public string RoleNumberNotNumber { get; set; } = "RoleNumberは数字でなければいけません";

		public string EmptyRoleLevel { get; set; } = "RoleLevelが空白またはNullです";

		public string RoleLevelDenyText { get; set; } = "このRoleLevelに使用できない文字が含まれています: {0}";

		public string RoleLevelNotNumber { get; set; } = "RoleLevelは数字でなければいけません";

		public string RoleLevelNotFound { get; set; } = "RoleLevelが適切ではありません";

		public string RoleNumberRegisted { get; set; } = "そのRoleNumberはすでに登録されています";

		public string IdRegisted { get; set; } = "そのIDはすでに登録されています";

		public string AuthFailure { get; set; } = "認証に失敗しました、初めからやり直してください";

		public string DBRoleAddSuccess { get; set; } = "Roleデータベースに\nID: {0}({1})\nRoleNumber: {2}\nRoleLevel: {3}\nで登録しました！";

		public string TypingMissed { get; set; } = "必要箇所が入力されていません";

		public string IdNotRegisted { get; set; } = "そのIDは登録されていません";

		public string DBRoleDeleteSuccess { get; set; } = "名前: {0}\nID: {1}\nをRoleデータベースから削除しました";

		public string ListNothing { get; set; } = "何も登録されていません";

		public string NameRegisted { get; set; } = "その名前はすでに登録されています";

		public string DBUserAddSuccess { get; set; } = "Userデータベースに\n名前: {0}\nID: {1}\nRoleNumber: {2}({3})\nで登録しました！";

		public string RoleNumberNotFound { get; set; } = "RoleNumberがRoleデータベースに存在しません";

		public string DBUserChangeRoleSuccess { get; set; } = "{0}のRoleを{1}から{2}に変更しました！";

		public string DBUserChangeRoleEmbedTitle { get; set; } = "Roleが{0}に変更になりました！ {0}では次のことが許可されています！";

		public string BeforeRoleNotFound { get; set; } = "Roleデータベースに元のRoleが存在しません";

		public string KickReason { get; set; } = "{0}によってUserデータベースから削除されたため";

		public string DBUserDeleteSuccess { get; set; } = "名前: {0}\nID: {1}\nをUserデータベースから削除しました";
		#endregion

		#region Add, Remove, LevelSys

		public string IsBot { get; set; } = "Botです";

		public string Bot_BanDescription { get; set; } = "名前: {0}\nID: {1}";

		public string Accessed { get; set; } = "アクセス完了";

		public string JoinPass { get; set; } = "参加を許可します";

		public string UserDescription { get; set; } = "名前: {0}\nID: {1}\nRole: {2}";

		public string AccessPermission { get; set; } = "アクセス権の確認";

		public string RoleNumNullMessage { get; set; } = "申し訳ありません... サーバー管理者にお問い合わせください";

		public string WelcomeEmbedTitle { get; set; } = "Welcome to {0}! {1}では次のことが許可されています！";

		public string PermissionDenied { get; set; } = "あなたはアクセス権がありません、もしアクセス権があり入れない場合はサーバー管理者にお問い合わせください";

		public string NotAccessed { get; set; } = "アクセス権がありません";

		public string AccessDenied { get; set; } = "アクセス権がないため蹴られました";

		public string Baned { get; set; } = "BANされました";

		public string BotRemoved { get; set; } = "サーバーから削除しました";

		public string Leaved { get; set; } = "サーバーを抜けました";

		public string BanReason { get; set; } = "サーバーを抜けたため";

		public string DBDeleteLeave { get; set; } = "サーバーに登録されていないかデータベース上から削除されました";

		public string DisableLeave { get; set; } = "サーバーを抜けました";

		public string DMEmbed_Public1 { get; set; } = "サーバーの操作ができます";

		public string DMEmbed_Public2 { get; set; } = "ほぼ自由にサーバーを操作できます";

		public string DMEmbed_Public3 { get; set; } = "すべてのチャンネルを読んだり送信することができます";

		public string DMEmbed_Public4 { get; set; } = "これはあなたが認定された証です";

		public string DMEmbed_Moderator1 { get; set; } = "サーバーの操作ができます";

		public string DMEmbed_Moderator2 { get; set; } = "今日からあなたはここの管理者です";

		public string DMEmbed_Moderator3 { get; set; } = "データベースへの追加、変更、削除等が行えます";

		public string DMEmbed_Moderator4 { get; set; } = "{0}Botのすべてのコマンドを使うことができます";

		public string DMEmbed_Moderator5 { get; set; } = "すべてのチャンネルを読んだり送信することができます";

		public string DMEmbed_Moderator6 { get; set; } = "これはあなたが認定された証です";

		public string DMEmbed_LeaveBan1 { get; set; } = "抜けたらBANされます";

		public string DMEmbed_LeaveBan2 { get; set; } = "現在{0}は抜けたらBANされます";

		public string LevelUpEmbed1 { get; set; } = "レベルが上がりました！";

		public string LevelUpEmbed2 { get; set; } = "経験値: {0}\nレベル: Lv.{1} -> Lv.{2}";
		#endregion

		#region Others

		public string CommandNotImpl { get; set; } = "このコマンドは定義されていますが実装されていません。";

		#endregion
	}
}
