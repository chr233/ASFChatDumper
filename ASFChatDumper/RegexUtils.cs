using System.Text.RegularExpressions;

namespace ASFChatDumper;
internal static partial class RegexUtils
{
    [GeneratedRegex(@"\( (\d+),")]
    public static partial Regex MatchSubId();

    [GeneratedRegex("g_rgTopCurators = ([^;]+);")]
    public static partial Regex MatchCuratorPayload();
}
