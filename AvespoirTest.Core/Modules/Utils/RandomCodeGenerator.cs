using System;
using System.Collections.Generic;
using System.Text;

namespace AvespoirTest.Core.Modules.Utils {

	class RandomCodeGenerator {

		static char[] AlphabetListGenerate() {
			Encoding utf8 = Encoding.UTF8;
			byte AByte = 65,
				 aByte = 97;

			List<byte> AByteList = new List<byte>();
			for (byte i = AByte; i < AByte + 26; i++) {
				AByteList.Add(i);
			}

			List<byte> aByteList = new List<byte>();
			for (byte i = aByte; i < aByte + 26; i++) {
				aByteList.Add(i);
			}

			byte[] AByteArray = AByteList.ToArray(),
				   aByteArray = aByteList.ToArray();

			char[] ACharArray = utf8.GetChars(AByteArray),
				   aCharArray = utf8.GetChars(aByteArray);

			char[] AlphabetArray = new char[ACharArray.Length + aCharArray.Length];
			ACharArray.CopyTo(AlphabetArray, 0);
			aCharArray.CopyTo(AlphabetArray, ACharArray.Length);

			return AlphabetArray;
		}

		internal static string RandomCodeGenerate() {
			char[] AlphabetArray = AlphabetListGenerate();

			Random InitRandom = new Random();
			char[] AuthCharArray = new char[5];
			for (int i = 0; i < AuthCharArray.Length; i++) {
				int AuthInt = InitRandom.Next(AlphabetArray.Length);

				if (i != 0 && AuthCharArray[i - 1] == AlphabetArray[AuthInt]) {
					i--;
					continue;
				}

				AuthCharArray[i] = AlphabetArray[AuthInt];
			}

			return new string(AuthCharArray);
		}
	}
}
