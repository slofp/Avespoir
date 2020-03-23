using Avespoir.Core.Database;

namespace Avespoir.Core.Configs {

	public class GetDBConfigJson {

		#region Database Config

		public string Url {
			get {
				return MongoDBConfigs.Url;
			}
			set {
				MongoDBConfigs.Url = value;
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

		public string UseDatabase {
			get {
				return MongoDBConfigs.UseDatabase;
			}
			set {
				MongoDBConfigs.UseDatabase = value;
			}
		}

		public string Username {
			get {
				return MongoDBConfigs.Username;
			}
			set {
				MongoDBConfigs.Username = value;
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
		#endregion

		#region Database Options

		public string MainDatabase {
			get {
				return MongoDBClient.MainDatabase;
			}
			set {
				MongoDBClient.MainDatabase = value;
			}
		}
		#endregion
	}
}
