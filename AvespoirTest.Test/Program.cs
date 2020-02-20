using AvespoirTest.Core;
using System.IO;
using AvespoirTest.Core.Configs;
using Newtonsoft.Json;
using System;

namespace AvespoirTest {

	class Program {
		static void Main(string[] args) {
			// gitignoreで削除しているため事前に用意する必要性あり
			// exeがBinにあるため3つのパスバックが必要※要検証
			string FilePath = @"../../../Configs/config.json";

			try {
				if (File.Exists(FilePath)) {
					var FileData = new StreamReader(FilePath);
					string JsonData = FileData.ReadToEnd();
					FileData.Close();

					GetConfigJson ConfigJson = new GetConfigJson();

					ConfigJson = JsonConvert.DeserializeObject<GetConfigJson>(JsonData);

				}
				else {
					throw new FileNotFoundException();
				}

				new StartClient(args);
			}
			catch(Exception Error) {
				Console.Error.WriteLine(Error);
			}
		}
	}
}