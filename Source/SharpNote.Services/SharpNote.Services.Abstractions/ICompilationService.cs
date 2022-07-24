using SharpNote.Core;
using Utf8Utility;

namespace SharpNote.Services;

/// <summary>
/// コンパイル処理に関する定義です。
/// </summary>
public interface ICompilationService
{
    /// <summary>
    /// コンパイルを実行します。
    /// </summary>
    /// <param name="sourceCodePath">ソースコードのパス</param>
    /// <param name="sourceCode">ソースコード</param>
    /// <param name="platform">プラットフォームの構成オプション</param>
    /// <param name="cancellationToken">タスクにキャンセル要求を行うためのトークン</param>
    /// <returns>コンパイル結果を返します。</returns>
    ValueTask<CompileResult> ExecuteAsync(string sourceCodePath, string sourceCode, PlatformOptions platform, CancellationToken cancellationToken = default);
}
