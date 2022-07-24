using SharpNote.Core.Extensions;

namespace SharpNote.Core;

/// <summary>
/// コンパイラメッセージを表す構造体です。
/// </summary>
/// <param name="Id">ID</param>
/// <param name="Severity">コンパイラメッセージのレベル</param>
/// <param name="TextRange">範囲</param>
/// <param name="Message">メッセージ</param>
public readonly record struct CompilerMessage(string Id, CompilerMessageSeverity Severity, TextPositionRange TextRange, string Message)
    : ISpanFormattable
{
    const int BufferSize = 256;

    /// <inheritdoc/>
    public override string ToString() => ToString(null, null);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => string.Create(formatProvider, stackalloc char[BufferSize], $"{TextRange.Start} {Enum.GetName(Severity)} {Id}: {Message}");

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        if (!destination.TryWrite(provider, $"{TextRange.Start} ", out charsWritten))
        {
            return false;
        }

        destination = destination[charsWritten..];

        if (!Severity.TryGetName(destination, out var tmpCharsWritten))
        {
            return false;
        }

        charsWritten += tmpCharsWritten;
        destination = destination[tmpCharsWritten..];

        if (!destination.TryWrite(provider, $" {Id}: {Message}", out tmpCharsWritten))
        {
            return false;
        }

        charsWritten += tmpCharsWritten;
        return true;
    }
}
