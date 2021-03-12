using Avespoir.Core;
using Avespoir.Core.JsonScheme;
using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Avespoir {

	static class Program {

		#if DEBUG
		private const string ConfigDirPath = @"../../Configs";
		#else
		private const string ConfigDirPath = @"./Configs";
		#endif

		// Need to be prepared in advance because it is deleted by gitignore
		private const string ClientConfigPath = ConfigDirPath + @"/ClientConfig.json";

		private const string DBConfigPath = ConfigDirPath + @"/DBConfig.json";

		static async Task Main() {
			if (File.Exists(ClientConfigPath) && File.Exists(DBConfigPath)) {
				string ClientConfigJsonData = await File.ReadAllTextAsync(ClientConfigPath).ConfigureAwait(false);
				string DBConfigJsonData = await File.ReadAllTextAsync(DBConfigPath).ConfigureAwait(false);

				JsonSerializer.Deserialize<ClientConfig>(ClientConfigJsonData);
				JsonSerializer.Deserialize<DBConfig>(DBConfigJsonData);
				
				await StartClient.Start().ConfigureAwait(false);
			}
			else {
				if (!Directory.Exists(ConfigDirPath)) {
					Console.WriteLine("Config directory is not exist.");
					Console.WriteLine("Create Config directory...");

					Directory.CreateDirectory(ConfigDirPath);
				}

				JsonSerializerOptions SerializerOptions = new JsonSerializerOptions {
					WriteIndented = true,
					Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
				};

				if (!File.Exists(ClientConfigPath)) {
					Console.WriteLine("ClientConfig file is not exist.");
					Console.WriteLine("Please write ClientConfig file.");
					Console.WriteLine("Writing ClientConfig file...");

					string ClientConfigString = JsonSerializer.Serialize(new ClientConfig(), SerializerOptions);
					await File.WriteAllTextAsync(ClientConfigPath, ClientConfigString).ConfigureAwait(false);
				}
				if (!File.Exists(DBConfigPath)) {
					Console.WriteLine("DBConfig file is not exist.");
					Console.WriteLine("Please write DBConfig file.");
					Console.WriteLine("Writing DBConfig file...");

					string DBConfigString = JsonSerializer.Serialize(new DBConfig(), SerializerOptions);
					await File.WriteAllTextAsync(DBConfigPath, DBConfigString).ConfigureAwait(false);
				}
			}
		}
	}
}