namespace SharpNote.Core;

/// <summary>
/// テキストの位置を表します。
/// </summary>
/// <param name="Line">行の位置</param>
/// <param name="Offset">列の位置</param>
public readonly record struct TextPosition(int Line, int Offset) : ISpanFormattable
{
    const int BufferSize = 16;

    /// <inheritdoc/>
    public override string ToString() => ToString(null, null);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => string.Create(formatProvider, stackalloc char[BufferSize], $"({Line}, {Offset})");

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => destination.TryWrite(provider, $"({Line}, {Offset})", out charsWritten);
}
