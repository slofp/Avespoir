using LinqToDB.Mapping;

namespace Avespoir.Core.Database.Schemas {

	public class GuildConfig {

		[PrimaryKey, Identity]
		public int Id { get; set; }

		[Column("Guildid"), NotNull]
		public ulong GuildID { get; set; }

		[Column("WhiteList")]
		public bool WhiteList { get; set; }

		[Column("LeaveBan")]
		public bool LeaveBan { get; set; }

		[Column("Prefix")]
		public string Prefix { get; set; }

		[Column("PublicPrefix")]
		public string PublicPrefix { get; set; }

		[Column("ModeratorPrefix")]
		public string ModeratorPrefix { get; set; }

		[Column("LogChannelId")]
		public ulong LogChannelId { get; set; }

		[Column("Language")]
		public string Language { get; set; }

		[Column("LevelSwitch")]
		public bool LevelSwitch { get; set; }
	}
}
