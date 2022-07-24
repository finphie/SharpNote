namespace SharpNote.Logging;

/// <summary>
/// SharpNote専用ロガー
/// </summary>
public interface ISharpNoteLogger
{
    /// <summary>
    /// トレースメッセージを出力します。
    /// </summary>
    /// <param name="payload">メッセージ</param>
    void Trace<T>(T payload);

    /// <summary>
    /// デバッグメッセージを出力します。
    /// </summary>
    /// <param name="payload">メッセージ</param>
    void Debug<T>(T payload);

    /// <summary>
    /// 情報メッセージを出力します。
    /// </summary>
    /// <param name="payload">メッセージ</param>
    void Information(string payload);

    /// <summary>
    /// 警告メッセージを出力します。
    /// </summary>
    /// <param name="payload">メッセージ</param>
    void Warning(string payload);

    /// <summary>
    /// エラーメッセージを出力します。
    /// </summary>
    /// <param name="payload">メッセージ</param>
    void Error(string payload);

    /// <summary>
    /// 致命的なエラーメッセージを出力します。
    /// </summary>
    /// <param name="payload">メッセージ</param>
    void Critical(string payload);
}

/// <summary>
/// SharpNote専用ロガー
/// </summary>
/// <typeparam name="TCategoryName">カテゴリー名</typeparam>
public interface ISharpNoteLogger<out TCategoryName> : ISharpNoteLogger
{
}
