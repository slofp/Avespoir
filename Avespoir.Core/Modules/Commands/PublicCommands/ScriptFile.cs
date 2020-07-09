using Avespoir.Core.Attributes;
using Avespoir.Core.Exceptions;
using Avespoir.Core.Modules.Lunetrip;
using DSharpPlus.Entities;
using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command(/*"scriptfile"*/)]
		public async Task ScriptFile(CommandObjects CommandObject) {
			try {
				IEnumerator<DiscordAttachment> Attachment = CommandObject.Message.Attachments.GetEnumerator();
				if (!Attachment.MoveNext()) throw new UrlNotFoundException();

				Uri FileUrl = new Uri(Attachment.Current.Url);

				using WebClient GetFile = new WebClient();
				byte[] Filebyte = await GetFile.DownloadDataTaskAsync(FileUrl).ConfigureAwait(false);
				using Stream File = new MemoryStream(Filebyte);

				using StreamReader FileReader = new StreamReader(File);
				string FileString = await FileReader.ReadToEndAsync().ConfigureAwait(false);

				try {
					using Lua ScriptState = new ScriptInit(CommandObject).Lua_State;
					ScriptState.DoString(FileString);
				}
				catch (NLua.Exceptions.LuaScriptException LuaError) {
					await CommandObject.Channel.SendMessageAsync("エラーが発生しました: " + LuaError.Message);
				}
			}
			catch (UrlNotFoundException) {
				await CommandObject.Channel.SendMessageAsync("画像が指定されていません！");
			}
		}
	}
}
