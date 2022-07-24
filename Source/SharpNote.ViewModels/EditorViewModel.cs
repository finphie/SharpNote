using System.Reactive.Linq;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using MessagePipe;
using Reactive.Bindings.Extensions;
using SharpNote.Core;
using SharpNote.Models;
using SharpNote.ViewModels.Core;

namespace SharpNote.ViewModels;

/// <summary>
/// Editor ViewModel
/// </summary>
public sealed partial class EditorViewModel : ViewModelBase, IDisposable
{
    static readonly TimeSpan DueTime = TimeSpan.FromMilliseconds(500);

    readonly ISubscriber<CompilerOptions> _compilerOptionSubscriber;
    readonly IAsyncRequestHandler<CaretPosition, CompletionList> _handler;

    readonly ICSharpDumper _csharpDumper;

    readonly IDisposable _disposable;

    [ObservableProperty]
    string _editorText = string.Empty;

    [ObservableProperty]
    string _viewerText = string.Empty;

    public EditorViewModel(
        ISubscriber<CompilerOptions> compilerOptionSubscriber,
        IAsyncRequestHandler<CaretPosition, CompletionList> handler,
        ICSharpDumper csharpDumper)
    {
        ArgumentNullException.ThrowIfNull(csharpDumper);
        ArgumentNullException.ThrowIfNull(compilerOptionSubscriber);

        _csharpDumper = csharpDumper;

        _compilerOptionSubscriber = compilerOptionSubscriber;
        _handler = handler;

        var bag = DisposableBag.CreateBuilder();

        // エディターまたはコンパイラーオプションに変化があった場合、
        // コードのダンプを実行する。
        this.ObserveProperty(x => x.EditorText)
            .Throttle(DueTime)
            .CombineLatest(_compilerOptionSubscriber.AsObservable().StartWith(CompilerOptions.Default))
            .Subscribe(value =>
            {
                var (sourceCode, compilerOptions) = value;
                _csharpDumper.DumpAsync(sourceCode, compilerOptions)
                    .SafeFireAndForget();
            })
            .AddTo(bag);

        _csharpDumper.ObserveProperty(static x => x.OutputText)
            .Subscribe(x => ViewerText = x)
            .AddTo(bag);

        _disposable = bag.Build();
    }

    /// <inheritdoc/>
    public void Dispose() => _disposable?.Dispose();

    // TODO: Modelへ移動
    public ValueTask<CompletionList> GetCompletionList(string text, int position, CancellationToken cancellationToken = default)
        => _handler.InvokeAsync(new(text, position), cancellationToken);
}
