namespace SharpNote.Core;

/// <summary>
/// キャレットの位置を表す構造体です。
/// </summary>
/// <param name="Text"></param>
/// <param name="Position"></param>
public readonly record struct CaretPosition(string Text, int Position);
