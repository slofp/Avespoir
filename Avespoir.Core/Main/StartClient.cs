using Avespoir.Core.Database;
using Avespoir.Core.Main;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core {

	public static class StartClient {
		public static async Task Start() {
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			await UpdateChecker.GetUpdate().ConfigureAwait(false);

			MongoDBClient.Main();
			//await MongoDBClient.Main().ConfigureAwait(false);
			Client.VoiceInit();
			await Client.Main().ConfigureAwait(false);
		}
	}
}
