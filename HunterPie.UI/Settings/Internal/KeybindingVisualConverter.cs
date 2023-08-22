using Avalonia.Data;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using KeybindingControl = HunterPie.UI.Controls.Buttons.Keybinding;

namespace HunterPie.UI.Settings.Internal;

public class KeybindingVisualConverter : IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var key = (Keybinding)childInfo.GetValue(parent);
        Binding binding = VisualConverterHelper.CreateBinding(key, nameof(Keybinding.KeyCombo));

        KeybindingControl control = new() { [KeybindingControl.HotKeyProperty] = binding };

        return control;
    }
}
