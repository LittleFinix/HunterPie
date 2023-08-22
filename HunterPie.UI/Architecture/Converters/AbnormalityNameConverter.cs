using Avalonia.Data.Converters;
using HunterPie.Core.Client.Localization;
using System;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class AbnormalityNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => Localization.FindString("Abnormalities", "Abnormality", $"{value}");

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
