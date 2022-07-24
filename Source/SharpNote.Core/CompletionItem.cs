namespace SharpNote.Core;

/// <summary>
/// 入力補完候補を表す構造体です。
/// </summary>
/// <param name="Label">ラベル</param>
/// <param name="InsertText">補完選択時に挿入する文字列</param>
/// <param name="Kind">種類</param>
/// <param name="Detail">追加情報</param>
/// <param name="Documentation">ドキュメンテーションコメント</param>
public readonly record struct CompletionItem(
    string Label,
    string InsertText,
    CompletionItemKind Kind,
    string Detail,
    string Documentation);
