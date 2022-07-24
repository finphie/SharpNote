namespace SharpNote.Core;

/// <summary>
/// ランタイムの構成オプション
/// <see href="https://docs.microsoft.com/dotnet/core/run-time-config/compilation"/>
/// </summary>
[Flags]
public enum RuntimeOptions
{
    /// <summary>
    /// なし
    /// </summary>
    None = 0,

    /// <summary>
    /// 階層型コンパイル
    /// </summary>
    TieredCompilation = 1 << 0,

    /// <summary>
    /// クイックJIT
    /// </summary>
    TCQuickJit = 1 << 1,

    /// <summary>
    /// ループに対するクイックJIT
    /// </summary>
    TCQuickJitForLoops = 1 << 2,

    /// <summary>
    /// プリコンパイル
    /// </summary>
    ReadyToRun = 1 << 3,

    /// <summary>
    /// ガイド付き最適化のプロファイル
    /// </summary>
    TieredPgo = 1 << 4,

    /// <summary>
    /// 動的PGO
    /// </summary>
    DynamicPgo = TieredCompilation | TCQuickJitForLoops | TieredPgo
}
