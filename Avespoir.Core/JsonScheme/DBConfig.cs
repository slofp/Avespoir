using Avespoir.Core.Configs;

namespace Avespoir.Core.JsonScheme {

	public class DBConfig {

		#region Database Config

		public string Filename {
			get {
				return LiteDBConfigs.FileName;
			}
			set {
				LiteDBConfigs.FileName = value;
			}
		}

		public string Password {
			get {
				return LiteDBConfigs.Password;
			}
			set {
				LiteDBConfigs.Password = value;
			}
		}

		#endregion
	}
}
