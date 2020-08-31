using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace Avespoir.Core.Language {

	class GetLanguage {

		internal JsonScheme.Language Language_Data { get; }

		private static Assembly assembly = Assembly.GetExecutingAssembly();

		internal GetLanguage(Database.Enums.Language Language_Config) {
			string Language_Name = Enum.GetName(typeof(Database.Enums.Language), Language_Config);

			using Stream Language_Stream = assembly.GetManifestResourceStream($"Avespoir.Core.Language.Resources.{Language_Name}.json");
			using StreamReader Language_StreamReader = new StreamReader(Language_Stream);

			Language_Data = JsonSerializer.Deserialize<JsonScheme.Language>(Language_StreamReader.ReadToEnd());
		}
	}
}
