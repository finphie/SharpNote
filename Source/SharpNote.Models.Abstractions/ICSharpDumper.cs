using System.ComponentModel;
using SharpNote.Core;

namespace SharpNote.Models;

/// <summary>
/// C#コードのダンプ処理に関する定義です。
/// </summary>
public interface ICSharpDumper : INotifyPropertyChanged
{
    string OutputText { get; }

    CompilerMessageList CompilerMessages { get; }

    /// <summary>
    /// C#コードをダンプします。
    /// </summary>
    /// <param name="sourceCode">ソースコード</param>
    /// <param name="compilerOptions">コンパイラーオプション</param>
    /// <param name="cancellationToken">キャンセル要求を行うためのトークン</param>
    /// <returns>
    /// このメソッドが完了すると、ダンプに成功した場合は<see langword="true"/>を返します。
    /// 失敗した場合は<see langword="false"/>を返します。
    /// </returns>
    ValueTask DumpAsync(string sourceCode, CompilerOptions compilerOptions, CancellationToken cancellationToken = default);
}
