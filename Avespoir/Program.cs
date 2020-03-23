using Avespoir.Core;
using Avespoir.Core.Configs;
using System;
using System.IO;
using System.Text.Json;

namespace Avespoir {

	static class Program {
		
		static void Main(string[] args) {
			// Need to be prepared in advance because it is deleted by gitignore
			string ClientConfigPath = @"./Configs/ClientConfig.json";
			string DBConfigPath = @"./Configs/DBConfig.json";

			if (File.Exists(ClientConfigPath) && File.Exists(DBConfigPath)) {
				string ClientConfigJsonData = File.ReadAllText(ClientConfigPath);
				string DBConfigJsonData = File.ReadAllText(DBConfigPath);

				JsonSerializer.Deserialize<GetClientConfigJson>(ClientConfigJsonData);
				JsonSerializer.Deserialize<GetDBConfigJson>(DBConfigJsonData);
				
				new StartClient();
			}
			else {
				Console.WriteLine("Configfile is not exist.");
			}
		}
	}
}