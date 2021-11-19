using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("join", RoleLevel.Public)]
	class Join : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await Command_Object.Channel.SendMessageAsync("何も入力されていません");
				return;
			}

			DiscordChannel VoiceChannel = Command_Object.Member.VoiceState?.Channel;

			if (VoiceChannel is null) {
				await Command_Object.Channel.SendMessageAsync("入ってません");

				return;
			}

			try {
				VoiceNextConnection connection = await VoiceChannel.ConnectAsync();

				Uri VoiceUrl;
				VoiceUrl = new Uri(@"http://127.0.0.1:8080/api/speechtext");
				Log.Debug(VoiceUrl.ToString());

				HttpWebRequest PostVoice = (HttpWebRequest) WebRequest.Create(VoiceUrl);
				PostVoice.Method = "POST";
				PostVoice.ContentType = "application/json";

				using (var streamWriter = new StreamWriter(PostVoice.GetRequestStream())) {
					string json = $"{{ \"Text\": \"{msgs[0]}\" }} ";

					streamWriter.Write(json);
				}

				Guid CacheId = Guid.NewGuid();
				Log.Debug(CacheId);

				HttpWebResponse GetVoice = (HttpWebResponse) await PostVoice.GetResponseAsync();
				Log.Debug(GetVoice.ContentType);

				byte[] Buffer = new byte[1024];
				int ReadLength;
				Stream VoiceStream = GetVoice.GetResponseStream();
				using (FileStream Cache = new FileStream($"./{CacheId}", FileMode.Create, FileAccess.Write))
					while ((ReadLength = VoiceStream.Read(Buffer, 0, Buffer.Length)) > 0)
						Cache.Write(Buffer, 0, ReadLength);

				var ffmpeg = Process.Start(new ProcessStartInfo {
					FileName = "./ffmpeg",
					Arguments = $@"-i ./{CacheId} -ac 2 -f s16le -ar 48000 pipe:1",
					RedirectStandardOutput = true,
					UseShellExecute = false
				});

				Stream pcm = ffmpeg.StandardOutput.BaseStream;

				VoiceTransmitSink transmit = connection.GetTransmitSink();

				await pcm.CopyToAsync(transmit);

				await connection.WaitForPlaybackFinishAsync();

				Client.Bot.GetVoiceNext().GetConnection(Command_Object.Guild).Disconnect();

				File.Delete($"./{CacheId}");


			}
			catch (Exception error) {
				Log.Error(error);
			}

			//Stream TextVoice = new MemoryStream(Voicebyte);



			await Task.Delay(0);
		}
	}
}
