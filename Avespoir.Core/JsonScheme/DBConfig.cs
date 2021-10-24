using Avespoir.Core.Configs;

namespace Avespoir.Core.JsonScheme {

	public class DBConfig {

		#region Database Config

		public string ServerUrl {
			get {
				return MongoDBConfigs.ServerUrl;
			}
			set {
				MongoDBConfigs.ServerUrl = value;
			}
		}

		public string Database {
			get {
				return MongoDBConfigs.Database;
			}
			set {
				MongoDBConfigs.Database = value;
			}
		}

		public string User {
			get {
				return MongoDBConfigs.User;
			}
			set {
				MongoDBConfigs.User = value;
			}
		}

		public string Password {
			get {
				return MongoDBConfigs.Password;
			}
			set {
				MongoDBConfigs.Password = value;
			}
		}

		public string Mechanism {
			get {
				return MongoDBConfigs.Mechanism;
			}
			set {
				MongoDBConfigs.Mechanism = value;
			}
		}

		public int Port {
			get {
				return MongoDBConfigs.Port;
			}
			set {
				MongoDBConfigs.Port = value;
			}
		}

		#endregion
	}
}
