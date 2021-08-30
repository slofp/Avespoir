namespace Avespoir.Core.Database.Enums {

	internal enum RoleLevel {

		Public = 0,

		Moderator = 1,

		Bot = 2,

		// 隠蔽します
		Owner = int.MinValue
	}
}