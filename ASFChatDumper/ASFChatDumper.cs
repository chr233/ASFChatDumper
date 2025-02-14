using ArchiSteamFarm.Core;
using ArchiSteamFarm.Helpers.Json;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFChatDumper.Core;
using System.ComponentModel;
using System.Composition;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ASFChatDumper;

[Export(typeof(IPlugin))]
internal sealed class ASFChatDumper : IASF, IBotCommand2, IBotMessage
{
    private bool ASFEBridge;

    public static PluginConfig Config => Utils.Config;

    private static Timer? StatisticTimer;

    private int Day;

    /// <summary>
    ///     获取插件信息
    /// </summary>
    private string? PluginInfo => $"{Name} {Version}";

    public string Name => "ASF Chat Dumper";
    public Version Version => MyVersion;

    /// <summary>
    ///     ASF启动事件
    /// </summary>
    /// <param name="additionalConfigProperties"></param>
    /// <returns></returns>
    public Task OnASFInit(IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties = null)
    {
        PluginConfig? config = null;

        if (additionalConfigProperties != null)
        {
            foreach (var (configProperty, configValue) in additionalConfigProperties)
            {
                if (configProperty == "ASFEnhance" && configValue.ValueKind == JsonValueKind.Object)
                {
                    try
                    {
                        config = configValue.ToJsonObject<PluginConfig>();
                        if (config != null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ASFLogger.LogGenericException(ex);
                    }
                }
            }
        }

        Utils.Config = config ?? new PluginConfig();

        var warnings = new StringBuilder();

        //使用协议
        if (!Config.EULA)
        {
            warnings.AppendLine();
            warnings.AppendLine(Langs.Line);
            warnings.AppendLineFormat(Langs.EulaWarning, Name);
            warnings.AppendLine(Langs.Line);
        }

        if (Config.EnableDailyDump)
        {
            ASFLogger.LogGenericWarning(Langs.EnableDailyDumpTips);
        }

        if (warnings.Length > 0)
        {
            ASFLogger.LogGenericWarning(warnings.ToString());
        }

        //统计
        if (Config.Statistic && !ASFEBridge)
        {
            var request = new Uri("https://asfe.chrxw.com/asfridstool");
            StatisticTimer = new Timer(
                async _ => await ASF.WebBrowser!.UrlGetToHtmlDocument(request).ConfigureAwait(false),
                null,
                TimeSpan.FromSeconds(30),
                TimeSpan.FromHours(24)
            );
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        ASFLogger.LogGenericInfo(Langs.PluginContact);
        ASFLogger.LogGenericInfo(Langs.PluginInfo);

        var flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        var handler = typeof(ASFChatDumper).GetMethod(nameof(ResponseCommand), flag);

        const string pluginId = nameof(ASFChatDumper);
        const string cmdPrefix = "ACD";
        const string repoName = "ASFChatDumper";

        ASFEBridge = AdapterBridge.InitAdapter(Name, pluginId, cmdPrefix, repoName, handler);

        if (ASFEBridge)
        {
            ASFLogger.LogGenericDebug(Langs.ASFEnhanceRegisterSuccess);
        }
        else
        {
            ASFLogger.LogGenericInfo(Langs.ASFEnhanceRegisterFailed);
            ASFLogger.LogGenericWarning(Langs.PluginStandalongMode);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     处理命令事件
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamId = 0)
    {
        if (ASFEBridge)
        {
            return null;
        }

        if (!Enum.IsDefined(access))
        {
            throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
        }

        try
        {
            var cmd = args[0].ToUpperInvariant();

            if (cmd.StartsWith("ACD."))
            {
                cmd = cmd[4..];
            }

            var task = ResponseCommand(bot, access, cmd, args);
            if (task != null)
            {
                return await task.ConfigureAwait(false);
            }

            return null;
        }
        catch (Exception ex)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                ASFLogger.LogGenericException(ex);
            }).ConfigureAwait(false);

            return ex.StackTrace;
        }
    }

    /// <summary>
    ///     处理命令
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    private Task<string?>? ResponseCommand(Bot bot, EAccess access, string cmd, string[] args)
    {
        var argLength = args.Length;
        return argLength switch
        {
            0 => throw new InvalidOperationException(nameof(args)),
            1 => cmd switch
            {
                //Plugin Info
                "ASFCHATDUMPER" or
                "ACD" when access >= EAccess.FamilySharing =>
                    Task.FromResult(PluginInfo),

                "DUMPCHAT" or
                "DC" when Config.EULA && access >= EAccess.Master =>
                    Command.ResponseDumpChat(bot, false),

                "DUMPCHATMIX" or
                "DCM" when Config.EULA && access >= EAccess.Master =>
                    Command.ResponseDumpChat(bot, true),

                _ => null
            },
            _ => cmd switch
            {
                "DUMPCHAT" or
                "DC" when Config.EULA && access >= EAccess.Master =>
                    Command.ResponseDumpChat(Utilities.GetArgsAsText(args, 1, ",")),

                "DUMPCHATMIX" or
                "DCM" when Config.EULA && access >= EAccess.Master =>
                    Command.ResponseDumpChat(Utilities.GetArgsAsText(args, 1, ",")),

                _ => null
            }
        };
    }

    public async Task<string?> OnBotMessage(Bot bot, ulong steamID, string message)
    {
        if (!Config.EULA || !Config.EnableDailyDump || DateTime.Now.Day == Day)
        {
            return null;
        }

        Day = DateTime.Now.Day;
        await Command.ResponseDumpChat(bot, Config.IsDailyDumpMix).ConfigureAwait(false);
        ASFLogger.LogGenericWarning(Langs.ChatHistoryDumped);
        return null;
    }
}