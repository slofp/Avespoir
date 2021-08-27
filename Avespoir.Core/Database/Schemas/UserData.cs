using LinqToDB.Mapping;

namespace Avespoir.Core.Database.Schemas {

	[Table(Name = "UserData")]
	public class UserData {

		[PrimaryKey, Identity]
		public int Id { get; set; }

		[Column("User_id"), NotNull]
		public ulong UserID { get; set; }

		[Column("Level")]
		public uint Level { get; set; }

		[Column("Experience-point")]
		public double ExperiencePoint { get; set; }
	}
}
