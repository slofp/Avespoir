using Avespoir.Core.Database;
using Avespoir.Core.Main;
using System.Threading.Tasks;

namespace Avespoir.Core {

	public static class StartClient {
		public static async Task Start() {
			await UpdateChecker.GetUpdate().ConfigureAwait(false);

			MySqlClient.Main();
			//await MongoDBClient.Main().ConfigureAwait(false);
			await Client.Main().ConfigureAwait(false);
		}
	}
}
