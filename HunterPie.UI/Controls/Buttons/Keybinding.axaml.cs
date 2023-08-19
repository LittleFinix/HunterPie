using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for Keybinding.xaml
/// </summary>
public partial class Keybinding : UserControl
{
    public ObservableCollection<string> Keys { get; } = new();

    public string HotKey
    {
        get => (string)GetValue(HotKeyProperty);
        set => SetValue(HotKeyProperty, value);
    }

    public static readonly StyledProperty<string> HotKeyProperty =
        AvaloniaProperty.Register<Keybinding, string>(nameof(HotKey));

    public Keybinding()
    {
        InitializeComponent();
    }

    private void OnClick() => Focus();

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Keys.Clear();

        if (HotKey is null)
            return;

        foreach (string key in HotKey.Split("+"))
            Keys.Add(key);
    }

    private void InputElement_OnKeyDown(object? sender, KeyEventArgs e)
    {
        e.Handled = true;

        Key key = e.Key;

        if (key is Key.LeftShift or Key.RightShift
            or Key.LeftCtrl or Key.RightCtrl
            or Key.LeftAlt or Key.RightAlt
            or Key.LWin or Key.RWin)
            return;

        Keys.Clear();

        if (key == Key.Delete)
        {
            Keys.Add("None");
            SetValue(HotKeyProperty, "None");
            return;
        }

        var shortcutText = new StringBuilder();
        if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            Keys.Add("Ctrl");
            _ = shortcutText.Append("Ctrl+");
        }
        
        if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
        {
            Keys.Add("Shift");
            _ = shortcutText.Append("Shift+");
        }

        if (e.KeyModifiers.HasFlag(KeyModifiers.Alt))
        {
            Keys.Add("Alt");
            _ = shortcutText.Append("Alt+");
        }

        Keys.Add(key.ToString());
        _ = shortcutText.Append(key.ToString());
        SetValue(HotKeyProperty, shortcutText.ToString());
    }
}
