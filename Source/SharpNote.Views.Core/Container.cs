using FToolkit.Diagnostics;
using FToolkit.IO;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpNote.Core;
using SharpNote.Models;
using SharpNote.Models.Handlers;
using SharpNote.Services;
using SharpNote.ViewModels;
using StrongInject;

namespace SharpNote.Views.Core;

[Register<FileOperations, IFileOperations>(Scope.SingleInstance)]
[Register<SystemOperations, ISystemOperations>(Scope.SingleInstance)]
[Register<CompilationService, ICompilationService>(Scope.SingleInstance)]
[Register<AssemblyDumpService, IAssemblyDumpService>(Scope.SingleInstance)]
[Register<CompletionService, ICompletionService>(Scope.SingleInstance)]

//[Register<CompletionRepository>(Scope.SingleInstance)]

[Register<ShellViewModel>(Scope.SingleInstance)]
[Register<EditorViewModel>(Scope.SingleInstance)]

[Register<CSharpDumper, ICSharpDumper>(Scope.SingleInstance)]

[Register<MessagePipeDiagnosticsInfo>(Scope.SingleInstance)]
[Register<MessagePipeOptions>(Scope.SingleInstance)]

[Register<FilterAttachedMessageHandlerFactory>(Scope.SingleInstance)]
[Register<AttributeFilterProvider<MessageHandlerFilterAttribute>>(Scope.SingleInstance)]

[Register<FilterAttachedAsyncMessageHandlerFactory>(Scope.SingleInstance)]
[Register<AttributeFilterProvider<AsyncMessageHandlerFilterAttribute>>(Scope.SingleInstance)]
[Register(typeof(MessageBrokerCore<>), Scope.SingleInstance)]
[Register(typeof(MessageBroker<>), Scope.SingleInstance, typeof(ISubscriber<>), typeof(IPublisher<>))]
[Register(typeof(AsyncMessageBrokerCore<>), Scope.SingleInstance)]
[Register(typeof(AsyncMessageBroker<>), Scope.SingleInstance, typeof(IAsyncSubscriber<>), typeof(IAsyncPublisher<>))]

[Register<CompletionHandler, IAsyncRequestHandler<CaretPosition, CompletionList>>(Scope.SingleInstance)]

public partial class Container : IContainer<ShellViewModel>, IContainer<EditorViewModel>
{
    readonly IServiceProvider _serviceProvider;

    public Container(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    [FactoryOf(typeof(ILogger<>))]
    [FactoryOf(typeof(IOptions<>))]
    [FactoryOf(typeof(IServiceProvider))]
    T GetService<T>() => _serviceProvider.GetRequiredService<T>();
}
