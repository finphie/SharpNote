using System.Buffers;
using System.Collections;

namespace SharpNote.Core;

/// <summary>
/// コンパイラメッセージのリストを表す構造体です。
/// </summary>
/// <param name="Messages">コンパイラメッセージ</param>
public readonly record struct CompilerMessageList(IReadOnlyList<CompilerMessage> Messages)
    : IReadOnlyList<CompilerMessage>, ISpanFormattable
{
    /// <summary>
    /// 空を表す<see cref="CompilerMessage"/>インスタンスを取得します。
    /// </summary>
    /// <value>
    /// <see cref="CompilerMessage"/>構造体の空配列で初期化したインスタンスを返します。
    /// </value>
    public static CompilerMessageList Empty => EmptyArray.Value;

    /// <inheritdoc/>
    public int Count => Messages.Count;

    /// <inheritdoc/>
    public CompilerMessage this[int index] => Messages[index];

    /// <inheritdoc/>
    public IEnumerator<CompilerMessage> GetEnumerator() => Messages.GetEnumerator();

    /// <inheritdoc/>
    public override string ToString() => ToString(null);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        // TODO: 出力サイズ上限撤廃
        var buffer = ArrayPool<char>.Shared.Rent(1024);
        TryFormat(buffer, out var charsWritten);

        var result = buffer.AsSpan(0, charsWritten).ToString();
        ArrayPool<char>.Shared.Return(buffer);

        return result;
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        charsWritten = 0;

        foreach (var message in Messages)
        {
            if (!destination.TryWrite(provider, $"{message}\n", out var written))
            {
                return false;
            }

            destination = destination[written..];
            charsWritten += written;
        }

        return true;
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => Messages.GetEnumerator();

    static class EmptyArray
    {
        public static readonly CompilerMessageList Value = new(Array.Empty<CompilerMessage>());
    }
}
