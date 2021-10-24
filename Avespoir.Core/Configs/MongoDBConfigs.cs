using MongoDB.Driver;

namespace Avespoir.Core.Configs {

	class MongoDBConfigs {

		internal static string ServerUrl { get; set; } = "localhost";

		internal static string Database { get; set; } = "Avespoir";

		internal static string User { get; set; } = "root";

		internal static string Password { get; set; } = "";

		internal static string Mechanism { get; set; } = "SCRAM-SHA-256";

		internal static int Port { get; set; } = 27017;

		private static MongoServerAddress MongoServer => new MongoServerAddress(ServerUrl, Port);

		private static MongoIdentity LoginUser => new MongoInternalIdentity(Database, User);

		private static MongoIdentityEvidence UserPass => new PasswordEvidence(Password);

		private static MongoCredential Credential => new MongoCredential(Mechanism, LoginUser, UserPass);

		internal static MongoClientSettings ClientSettings => new MongoClientSettings() {
			Server = MongoServer,
			Credential = Credential
		};
	}
}
