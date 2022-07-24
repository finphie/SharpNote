namespace SharpNote.Core;

/// <summary>
/// テキストの範囲を表します。
/// </summary>
/// <param name="Start">開始位置</param>
/// <param name="End">終了位置</param>
public readonly record struct TextPositionRange(TextPosition Start, TextPosition End);
