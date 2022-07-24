using Microsoft.Extensions.DependencyInjection;
using SharpNote.ViewModels;
using StrongInject.Extensions.DependencyInjection;

namespace SharpNote.Views.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSharpNote(this IServiceCollection services)
    {
        services.AddSingleton<Container>();

        services.AddSingletonServiceUsingContainer<Container, ShellViewModel>();
        services.AddSingletonServiceUsingContainer<Container, EditorViewModel>();
    }
}
