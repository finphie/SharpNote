namespace SharpNote.Core;

/// <summary>
/// コンパイル結果を表す構造体です。
/// </summary>
/// <param name="RawAssembly">アセンブリデータ</param>
/// <param name="Messages">コンパイラのメッセージリスト</param>
public readonly record struct CompileResult(ReadOnlyMemory<byte> RawAssembly, CompilerMessageList Messages);
