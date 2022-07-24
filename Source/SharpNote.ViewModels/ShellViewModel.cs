using CommunityToolkit.Mvvm.ComponentModel;
using MessagePipe;
using SharpNote.Core;
using SharpNote.ViewModels.Core;

namespace SharpNote.ViewModels;

/// <summary>
/// Shell ViewModel
/// </summary>
public sealed partial class ShellViewModel : ViewModelBase
{
    readonly IPublisher<CompilerOptions> _compilerOptionsPublisher;

    readonly RuntimeOptions _runtime = RuntimeOptions.TieredCompilation;

    [ObservableProperty]
    PlatformOptions _platform = PlatformOptions.X64;

    public ShellViewModel(IPublisher<CompilerOptions> compilerOptionsPublisher)
    {
        ArgumentNullException.ThrowIfNull(compilerOptionsPublisher);

        _compilerOptionsPublisher = compilerOptionsPublisher;
    }

    partial void OnPlatformChanged(PlatformOptions value)
        => _compilerOptionsPublisher.Publish(new(value, _runtime));
}
