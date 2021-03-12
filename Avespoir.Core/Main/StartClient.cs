using Avespoir.Core.Database;
using System.Threading.Tasks;

namespace Avespoir.Core {

	public static class StartClient {
		public static async Task Start() {
			LiteDBClient.Main();
			//await MongoDBClient.Main().ConfigureAwait(false);
			await Client.Main().ConfigureAwait(false);
		}
	}
}
