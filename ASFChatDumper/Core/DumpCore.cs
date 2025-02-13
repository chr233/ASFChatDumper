using System.Text;

namespace ASFChatDumper.Core;
internal static class DumpCore
{
    public static async Task DempChatToCsv(string friendName, IEnumerable<DumpChatData> chatDatas)
    {
        EnsureDirectory();

        friendName = friendName
            .Replace("/", "")
            .Replace("\\", "")
            .Replace(":", "")
            .Replace("?", "")
            .Replace("\"", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("|", "");

        var filePath = Path.Combine(OutputPath, $"{friendName}-{DateTime.Now:yyyy_MM_dd-HH_mm_dd}.csv");
        using var fs = new FileStream(filePath, FileMode.Create);
        using var sw = new StreamWriter(fs, Encoding.UTF8);
        sw.WriteLine("发送者,发送者 SteamID,接收者,接收者 SteamID,消息内容,发送时间 (UTC),时间戳");
        foreach (var data in chatDatas)
        {
            var sanitizedMessage = data.Message?.Replace("\"", "\"\"");
            if (sanitizedMessage?.Contains('\n') == true || sanitizedMessage?.Contains('\r') == true)
            {
                sanitizedMessage = $"\"{sanitizedMessage}\"";
            }

            sw.WriteLine($"{data.SenderName},{data.SenderSteamId},{data.ReceiverName},{data.ReceiverSteamId},{sanitizedMessage},{data.Time:F},{data.Timestamp}");
        }
        await sw.FlushAsync().ConfigureAwait(false);
        await fs.FlushAsync().ConfigureAwait(false);
    }

    private static void EnsureDirectory()
    {
        if (!Directory.Exists(OutputPath))
        {
            Directory.CreateDirectory(OutputPath);
        }
    }
}
