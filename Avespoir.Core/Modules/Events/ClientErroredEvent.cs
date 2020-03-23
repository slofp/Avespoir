using Avespoir.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class ClientErroredEvent {

		internal static Task Main(ClientErrorEventArgs ClientError) {
			string Errorstring = string.Format("EventName: {0}\nException: {1}", ClientError.EventName, ClientError.Exception);
			new ErrorLog(Errorstring);

			return Task.CompletedTask;
		}
	}
}
