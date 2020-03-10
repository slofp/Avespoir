using AvespoirTest.Core.Attributes;
using System.Threading.Tasks;
using static AvespoirTest.Core.Modules.Utils.RandomCodeGenerator;

namespace AvespoirTest.Core.Modules.Commands {

	partial class BotownerCommands {

		#if DEBUG
		[Command("test")]
		public async Task Test(CommandObjects CommandObject) {
			await CommandObject.Channel.SendMessageAsync("send BotownerCommand");
			await CommandObject.Channel.SendMessageAsync(RandomCodeGenerate());
		}
		#endif
	}
}
