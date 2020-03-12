using AvespoirTest.Core.Database;
using System.Threading.Tasks;

namespace AvespoirTest.Core {

	public class StartClient {
		public StartClient(string[] args) {
			ClientLog.InitlogFile().ConfigureAwait(false).GetAwaiter().GetResult();

			MongoDBClient.Main().ConfigureAwait(false).GetAwaiter().GetResult();
			Client.Main(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}
	}
}
