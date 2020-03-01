using AvespoirTest.Core;
using System.IO;
using AvespoirTest.Core.Configs;
using System.Text.Json;
using System;
using System.Threading.Tasks;

namespace AvespoirTest {

	class Program {
		static void Main(string[] args) {
			// gitignoreで削除しているため事前に用意する必要性あり
			// Projectプロパティから作業ディレクトリを変更してください
			string ClientConfigPath = @"./Configs/ClientConfig.json";
			string DBConfigPath = @"./Configs/DBConfig.json";

			try {
				if (File.Exists(ClientConfigPath) && File.Exists(DBConfigPath)) {
					string ClientConfigJsonData = File.ReadAllText(ClientConfigPath);
					string DBConfigJsonData = File.ReadAllText(DBConfigPath);

					GetClientConfigJson ClientConfigJson = new GetClientConfigJson();
					GetDBConfigJson DBConfigJson = new GetDBConfigJson();

					ClientConfigJson = JsonSerializer.Deserialize<GetClientConfigJson>(ClientConfigJsonData);
					DBConfigJson = JsonSerializer.Deserialize<GetDBConfigJson>(DBConfigJsonData);
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