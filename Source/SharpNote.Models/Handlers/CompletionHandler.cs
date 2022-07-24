using MessagePipe;
using Microsoft.Extensions.Logging;
using SharpNote.Core;
using SharpNote.Services;

namespace SharpNote.Models.Handlers;

/// <summary>
/// 入力補完のハンドラーです。
/// </summary>
public sealed partial class CompletionHandler : IAsyncRequestHandler<CaretPosition, CompletionList>
{
    readonly ILogger _logger;
    readonly ICompletionService _completionService;

    /// <summary>
    /// <see cref="CompletionHandler"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="completionService">コード補完候補を取得するサービス</param>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/>または<paramref name="completionService"/>がnullです。</exception>
    public CompletionHandler(ILogger<CompletionHandler> logger, ICompletionService completionService)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(completionService);

        _logger = logger;
        _completionService = completionService;
    }

    /// <summary>
    /// 入力補完候補を取得します。
    /// </summary>
    /// <param name="request">コードの情報</param>
    /// <param name="cancellationToken">キャンセル要求を行うためのトークン</param>
    /// <returns>このメソッドが完了すると、入力補完候補を返します。</returns>
    public ValueTask<CompletionList> InvokeAsync(CaretPosition request, CancellationToken cancellationToken = default)
    {
        Starting();
        return _completionService.ExecuteAsync(request.Text, request.Position, cancellationToken);
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = $"{nameof(CompletionHandler)} is starting.")]
    partial void Starting();
}
