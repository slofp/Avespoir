using System.Text.RegularExpressions;

namespace Avespoir.Core.Modules.Utils {

	class PatternMatch {

		static readonly string URLPattern = @"(http|https?)://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+";

		static readonly string IDPattern = @"<(@!|@&|#|a:[\S]+:|:[\S]+:)([0-9]+)>";

		static readonly string CodePattern = @"```(.|\s)+```";

		static readonly string BeyondNumbersPattern = @"[^0-9]";

		internal static bool MatchUrl(string Text)
			=> Regex.IsMatch(Text, URLPattern);

		internal static bool MatchID(string Text)
			=> Regex.IsMatch(Text, IDPattern);

		internal static bool MatchCode(string Text)
			=> Regex.IsMatch(Text, CodePattern);

		internal static bool MatchBeyondNumbers(string Text)
			=> Regex.IsMatch(Text, BeyondNumbersPattern);

		internal static int MatchUrlCount(string Text)
			=> Regex.Matches(Text, URLPattern).Count;

		internal static int MatchIDCount(string Text)
			=> Regex.Matches(Text, IDPattern).Count;

		internal static int MatchCodeCount(string Text)
			=> Regex.Matches(Text, CodePattern).Count;

		internal static int MatchBeyondNumbersCount(string Text)
			=> Regex.Matches(Text, BeyondNumbersPattern).Count;

		internal static string ReplaceUrl(string Text, string Replacement)
			=> Regex.Replace(Text, URLPattern, Replacement);

		internal static string ReplaceID(string Text, string Replacement)
			=> Regex.Replace(Text, IDPattern, Replacement);

		internal static string ExtructID(string Text)
			=> Regex.Match(Text, IDPattern).Groups[2].Value.Trim();

		internal static string ReplaceCode(string Text, string Replacement)
			=> Regex.Replace(Text, CodePattern, Replacement);

		internal static string ReplaceBeyondNumbers(string Text, string Replacement)
			=> Regex.Replace(Text, BeyondNumbersPattern, Replacement);
	}
}
