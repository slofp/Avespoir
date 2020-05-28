using Avespoir.Core.Modules.Logger;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Avespoir.Core.Modules.LevelSystems {

	class ExpCalculator {

		static readonly string URLPattern = @"(http|https?)://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+";

		static readonly string IDPattern = @"<(@!|@&|#|a:[\S]+:|:[\S]+:)[0-9]+>";

		internal static (int MessageCount, string MessageIDReplace) ExpConvert(string MessageID, string MessageContentSource) {
			string MessageNoURLContent = Regex.Replace(MessageContentSource, URLPattern, "");
			string MessageContent = Regex.Replace(MessageNoURLContent, IDPattern, "");

			Log.Debug("MessageNoURLContent: " + MessageNoURLContent);
			Log.Debug("MessageContent: " + MessageContent);
			Log.Debug("MessageContentCount: " + MessageContent.Length);

			int MessageURLCount = Regex.Matches(MessageContentSource, URLPattern).Count;
			int MessageMentionEmojiCount = Regex.Matches(MessageNoURLContent, IDPattern).Count;

			Log.Debug("MessageURLCount: " + MessageURLCount);
			Log.Debug("MessageMentionEmojiCount: " + MessageMentionEmojiCount);

			int MessageCount = MessageContent.Length + MessageURLCount + MessageMentionEmojiCount;

			SHA256 CryptoProvider = new SHA256CryptoServiceProvider();

			byte[] MessageIDBytes = Encoding.UTF8.GetBytes(MessageID);
			byte[] MessageIDHashBytes = CryptoProvider.ComputeHash(MessageIDBytes);

			string MessageIDHash = "";
			for (int i = 0; i < MessageIDHashBytes.Length; i++) {
				MessageIDHash += string.Format("{0:X2}", MessageIDHashBytes[i]);
			}

			string MessageIDReplace = Regex.Replace(MessageIDHash, @"[^0-9]", "");
			Log.Debug("MessageIDReplace: " + MessageIDReplace);

			return (MessageCount, MessageIDReplace);
		}

		internal static double ExpCalculate(int MessageCount, string MessageIDReplace) {
			double ExpSource = double.Parse(MessageIDReplace);
			Log.Debug("ExpSource: " + ExpSource);

			int ExpNerf = 62;
			double Exp = ExpSource;
			for (int i = 0; i < ExpNerf; i++) {
				Exp /= 10.0;
			}

			int FloorCount = 0;
			for (double ExpFloor = Math.Floor(Exp); ExpFloor.ToString().Length < 3; ExpFloor = Math.Floor(Exp), FloorCount++) {
				Log.Debug("ExpFloor " + FloorCount + ": " + ExpFloor);
				Exp *= 10.0;
			}
			Log.Debug("ExpFloor " + FloorCount + ": " + Math.Floor(Exp));

			Log.Debug("Exp: " + Exp);
			double ExpScale;
			if (MessageCount <= 20) ExpScale = 0.05 * MessageCount;
			else if (MessageCount < 100) ExpScale = Math.Sin((MessageCount - 20) / 50.93) + 1;
			else ExpScale = (((MessageCount - 100) * (MessageCount / 2.0)) / 237500.0) + 2;

			Exp *= ExpScale;

			Log.Debug("LevelScale: " + ExpScale + "x");
			Log.Debug("FinalLevel: " + Exp);

			return Exp;
		}
	}
}
