using AvespoirTest.Core.Database;
using System.Threading.Tasks;

namespace AvespoirTest.Core {

	public class StartClient {
		public StartClient(string[] args) {
			ClientLog.InitlogFile().ConfigureAwait(false).GetAwaiter().GetResult();

			Task DBClient = Task.Factory.StartNew(() => MongoDBClient.Main());
			Task<Client> BotClient = Task.Factory.StartNew(() => new Client(args));
			Task.WaitAll(DBClient, BotClient);
		}
	}
}
