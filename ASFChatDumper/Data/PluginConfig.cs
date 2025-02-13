namespace ASFChatDumper.Data;

/// <summary>
///     插件设置
/// </summary>
public sealed record PluginConfig
{
    int JsonIgnore;

    /// <summary>
    ///     是否同意使用协议
    /// </summary>
    public bool EULA { get; set; }

    /// <summary>
    ///     是否启用统计
    /// </summary>
    public bool Statistic { get; set; } = true;
}