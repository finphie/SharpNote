using CommunityToolkit.HighPerformance.Buffers;
using CommunityToolkit.Mvvm.ComponentModel;
using FToolkit.IO;
using Microsoft.Extensions.Logging;
using PathUtility;
using SharpNote.Core;
using SharpNote.Services;
using Utf8Utility;

namespace SharpNote.Models;

public sealed partial class CSharpDumper : ObservableObject, ICSharpDumper
{
    readonly ILogger<CSharpDumper> _logger;
    readonly IFileOperations _fileOperations;
    readonly ICompilationService _compilationService;
    readonly IAssemblyDumpService _assemblyDumpService;

    [ObservableProperty]
    string _outputText = string.Empty;

    [ObservableProperty]
    CompilerMessageList _compilerMessages = CompilerMessageList.Empty;

    /// <summary>
    /// <see cref="CSharpDumper"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="fileOperations">ファイル操作を行うオブジェクト</param>
    /// <param name="compilationService">コンパイルを実行するオブジェクト</param>
    /// <param name="disassembleService">アセンブリのダンプを実行するオブジェクト</param>
    public CSharpDumper(ILogger<CSharpDumper> logger, IFileOperations fileOperations, ICompilationService compilationService, IAssemblyDumpService disassembleService)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(fileOperations);
        ArgumentNullException.ThrowIfNull(compilationService);
        ArgumentNullException.ThrowIfNull(disassembleService);

        _logger = logger;
        _fileOperations = fileOperations;
        _compilationService = compilationService;
        _assemblyDumpService = disassembleService;
    }

    /// <inheritdoc/>
    public async ValueTask DumpAsync(string sourceCode, CompilerOptions compilerOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);

        StartingDump();

        var sourceCodePath = ZPath.GetTempFilePath(".cs.tmp");
        var compileResult = await _compilationService.ExecuteAsync(sourceCodePath, sourceCode, compilerOptions.Platform, cancellationToken)
            .ConfigureAwait(false);
        CompilerMessages = compileResult.Messages;

        // コンパイル失敗
        if (compileResult.RawAssembly.IsEmpty)
        {
            CompileError();

            OutputText = CompilerMessages.ToString();
            return;
        }

        var assemblyFilePath = ZPath.GetTempFilePath(".dll.tmp");
        var utf8SourceCode = new Utf8Array(sourceCode);

        try
        {
            _fileOperations.Save(sourceCodePath, utf8SourceCode.AsSpan());
            _fileOperations.Save(assemblyFilePath, compileResult.RawAssembly.Span);
        }
        catch
        {
            OutputText = string.Empty;
            return;
        }

        using var bufferWriter = new ArrayPoolBufferWriter<char>();
        var success = false;

        try
        {
            success = await _assemblyDumpService.TryExecuteAsync(bufferWriter, assemblyFilePath, compilerOptions.Runtime, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            _fileOperations.Delete(sourceCodePath);
            _fileOperations.Delete(assemblyFilePath);
        }

        if (!success || bufferWriter.WrittenCount == 0)
        {
            DumpError();

            OutputText = "ダンプに失敗しました。";
            return;
        }

        OutputText = bufferWriter.WrittenSpan.ToString();
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = $"{nameof(CSharpDumper)}.{nameof(DumpAsync)} is starting.")]
    partial void StartingDump();

    [LoggerMessage(EventId = 10000, Level = LogLevel.Warning, Message = "compile error")]
    partial void CompileError();

    [LoggerMessage(EventId = 10001, Level = LogLevel.Warning, Message = "dump error")]
    partial void DumpError();
}
