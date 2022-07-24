namespace SharpNote.Services;

public sealed record AssemblyDumpServiceSettings
{
    public const string ConfigurationSectionName = nameof(AssemblyDumpService);

    public string DisassemblerPath { get; init; }
}
