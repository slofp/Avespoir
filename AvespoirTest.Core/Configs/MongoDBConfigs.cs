using MongoDB.Driver;

namespace AvespoirTest.Core.Configs {

	class MongoDBConfigs {

		internal static string UseDatabase { get; set; } = "admin";
		internal static string Username { get; set; } = "";
		internal static string Password { get; set; } = "";
		internal static string Url { get; set; } = "localhost";
		internal static int Port { get; set; } = 27017;
		internal static string Mechanism { get; set; } = "SCRAM-SHA-256";

		MongoIdentity LoginUser = new MongoInternalIdentity(UseDatabase, Username);
		MongoIdentityEvidence UserPass = new PasswordEvidence(Password);

		MongoServerAddress ServerAddress = new MongoServerAddress(Url, Port);

		internal MongoDBConfigs() {
			ClientSettings = new MongoClientSettings {
				Server = ServerAddress,
				Credential = new MongoCredential(Mechanism, LoginUser, UserPass)
			};
		}

		internal MongoClientSettings ClientSettings;
	}
}
