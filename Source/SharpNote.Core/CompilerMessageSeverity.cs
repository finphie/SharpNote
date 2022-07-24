namespace SharpNote.Core;

/// <summary>
/// コンパイラメッセージのレベル
/// </summary>
public enum CompilerMessageSeverity
{
    /// <summary>
    /// 重要ではない情報
    /// </summary>
    Hidden,

    /// <summary>
    /// 情報
    /// </summary>
    Info,

    /// <summary>
    /// 警告
    /// </summary>
    Warning,

    /// <summary>
    /// エラー
    /// </summary>
    Error
}
