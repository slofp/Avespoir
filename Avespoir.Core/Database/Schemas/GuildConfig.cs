using LiteDB;

namespace Avespoir.Core.Database.Schemas {

	class GuildConfig {

		[BsonId]
		public ObjectId Id { get; set; }

		[BsonField("Guildid")]
		public ulong GuildID { get; set; }

		[BsonField("WhiteList")]
		public bool WhiteList { get; set; }

		[BsonField("LeaveBan")]
		public bool LeaveBan { get; set; }

		[BsonField("PublicPrefix")]
		public string PublicPrefix { get; set; }

		[BsonField("ModeratorPrefix")]
		public string ModeratorPrefix { get; set; }

		[BsonField("LogChannelId")]
		public ulong LogChannelId { get; set; }

		[BsonField("Language")]
		public string Language { get; set; }

		[BsonField("LevelSwitch")]
		public bool LevelSwitch { get; set; }
	}
}
