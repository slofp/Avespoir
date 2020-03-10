using AvespoirTest.Core.Attributes;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {
		#if DEBUG
		[Command("test")]
		public async Task Test(CommandObjects CommandObject) {
			await CommandObject.Channel.SendMessageAsync("send PublicCommand");
		}
		#endif
	}
}
