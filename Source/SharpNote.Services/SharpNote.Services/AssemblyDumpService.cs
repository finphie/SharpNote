using System.Buffers;
using System.Runtime.CompilerServices;
using FToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpNote.Core;

namespace SharpNote.Services;

/// <summary>
/// アセンブリのダンプを実行するクラスです。
/// </summary>
public sealed partial class AssemblyDumpService : IAssemblyDumpService
{
    readonly ILogger _logger;
    readonly AssemblyDumpServiceSettings _settings;
    readonly ISystemOperations _systemOperations;

    /// <summary>
    /// <see cref="AssemblyDumpService"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="settings">設定</param>
    /// <param name="systemOperations">システムに関する操作を行うオブジェクト</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logger"/>または<paramref name="settings"/>、<paramref name="systemOperations"/>がnullです。
    /// </exception>
    public AssemblyDumpService(
        ILogger<AssemblyDumpService> logger,
        IOptions<AssemblyDumpServiceSettings> settings,
        ISystemOperations systemOperations)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(systemOperations);

        _logger = logger;
        _settings = settings.Value;
        _systemOperations = systemOperations;
    }

    /// <inheritdoc/>
    public async ValueTask<bool> TryExecuteAsync(
        IBufferWriter<char> bufferWriter,
        string assemblyFilePath,
        RuntimeOptions runtimeOptions,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bufferWriter);
        ArgumentNullException.ThrowIfNull(assemblyFilePath);

        Starting();

        var environment = CreateEnvironment(runtimeOptions);
        var arg = "--diffable -l " + assemblyFilePath;

        var exitCode = await _systemOperations.WaitProcessAsync(
            bufferWriter,
            _settings.DisassemblerPath,
            arg,
            environmentVariable: environment,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return exitCode == 0;
    }

    static Dictionary<string, string> CreateEnvironment(RuntimeOptions runtimeOptions)
    {
        return new()
        {
            { "DOTNET_TieredCompilation", GetValue(RuntimeOptions.TieredCompilation) },
            { "DOTNET_TC_QuickJit", GetValue(RuntimeOptions.TCQuickJit) },
            { "DOTNET_TC_QuickJitForLoops", GetValue(RuntimeOptions.TCQuickJitForLoops) },
            { "DOTNET_ReadyToRun", GetValue(RuntimeOptions.ReadyToRun) },
            { "DOTNET_TieredPGO", GetValue(RuntimeOptions.TieredPgo) }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string GetValue(RuntimeOptions flag)
            => runtimeOptions.HasFlag(flag) ? "1" : "0";
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = $"{nameof(AssemblyDumpService)} is starting.")]
    partial void Starting();
}
