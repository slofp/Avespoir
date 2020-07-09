using Avespoir.Core.Modules.Commands;
using NLua;
using System.Text;

namespace Avespoir.Core.Modules.Lunetrip {

	class ScriptInit {

		internal Lua Lua_State = new Lua();

		/*
			This feature is currently under development!

			English:
			Lunetrip is Lua language base bot control script
			This can be used to make it work in ways other than just commands
			This feature is subject to significant changes in the future

			日本語:
			LunetripはLua言語ベースのBotコントロールスクリプトです
			これによりコマンド以外の動作をさせることを可能にしています
			この機能は今後大幅に変更する可能性があります
		 */
		internal ScriptInit (CommandObjects CommandObject) {
			Lua_State.State.Encoding = Encoding.UTF8;

			Lua_State.DoString("package = nil");
			Lua_State.DoString("os.execute = nil");
			Lua_State.DoString("os.remove = nil");
			Lua_State.DoString("os.rename = nil");
			Lua_State.DoString("os.getenv = nil");
			Lua_State.DoString("os.tmpname = nil");
			Lua_State.DoString("os.setlocale = nil");
			Lua_State.DoString("os.exit = nil");
			Lua_State.DoString("io = nil");
			Lua_State.DoString("dofile = nil");
			Lua_State.DoString("require = nil");
			Lua_State.DoString("debug = nil");

			ScriptMethods Script_Methods = new ScriptMethods(CommandObject);

			// Lua_State.NewTable("");
			// Lua_State.RegisterFunction("", Script_Methods, typeof(ScriptMethods).GetMethod(""));

			Lua_State.RegisterFunction("print", Script_Methods, typeof(ScriptMethods).GetMethod("Print"));

			Lua_State.NewTable("get_codesender");
			Lua_State.RegisterFunction("get_codesender.author", Script_Methods, typeof(ScriptMethods).GetMethod("GetCodeSender_Author"));
			Lua_State.RegisterFunction("get_codesender.channel", Script_Methods, typeof(ScriptMethods).GetMethod("GetCodeSender_Channel"));
			Lua_State.RegisterFunction("get_codesender.guild", Script_Methods, typeof(ScriptMethods).GetMethod("GetCodeSender_Guild"));
			Lua_State.RegisterFunction("get_codesender.member", Script_Methods, typeof(ScriptMethods).GetMethod("GetCodeSender_Member"));
			Lua_State.RegisterFunction("get_codesender.message", Script_Methods, typeof(ScriptMethods).GetMethod("GetCodeSender_Message"));

			Lua_State.NewTable("user");
			Lua_State.RegisterFunction("user.get_avatarurl", Script_Methods, typeof(ScriptMethods).GetMethod("User_GetAvatarUrl"));
			Lua_State.RegisterFunction("user.unban", Script_Methods, typeof(ScriptMethods).GetMethod("User_Unban"));

			Lua_State.NewTable("channel");
			Lua_State.RegisterFunction("send_message", Script_Methods, typeof(ScriptMethods).GetMethod("Channel_SendMessage"));
			Lua_State.RegisterFunction("channel.create_invite", Script_Methods, typeof(ScriptMethods).GetMethod("Channel_CreateInvite"));

			Lua_State.NewTable("guild");

			Lua_State.NewTable("member");

			Lua_State.NewTable("message");
			Lua_State.RegisterFunction("message.delete", Script_Methods, typeof(ScriptMethods).GetMethod("Message_Delete"));

			Lua_State.NewTable("discord");
			Lua_State.NewTable("discord.imageformat");
			Lua_State.RegisterFunction("discord.imageformat.gif", Script_Methods, typeof(ScriptMethods).GetMethod("Discord_ImageFormat_Gif"));
			Lua_State.RegisterFunction("discord.imageformat.jpeg", Script_Methods, typeof(ScriptMethods).GetMethod("Discord_ImageFormat_Jpeg"));
			Lua_State.RegisterFunction("discord.imageformat.png", Script_Methods, typeof(ScriptMethods).GetMethod("Discord_ImageFormat_Png"));
			Lua_State.RegisterFunction("discord.imageformat.unknown", Script_Methods, typeof(ScriptMethods).GetMethod("Discord_ImageFormat_Unknown"));
			Lua_State.RegisterFunction("discord.imageformat.webp", Script_Methods, typeof(ScriptMethods).GetMethod("Discord_ImageFormat_WebP"));
		}
	}
}
