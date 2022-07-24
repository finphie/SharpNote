using SharpNote.Core;

namespace SharpNote.Services;

/// <summary>
/// コード補完候補の取得処理に関する定義です。
/// </summary>
public interface ICompletionService
{
    /// <summary>
    /// コード補完候補の一覧を取得します。
    /// </summary>
    /// <param name="text">文字列</param>
    /// <param name="position">位置</param>
    /// <param name="cancellationToken">キャンセル要求を行うためのトークン</param>
    /// <returns>このメソッドが完了すると、コード補完候補の一覧を返します。</returns>
    ValueTask<CompletionList> ExecuteAsync(string text, int position, CancellationToken cancellationToken = default);
}
