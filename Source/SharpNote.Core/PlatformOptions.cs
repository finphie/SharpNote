namespace SharpNote.Core;

/// <summary>
/// プラットフォームの構成オプション
/// </summary>
public enum PlatformOptions
{
    /// <summary>
    /// なし
    /// </summary>
    None,

    /// <summary>
    /// 任意のプラットフォーム
    /// </summary>
    AnyCpu,

    /// <summary>
    /// 64bit
    /// </summary>
    X64,

    /// <summary>
    /// 32bit
    /// </summary>
    X86,

    /// <summary>
    /// ARM 64bit
    /// </summary>
    Arm64
}
