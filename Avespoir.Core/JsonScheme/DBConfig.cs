using Avespoir.Core.Configs;

namespace Avespoir.Core.JsonScheme {

	public class DBConfig {

		#region Database Config

		public string Server {
			get {
				return MySqlConfigs.Server;
			}
			set {
				MySqlConfigs.Server = value;
			}
		}

		public string Database {
			get {
				return MySqlConfigs.Database;
			}
			set {
				MySqlConfigs.Database = value;
			}
		}

		public string User {
			get {
				return MySqlConfigs.User;
			}
			set {
				MySqlConfigs.User = value;
			}
		}

		public string Password {
			get {
				return MySqlConfigs.Password;
			}
			set {
				MySqlConfigs.Password = value;
			}
		}

		public string OtherOptions {
			get {
				return MySqlConfigs.OtherOptions;
			}
			set {
				MySqlConfigs.OtherOptions = value;
			}
		}

		#endregion
	}
}
