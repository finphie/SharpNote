using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using SharpNote.Core;

namespace SharpNote.Services;

/// <summary>
/// コンパイルを実行するクラスです。
/// </summary>
public sealed partial class CompilationService : ICompilationService
{
    const string GeneratedAssemblyName = "SharpNoteGenerated.dll";
    const string GeneratedPdbName = "SharpNoteGenerated.pdb";

    static readonly Assembly ObjectAssembly = typeof(object).Assembly;

    static readonly MetadataReference[] MetadataReferences = GetMetadataReferences().ToArray();

    readonly ILogger _logger;

    /// <summary>
    /// <see cref="CompilationService"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    public CompilationService(ILogger<CompilationService> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public ValueTask<CompileResult> ExecuteAsync(string sourceCodePath, string sourceCode, PlatformOptions platform, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(sourceCodePath);
        ArgumentNullException.ThrowIfNull(sourceCode);

        Starting();
        Parameters(sourceCodePath, platform);

        var sourceText = SourceText.From(sourceCode, Encoding.UTF8);
        var compilation = CreateCompilation(sourceCodePath, sourceText, platform, cancellationToken);
        var emitOptions = new EmitOptions(
              debugInformationFormat: DebugInformationFormat.Embedded,
              pdbFilePath: GeneratedPdbName);

        using var writer = new ArrayPoolBufferWriter<byte>();

        // 書き込み専用Stream
        using var writeStream = writer.AsStream();

        var result = compilation.Emit(writeStream, options: emitOptions, cancellationToken: cancellationToken);
        byte[] rawAssembly;

        if (result.Success)
        {
            // 読み取り専用Stream
            using var stream = writer.WrittenMemory.AsStream();

            // アセンブリデータが書き込まれた後なので、初期位置に戻す。
            stream.Seek(0, SeekOrigin.Begin);

            rawAssembly = GC.AllocateUninitializedArray<byte>((int)stream.Length);
            stream.Read(rawAssembly);
        }
        else
        {
            rawAssembly = Array.Empty<byte>();
        }

        var messages = GetCompilerMessages(result.Diagnostics);
        return ValueTask.FromResult(new CompileResult(rawAssembly, new(messages)));
    }

    static CSharpCompilation CreateCompilation(string sourceCodePath, SourceText sourceText, PlatformOptions platform, CancellationToken cancellationToken)
    {
        var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);
        var compilationPlatform = GetCompilationPlatform(platform);
        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithAllowUnsafe(true)
            .WithOptimizationLevel(OptimizationLevel.Release)
            .WithPlatform(compilationPlatform);

        var syntaxTree = CSharpSyntaxTree.ParseText(sourceText, parseOptions, sourceCodePath, cancellationToken: cancellationToken);

        // TODO: 構文エラー表示
        // syntaxTree.GetDiagnostics()
        var compilation = CSharpCompilation.Create(GeneratedAssemblyName, new[] { syntaxTree }, MetadataReferences, compilationOptions);

        return compilation;
    }

    static CompilerMessage[] GetCompilerMessages(IReadOnlyList<Diagnostic> diagnostics)
    {
        var messages = diagnostics
            .OrderBy(static x => x.Location.SourceSpan.Start)
            .Select(static diagnostic =>
            {
                var severity = GetCompilerMessageSeverity(diagnostic.Severity);
                var lineSpan = diagnostic.Location.GetMappedLineSpan().Span;
                var start = new TextPosition(lineSpan.Start.Line, lineSpan.Start.Character);
                var end = new TextPosition(lineSpan.End.Line, lineSpan.End.Character);

                return new CompilerMessage(diagnostic.Id, severity, new(start, end), diagnostic.GetMessage());
            })
            .ToArray();

        return messages;
    }

    static IEnumerable<MetadataReference> GetMetadataReferences()
    {
        // TODO: ユーザーが指定したdllを読み込む
        var assemblyPath = Path.GetDirectoryName(ObjectAssembly.Location)!;
        var files = Directory.EnumerateFiles(assemblyPath, "*.dll");

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);

            if (fileName.StartsWith("System.", StringComparison.Ordinal) || fileName.StartsWith("Microsoft.", StringComparison.Ordinal))
            {
                // ネイティブファイルは除外する
                if (fileName.Contains("Native", StringComparison.Ordinal))
                {
                    continue;
                }

                yield return MetadataReference.CreateFromFile(file);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static Platform GetCompilationPlatform(PlatformOptions platform)
    {
        return platform switch
        {
            PlatformOptions.None or PlatformOptions.AnyCpu => Platform.AnyCpu,
            PlatformOptions.X64 => Platform.X64,
            PlatformOptions.X86 => Platform.X86,
            PlatformOptions.Arm64 => Platform.Arm64,
            _ => throw new ArgumentOutOfRangeException(nameof(platform))
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static CompilerMessageSeverity GetCompilerMessageSeverity(DiagnosticSeverity severity)
    {
        return severity switch
        {
            DiagnosticSeverity.Hidden => CompilerMessageSeverity.Hidden,
            DiagnosticSeverity.Info => CompilerMessageSeverity.Info,
            DiagnosticSeverity.Warning => CompilerMessageSeverity.Warning,
            DiagnosticSeverity.Error => CompilerMessageSeverity.Error,
            _ => throw new ArgumentOutOfRangeException(nameof(severity))
        };
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = $"{nameof(CompilationService)} is starting.")]
    partial void Starting();

    [LoggerMessage(EventId = 2, Level = LogLevel.Trace, Message = "Path: {sourceCodePath}, Platform: {platform}")]
    partial void Parameters(string sourceCodePath, PlatformOptions platform);
}
