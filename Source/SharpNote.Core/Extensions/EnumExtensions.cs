using System.Runtime.CompilerServices;

namespace SharpNote.Core.Extensions;

/// <summary>
/// 列挙型の拡張メソッド集です。
/// </summary>
static class EnumExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetName(this CompilerMessageSeverity severity, Span<char> destination, out int charsWritten)
    {
        const string Hidden = nameof(CompilerMessageSeverity.Hidden);
        const string Info = nameof(CompilerMessageSeverity.Info);
        const string Warning = nameof(CompilerMessageSeverity.Warning);
        const string Error = nameof(CompilerMessageSeverity.Error);

        if (severity == CompilerMessageSeverity.Hidden && Hidden.TryCopyTo(destination))
        {
            charsWritten = Hidden.Length;
            return true;
        }

        if (severity == CompilerMessageSeverity.Info && Info.TryCopyTo(destination))
        {
            charsWritten = Info.Length;
            return true;
        }

        if (severity == CompilerMessageSeverity.Warning && Warning.TryCopyTo(destination))
        {
            charsWritten = Warning.Length;
            return true;
        }

        if (severity == CompilerMessageSeverity.Error && Error.TryCopyTo(destination))
        {
            charsWritten = Error.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }
}
