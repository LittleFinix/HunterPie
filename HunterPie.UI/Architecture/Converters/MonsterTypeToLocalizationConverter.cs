﻿using Avalonia.Data.Converters;
using HunterPie.Core.Client.Localization;
using System;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class MonsterTypeToLocalizationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string id)
            return Localization.FindString("Monsters", "Types", "Type", id);

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}