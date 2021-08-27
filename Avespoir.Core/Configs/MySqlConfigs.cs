namespace Avespoir.Core.Configs {

	class MySqlConfigs {

		internal static string Server { get; set; } = "localhost";

		internal static string Database { get; set; } = "Avespoir";

		internal static string User { get; set; } = "root";

		internal static string Password { get; set; } = "";

		internal static string OtherOptions { get; set; } = "";

		internal static string ConnectionString => string.Format("Server={0};Database={1};Uid={2};Pwd={3};{4}", Server, Database, User, Password, OtherOptions);

	}
}
