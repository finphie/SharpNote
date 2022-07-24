using System.Collections;

namespace SharpNote.Core;

/// <summary>
/// 入力補完候補のリストを表す構造体です。
/// </summary>
/// <param name="Items">入力補完候補の一覧</param>
public readonly record struct CompletionList(IReadOnlyList<CompletionItem> Items)
    : IReadOnlyList<CompletionItem>
{
    /// <summary>
    /// 空を表す<see cref="CompletionList"/>インスタンスを取得します。
    /// </summary>
    /// <value>
    /// <see cref="CompletionItem"/>構造体の空配列で初期化したインスタンスを返します。
    /// </value>
    public static CompletionList Empty => EmptyArray.Value;

    /// <inheritdoc/>
    public int Count => Items.Count;

    /// <inheritdoc/>
    public CompletionItem this[int index] => Items[index];

    /// <inheritdoc/>
    public IEnumerator<CompletionItem> GetEnumerator() => Items.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

    static class EmptyArray
    {
        public static readonly CompletionList Value = new(Array.Empty<CompletionItem>());
    }
}
