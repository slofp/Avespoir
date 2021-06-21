using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Audio;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using Discord.Audio;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	[Command(/*"play", RoleLevel.Public*/)]
	class Play : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("曲を流します") {
			{ Database.Enums.Language.en_US, "Play the song" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}play") {
			{ Database.Enums.Language.en_US, "{0}play" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				await Command_Object.Channel.SendMessageAsync("URLが指定されていません");
				return;
			}
			else if (msgs.Length >= 1) {
				//string URL = msgs[0];

				await new Join { IsEntryPlay = true }.Execute(Command_Object).ConfigureAwait(false);
				Log.Debug("End Joined");
				if (!Client.ConnectedVoiceChannel_Dict.TryGetValue(Command_Object.Guild.Id, out VCInfo VC_Info)) return;

				VC_Info.AddQueue(msgs[0], GetAudioType.Ytdl);

				/*using AudioOutStream DiscordStream = VC_Info.AudioClient.CreatePCMStream(AudioApplication.Mixed, Command_Object.Member.VoiceChannel.Bitrate);

				ProcessStartInfo processStartInfo = new ProcessStartInfo() {
					FileName = @"cmd.exe", // OSごとに分ける必要あり(大まかにUnixとWindowsで、その時点でshellとcmd限定になる)
					WorkingDirectory = @"D:\User_files\ふぁい\Desktop\Streamlink\bin",
					Arguments = string.Format("/Q /C streamlink -l error {0} best -O | ffmpeg -loglevel error -i pipe:0 -vn -ar 48000 -ac 2 -f s16le pipe:1", msgs[0]),
					//Arguments = string.Format("/Q /C youtube-dl --quiet -f bestaudio {0} -o - | ffmpeg -loglevel error -i pipe:0 -vn -ar 48000 -ac 2 -f s16le pipe:1", msgs[0]),
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					StandardOutputEncoding = Encoding.UTF8
				};

				using Process YoutubeDL = Process.Start(processStartInfo);
				YoutubeDL.ErrorDataReceived += (object sender, DataReceivedEventArgs DataReceive) => {
					Log.Error(DataReceive.Data);
					//if (sender is Process Proc) if (!Proc.HasExited) Proc.Kill();
				};
				YoutubeDL.BeginErrorReadLine();
				YoutubeDL.StandardOutput.BaseStream.CopyTo(DiscordStream);
				await DiscordStream.FlushAsync();*/
				//await DiscordStream.WriteByte(YoutubeDL.StandardInput.BaseStream.ReadByte());
				//await YoutubeDL.StandardInput.BaseStream.ReadByte(DiscordStream);
				//await DiscordStream.FlushAsync();

				/*int Current;// = memoryStream.Write(Encoding.UTF8.GetBytes(Data.Data));
				while ((Current = proc.StandardOutput.BaseStream.ReadByte()) > -1) {
					memoryStream.WriteByte((byte) Current);
				}*/
			}
		}
	}
}
