using Avespoir.Core.Modules.Logger;
using Discord;
using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Audio {

	class VCInfo {

		internal delegate void ErrorHandler(string ErrorText);

		internal event ErrorHandler ErrorEvent;

		internal SocketVoiceChannel VoiceChannel { get; private set; }

		internal IAudioClient AudioClient { get; private set; }

		internal AudioOutStream DiscordStream { get; private set; }

		internal DateTime LastUpdateDate { get; private set; }

		private Queue<Tuple<string, GetAudioType>> AudioQueue { get; }

		private ProcessStartInfo AudioProcessInfo { get; set; }

		private Process AudioProcess { get; set; }

		private MemoryStream AudioMemory { get; set; }

		private bool Finalized = false;

		private VCInfo(SocketVoiceChannel VoiceChannel, IAudioClient AudioClient, int Bitrate = 64000) {
			UpdateVoiceChannel(VoiceChannel, AudioClient, Bitrate);

			AudioProcessInfo = new ProcessStartInfo() {
				FileName = @"cmd.exe", // OSごとに分ける必要あり(大まかにUnixとWindowsで、その時点でshellとcmd限定になる)
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				StandardOutputEncoding = Encoding.UTF8
			};

			AudioQueue = new Queue<Tuple<string, GetAudioType>>();
			LastUpdateDate = DateTime.Now;
		}

		internal static async Task<VCInfo> Create(SocketVoiceChannel VoiceChannel) {
			VCInfo VC_Info = new VCInfo(VoiceChannel, await VoiceChannel.ConnectAsync(), VoiceChannel.Bitrate);
			VC_Info.AudioClient.Disconnected += _ => VC_Info.Finalize();

			Client.ConnectedVoiceChannel_Dict.Add(VoiceChannel.Guild.Id, VC_Info);

			Log.Info("ConnectedVoiceChannel_Dict Added");

			return VC_Info;
		}

		private Task StartSpeaking() {
			while (AudioClient.ConnectionState != ConnectionState.Disconnecting || AudioClient.ConnectionState != ConnectionState.Disconnected) {
				foreach (KeyValuePair<ulong, AudioInStream> AudioInputPair in AudioClient.GetStreams()) {
					// Key: UserID, Value: UserVoice
					Log.Debug(AudioInputPair.Key);
				}
				break;
			}

			return Task.CompletedTask;
		}

		internal Task UpdateDate(bool ProcessLoop) {
			while (ProcessLoop && !Finalized)
				if (!AudioProcess.HasExited)
					if (VoiceChannel.Users.Count > 1) LastUpdateDate = DateTime.Now;
					else Log.Debug(VoiceChannel.Users.Count);
				else break;

			LastUpdateDate = DateTime.Now;

			return Task.CompletedTask;
		}

		internal void AddQueue(string URL, GetAudioType GetType) {
			AudioQueue.Enqueue(new Tuple<string, GetAudioType>(URL, GetType));
			Log.Info("Queue Added");
		}

		internal async Task UpdateVoiceChannel(SocketVoiceChannel VoiceChannel) {
			await this.VoiceChannel.DisconnectAsync();
			UpdateVoiceChannel(VoiceChannel, await VoiceChannel.ConnectAsync(), VoiceChannel.Bitrate);
		}

		private void UpdateVoiceChannel(SocketVoiceChannel VoiceChannel, IAudioClient AudioClient, int Bitrate = 64000) {
			this.AudioClient = AudioClient;
			this.VoiceChannel = VoiceChannel;

			if (!(DiscordStream is null)) DiscordStream.Close();
			DiscordStream = AudioClient.CreatePCMStream(AudioApplication.Mixed, Bitrate);

			if (!(AudioProcess is null)) {
				AudioMemory.CopyTo(DiscordStream);
				DiscordStream.FlushAsync().ConfigureAwait(false);
			}
		}

		internal Task Start() {
			StartSpeaking().ConfigureAwait(false);

			while (!Finalized) {
				if (AudioQueue.Count > 0 && (AudioProcess is null || AudioProcess.HasExited)) {
					(string URL, GetAudioType GetType) = AudioQueue.Dequeue();
					StartProcess(URL, GetType);
				}
			}

			return Task.CompletedTask;
		}

		internal void StartProcess(string URL, GetAudioType GetType) {
			switch (GetType) {
				case GetAudioType.Streamlink:
					AudioProcessInfo.WorkingDirectory = @"D:\User_files\ふぁい\Desktop\Streamlink\bin";
					AudioProcessInfo.Arguments = string.Format("/Q /C streamlink -l error {0} best -O | ffmpeg -loglevel error -i pipe:0 -vn -f s16le pipe:1", URL); // -ar 48000 -ac 2
					break;
				case GetAudioType.Ytdl:
				default:
					if (GetType != GetAudioType.Ytdl) Log.Warning("GetType is not found. Start in youtube-dl");
					AudioProcessInfo.Arguments = string.Format("/Q /C youtube-dl --quiet -f bestaudio/best {0} -o - | ffmpeg -loglevel error -i pipe:0 -vn -f s16le pipe:1", URL); // -ar 48000 -ac 2
					break;
			}

			AudioProcess = Process.Start(AudioProcessInfo);
			AudioProcess.ErrorDataReceived += AudioProcess_ErrorDataReceivedEvent;
			AudioProcess.BeginErrorReadLine();

			AudioStreamStart();

			UpdateDate(true).ConfigureAwait(false);
		}

		private void AudioStreamStart() {
			if (!(AudioMemory is null)) AudioMemory.Close();

			AudioMemory = new MemoryStream();
			// VC切り替え時にSystem.OperationCanceledExceptionがでる
			AudioProcess.StandardOutput.BaseStream.CopyTo(AudioMemory);
			AudioMemory.CopyTo(DiscordStream);
			DiscordStream.FlushAsync().ConfigureAwait(false);
		}

		private void AudioProcess_ErrorDataReceivedEvent(object _, DataReceivedEventArgs DataReceive) {
			if (string.IsNullOrEmpty(DataReceive.Data)) return;
			Log.Error(DataReceive.Data);
			ErrorEvent?.Invoke(DataReceive.Data);
			ExitProcess();
		}

		private void ExitProcess() {
			lock (AudioProcess) {
				if (!AudioProcess.HasExited) {
					AudioProcess.Kill();
					AudioProcess.WaitForExit();
				}
			}
		}

		internal async Task Finalize() {
			if (Finalized) return;
			Log.Info("VCInfo Finalized");
			Finalized = true;

			await DiscordStream.DisposeAsync().ConfigureAwait(false);
			await VoiceChannel.DisconnectAsync().ConfigureAwait(false);
			ExitProcess();
			AudioProcess.Dispose();
			AudioQueue.Clear();
			Client.ConnectedVoiceChannel_Dict.Remove(Client.ConnectedVoiceChannel_Dict.First(Key_Pair => Key_Pair.Value == this).Key);
		}
	}
}
