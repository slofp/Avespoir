using Avespoir.Core.Database;
using System.Threading.Tasks;

namespace Avespoir.Core {

	public class StartClient {
		public StartClient() {
			ClientLog.InitlogFile().ConfigureAwait(false).GetAwaiter().GetResult();

			MongoDBClient.Main().ConfigureAwait(false).GetAwaiter().GetResult();
			Client.Main().ConfigureAwait(false).GetAwaiter().GetResult();
		}
	}
}
