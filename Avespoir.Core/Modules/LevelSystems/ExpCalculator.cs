using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Avespoir.Core.Modules.LevelSystems {

	class ExpCalculator {

		static readonly SHA256 CryptoProvider = new SHA256CryptoServiceProvider();

		internal static (int MessageCount, string MessageIDReplace) ExpConvert(string MessageID, string MessageContentSource) {
			string MessageCodeContent = PatternMatch.ReplaceCode(MessageContentSource, "");
			string MessageNoURLContent = PatternMatch.ReplaceUrl(MessageCodeContent, "");
			string MessageContent = PatternMatch.ReplaceID(MessageNoURLContent, "");

			Log.Debug("MessageCodeContent: " + MessageCodeContent);
			Log.Debug("MessageNoURLContent: " + MessageNoURLContent);
			Log.Debug("MessageContent: " + MessageContent);
			Log.Debug("MessageContentCount: " + MessageContent.Length);

			int MessageCodeCount = PatternMatch.MatchCodeCount(MessageContentSource);
			int MessageURLCount = PatternMatch.MatchUrlCount(MessageCodeContent);
			int MessageMentionEmojiCount = PatternMatch.MatchIDCount(MessageNoURLContent);

			Log.Debug("MessageCodeCount: " + MessageCodeCount);
			Log.Debug("MessageURLCount: " + MessageURLCount);
			Log.Debug("MessageMentionEmojiCount: " + MessageMentionEmojiCount);

			int MessageCount = MessageContent.Length + MessageCodeCount + MessageURLCount + MessageMentionEmojiCount;

			byte[] MessageIDBytes = Encoding.UTF8.GetBytes(MessageID);
			byte[] MessageIDHashBytes = CryptoProvider.ComputeHash(MessageIDBytes);

			string MessageIDHash = "";
			for (int i = 0; i < MessageIDHashBytes.Length; i++) {
				MessageIDHash += string.Format("{0:X2}", MessageIDHashBytes[i]);
			}

			string MessageIDReplace = PatternMatch.ReplaceBeyondNumbers(MessageIDHash, "");
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
			if (MessageCount < 100) ExpScale = 0.01 * MessageCount;
			else if (MessageCount < 1000) ExpScale = Math.Sin((MessageCount - 100) / 573.01) + 1;
			else ExpScale = (((MessageCount - 1000) * (MessageCount / 4.0)) / 250000.0) + 2;

			ExpScale /= 2.0; // Nerf Final Exp

			Exp *= ExpScale;

			Log.Debug("LevelScale: " + ExpScale + "x");
			Log.Debug("FinalLevel: " + Exp);

			return Exp;
		}
	}
}
