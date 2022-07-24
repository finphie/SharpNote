using Microsoft.Extensions.Logging;
using ZLogger;

namespace SharpNote.Logging;

public sealed class SharpNoteLogger<TCategoryName> : ISharpNoteLogger<TCategoryName>
{
    readonly ILogger _logger;

    /// <summary>
    /// <see cref="SharpNoteLogger{TCategoryName}"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    public SharpNoteLogger(ILogger<TCategoryName> logger)
        => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc/>
    //public void Trace<T>(string payload) => _logger.ZLogTraceWithPayload(payload);

    /// <inheritdoc/>
    public void Debug(string payload) => _logger.ZLogDebug(payload);

    /// <inheritdoc/>
    public void Information(string payload) => _logger.ZLogInformation(payload);

    /// <inheritdoc/>
    public void Warning(string payload) => _logger.ZLogWarning(payload);

    /// <inheritdoc/>
    public void Error(string payload) => _logger.ZLogError(payload);

    /// <inheritdoc/>
    public void Critical(string payload) => _logger.ZLogCritical(payload);
}
