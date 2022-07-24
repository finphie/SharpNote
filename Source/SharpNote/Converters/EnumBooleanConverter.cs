using System.Globalization;
using System.Windows.Data;

namespace SharpNote.Converters;

/// <summary>
/// bool型を列挙型に変換するコンバーターです。
/// </summary>
[ValueConversion(typeof(bool), typeof(Enum))]
sealed class EnumBooleanConverter : IValueConverter
{
    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.Equals(parameter);

    /// <inheritdoc/>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.Equals(true) == true ? parameter : Binding.DoNothing;
}
