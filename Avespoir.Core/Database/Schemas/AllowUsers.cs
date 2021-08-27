using LinqToDB.Mapping;

namespace Avespoir.Core.Database.Schemas {

	public class AllowUsers {

		[PrimaryKey, Identity]
		public int Id { get; set; }

		[Column("Guildid"), NotNull]
		public ulong GuildID { get; set; }

		[Column("Name")]
		public string Name { get; set; }

		[Column("uuid"), NotNull]
		public ulong Uuid { get; set; }

		[Column("RoleNum"), NotNull]
		public uint RoleNum { get; set; }
	}
}
