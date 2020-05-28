using Avespoir.Core.Database;
using System.Threading.Tasks;

namespace Avespoir.Core {

	public class StartClient {
		public StartClient() {
			MongoDBClient.Main().ConfigureAwait(false).GetAwaiter().GetResult();
			Client.Main().ConfigureAwait(false).GetAwaiter().GetResult();
		}
	}
}
