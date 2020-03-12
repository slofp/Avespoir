using AvespoirTest.Core;
using System.IO;
using AvespoirTest.Core.Configs;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace AvespoirTest {

	static class Program {
		
		static void Main(string[] args) {
			// gitignoreで削除しているため事前に用意する必要性あり
			// Projectプロパティから作業ディレクトリを変更してください
			string ClientConfigPath = @"./Configs/ClientConfig.json";
			string DBConfigPath = @"./Configs/DBConfig.json";

			try {
				if (File.Exists(ClientConfigPath) && File.Exists(DBConfigPath)) {
					string ClientConfigJsonData = File.ReadAllText(ClientConfigPath);
					string DBConfigJsonData = File.ReadAllText(DBConfigPath);

					JsonSerializer.Deserialize<GetClientConfigJson>(ClientConfigJsonData);
					JsonSerializer.Deserialize<GetDBConfigJson>(DBConfigJsonData);
				}
				else {
					throw new FileNotFoundException();
				}

				new StartClient(args);
			}
			catch(FileNotFoundException Error) {
				Console.Error.WriteLine(Error);
			}
		}
	}
}