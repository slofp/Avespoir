using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class ClientErroredEvent {

		internal static Task Main(DiscordClient Bot, ClientErrorEventArgs ClientError) {
			string Errorstring = string.Format("EventName: {0}\nException: {1}", ClientError.EventName, ClientError.Exception);
			Log.Error(Errorstring);
			Log.Error(ClientError.EventName, ClientError.Exception);

			return Task.CompletedTask;
		}
	}
}
