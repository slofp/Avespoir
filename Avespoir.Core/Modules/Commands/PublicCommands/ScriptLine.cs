using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Lunetrip;
using Avespoir.Core.Modules.Utils;
using NLua;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command(/*"scriptline"*/)]
		public async Task ScriptLine(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
				return;
			}
			string strings = string.Join(null, msgs);

			try {
				using Lua ScriptState = new ScriptInit(CommandObject).Lua_State;
				ScriptState.DoString(strings);
			}
			catch (NLua.Exceptions.LuaScriptException LuaError) {
				await CommandObject.Channel.SendMessageAsync("エラーが発生しました: " + LuaError.Message);
			}
		}
	}
}
