using Avespoir.Core;
using Avespoir.Core.JsonScheme;
using System;
using System.IO;
using System.Text.Json;

namespace Avespoir {

	static class Program {
		
		static void Main(string[] args) {
			// Need to be prepared in advance because it is deleted by gitignore
			#if REPO
			string ClientConfigPath = @"../../Configs/ClientConfig.json";
			string DBConfigPath = @"../../Configs/DBConfig.json";
			#else
			string ClientConfigPath = @"./Configs/ClientConfig.json";
			string DBConfigPath = @"./Configs/DBConfig.json";
			#endif

			if (File.Exists(ClientConfigPath) && File.Exists(DBConfigPath)) {
				string ClientConfigJsonData = File.ReadAllText(ClientConfigPath);
				string DBConfigJsonData = File.ReadAllText(DBConfigPath);

				JsonSerializer.Deserialize<ClientConfig>(ClientConfigJsonData);
				JsonSerializer.Deserialize<DBConfig>(DBConfigJsonData);
				
				new StartClient();
			}
			else {
				Console.WriteLine("Configfile is not exist.");
			}
		}
	}
}