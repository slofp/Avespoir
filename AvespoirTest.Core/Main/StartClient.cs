using AvespoirTest.Core.Database;
using System.Threading.Tasks;

namespace AvespoirTest.Core {

	public class StartClient {

		public StartClient(string[] args) {
			ClientLog.InitlogFile().Wait();
			Task<MongoDBClient> DBClient = Task.Run(() => new MongoDBClient());
			Task<Client> BotClient = Task.Run(() => new Client(args));
			BotClient.Wait();
			DBClient.Wait();
		}
	}
}
