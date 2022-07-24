using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Tags;
using Microsoft.CodeAnalysis.Text;
using SharpNote.Core;
using CompletionItem = SharpNote.Core.CompletionItem;
using RoslynCompletionService = Microsoft.CodeAnalysis.Completion.CompletionService;

namespace SharpNote.Services;

/// <summary>
/// コード補完候補を取得するクラスです。
/// </summary>
public sealed class CompletionService : ICompletionService, IDisposable
{
    readonly AdhocWorkspace _workspace;
    readonly Project _project;

    public CompletionService()
    {
        // Lazy<Workspace>(() => new AdhocWorkspace());
        var host = MefHostServices.Create(MefHostServices.DefaultAssemblies);
        _workspace = new AdhocWorkspace(host);

        var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "MyProject", "MyProject", LanguageNames.CSharp)
           .WithMetadataReferences(new[]
           {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
           });
        _project = _workspace.AddProject(projectInfo);
    }

    /// <inheritdoc/>
    public void Dispose() => _workspace.Dispose();

    /// <inheritdoc/>
    public async ValueTask<CompletionList> ExecuteAsync(string text, int position, CancellationToken cancellationToken = default)
    {
        var document = _workspace.AddDocument(_project.Id, "MyFile.cs", SourceText.From(text));

        var completionService = RoslynCompletionService.GetService(document);
        var results = await completionService.GetCompletionsAsync(document, position, cancellationToken: cancellationToken).ConfigureAwait(false);

        if (results is null || results.Items.Length == 0)
        {
            return CompletionList.Empty;
        }

        var items = results.Items
            .Select(item =>
            {
                if (!item.Properties.TryGetValue("SymbolName", out var symbolName))
                {
                    symbolName = item.DisplayText;
                }

                if (!item.Properties.TryGetValue("InsertionText", out var insertionText))
                {
                    insertionText = symbolName;
                }

                if (!item.Properties.TryGetValue("SymbolKind", out var symbolKind) && !item.Tags.IsDefaultOrEmpty)
                {
                    symbolKind = item.Tags[0];
                }

                var kind = ConvertKind(symbolKind);

                return new CompletionItem
                {
                    Label = symbolName,
                    InsertText = insertionText,
                    Kind = kind,
                    Detail = string.Empty,
                    Documentation = string.Empty
                };
            })
            .DistinctBy(x => x.Label)
            .ToArray();

        return new(items);
    }

    static CompletionItemKind ConvertKind(string? kind)
    {
        // https://github.com/dotnet/roslyn/blob/v4.1.0/src/Features/LanguageServer/Protocol/Extensions/ProtocolConversions.cs
        return kind switch
        {
            WellKnownTags.Public => CompletionItemKind.Keyword,
            WellKnownTags.Protected => CompletionItemKind.Keyword,
            WellKnownTags.Private => CompletionItemKind.Keyword,
            WellKnownTags.Internal => CompletionItemKind.Keyword,
            WellKnownTags.File => CompletionItemKind.File,
            WellKnownTags.Project => CompletionItemKind.File,
            WellKnownTags.Folder => CompletionItemKind.Folder,
            WellKnownTags.Assembly => CompletionItemKind.File,
            WellKnownTags.Class => CompletionItemKind.Class,
            WellKnownTags.Constant => CompletionItemKind.Constant,
            WellKnownTags.Delegate => CompletionItemKind.Method,
            WellKnownTags.Enum => CompletionItemKind.Enum,
            WellKnownTags.EnumMember => CompletionItemKind.EnumMember,
            WellKnownTags.Event => CompletionItemKind.Event,
            WellKnownTags.ExtensionMethod => CompletionItemKind.Method,
            WellKnownTags.Field => CompletionItemKind.Field,
            WellKnownTags.Interface => CompletionItemKind.Interface,
            WellKnownTags.Intrinsic => CompletionItemKind.Text,
            WellKnownTags.Keyword => CompletionItemKind.Keyword,
            WellKnownTags.Label => CompletionItemKind.Text,
            WellKnownTags.Local => CompletionItemKind.Variable,
            WellKnownTags.Namespace => CompletionItemKind.Text,
            WellKnownTags.Method => CompletionItemKind.Method,
            WellKnownTags.Module => CompletionItemKind.Module,
            WellKnownTags.Operator => CompletionItemKind.Operator,
            WellKnownTags.Parameter => CompletionItemKind.Value,
            WellKnownTags.Property => CompletionItemKind.Property,
            WellKnownTags.RangeVariable => CompletionItemKind.Variable,
            WellKnownTags.Reference => CompletionItemKind.Reference,
            WellKnownTags.Structure => CompletionItemKind.Struct,
            WellKnownTags.TypeParameter => CompletionItemKind.TypeParameter,
            WellKnownTags.Snippet => CompletionItemKind.Snippet,
            WellKnownTags.Error => CompletionItemKind.Text,
            WellKnownTags.Warning => CompletionItemKind.Text,
            _ => CompletionItemKind.Text
        };
    }
}
