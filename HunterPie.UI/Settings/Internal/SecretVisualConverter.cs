using HunterPie.Core.Settings.Types;
using HunterPie.UI.Controls.TextBox;
using HunterPie.UI.Settings.Converter;
using System.Reflection;

namespace HunterPie.UI.Settings.Internal;

internal class SecretVisualConverter : IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var observable = (Secret)childInfo.GetValue(parent);
        SecretTextBox textbox = new() { [SecretTextBox.TextProperty] = observable };

        return textbox;
    }
}
