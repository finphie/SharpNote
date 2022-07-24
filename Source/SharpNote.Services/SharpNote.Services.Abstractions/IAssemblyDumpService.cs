using System.Buffers;
using SharpNote.Core;

namespace SharpNote.Services;

/// <summary>
/// アセンブリのダンプ処理に関する定義です。
/// </summary>
public interface IAssemblyDumpService
{
    /// <summary>
    /// アセンブリをダンプします。
    /// </summary>
    /// <param name="bufferWriter"><see cref="char"/>データを書き込むことができる出力シンク</param>
    /// <param name="assemblyFilePath">アセンブリファイルのパス</param>
    /// <param name="runtimeOptions">ランタイムの構成オプション</param>
    /// <param name="cancellationToken">キャンセル要求を行うためのトークン</param>
    /// <returns>
    /// このメソッドが完了すると、ダンプに成功した場合は<see langword="true"/>を返します。
    /// 失敗した場合は<see langword="false"/>を返します。
    /// </returns>
    ValueTask<bool> TryExecuteAsync(
        IBufferWriter<char> bufferWriter,
        string assemblyFilePath,
        RuntimeOptions runtimeOptions,
        CancellationToken cancellationToken = default);
}
