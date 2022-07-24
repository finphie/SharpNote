namespace SharpNote.Core;

/// <summary>
/// コンパイラーオプションを表す構造体です。
/// </summary>
/// <param name="Platform">プラットフォーム</param>
/// <param name="Runtime">ランタイム</param>
public readonly record struct CompilerOptions(PlatformOptions Platform, RuntimeOptions Runtime)
{
    /// <summary>
    /// デフォルトのコンパイラーオプションを取得します。
    /// </summary>
    /// <value>
    /// <see cref="PlatformOptions.AnyCpu"/>と<see cref="RuntimeOptions.None"/>を設定した
    /// <see cref="CompilerOptions"/>構造体のインスタンスを返します。
    /// </value>
    public static CompilerOptions Default => new(PlatformOptions.AnyCpu, RuntimeOptions.None);
}
