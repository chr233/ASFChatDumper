using ArchiSteamFarm.Core;
using ArchiSteamFarm.NLog;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using System.Reflection;
using System.Text;

namespace ASFChatDumper;

internal static class Utils
{
    /// <summary>
    ///     插件配置
    /// </summary>
    internal static PluginConfig Config { get; set; } = new();

    /// <summary>
    ///     更新已就绪
    /// </summary>
    internal static bool UpdatePadding { get; set; }

    /// <summary>
    ///     更新标记
    /// </summary>
    /// <returns></returns>
    private static string UpdateFlag => UpdatePadding ? "*" : "";

    /// <summary>
    ///     获取版本号
    /// </summary>
    internal static Version MyVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new Version("0");

    /// <summary>
    ///     获取插件所在路径
    /// </summary>
    internal static string MyLocation => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    internal static string OutputPath => Path.Combine(MyLocation, "chat_dump");

    /// <summary>
    ///     Steam商店链接
    /// </summary>
    internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;

    /// <summary>
    ///     Steam社区链接
    /// </summary>
    internal static Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;
    /// <summary>
    /// Stean客服链接
    /// </summary>
    internal static Uri SteamHelpURL => ArchiWebHandler.SteamHelpURL;

    /// <summary>
    ///     SteamAPI链接
    /// </summary>
    internal static Uri SteamApiURL => new("https://api.steampowered.com");

    /// <summary>
    ///     日志
    /// </summary>
    internal static ArchiLogger ASFLogger => ASF.ArchiLogger;

    /// <summary>
    ///     格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message)
    {
        return $"<ASFE{UpdateFlag}> {message}";
    }

    /// <summary>
    ///     格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message, params object?[] args)
    {
        return FormatStaticResponse(string.Format(message, args));
    }

    /// <summary>
    ///     格式化返回文本
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatBotResponse(this Bot bot, string message)
    {
        return $"<{bot.BotName}{UpdateFlag}> {message}";
    }

    /// <summary>
    ///     格式化返回文本
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal static string FormatBotResponse(this Bot bot, string message, params object?[] args)
    {
        return bot.FormatBotResponse(string.Format(message, args));
    }

    internal static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object?[] args)
    {
        return sb.AppendLine(string.Format(format, args));
    }

    /// <summary>
    /// 转换SteamId
    /// </summary>
    /// <param name="steamId"></param>
    /// <returns></returns>
    internal static ulong SteamId2Steam32(ulong steamId)
    {
        return IsSteam32ID(steamId) ? steamId : steamId - 0x110000100000000;
    }

    /// <summary>
    /// 转换SteamId
    /// </summary>
    /// <param name="steamId"></param>
    /// <returns></returns>
    internal static ulong Steam322SteamId(ulong steamId)
    {
        return IsSteam32ID(steamId) ? steamId + 0x110000100000000 : steamId;
    }

    internal static bool IsSteam32ID(ulong id)
    {
        return id <= 0xFFFFFFFF;
    }
}