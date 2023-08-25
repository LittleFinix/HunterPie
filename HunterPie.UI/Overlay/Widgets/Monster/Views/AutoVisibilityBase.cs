using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Data;
using System;

namespace HunterPie.UI.Overlay.Widgets.Monster.Views;

[PseudoClasses(":active")]
public class AutoVisibilityBase : UserControl
{
    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<AutoVisibilityBase, bool>(
        "IsActive");

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    
    protected override void OnInitialized()
    {
        base.OnInitialized();

        this[!IsActiveProperty] = new Binding("IsActive"); 
        
        PseudoClasses.Set(":active", this.GetObservable(IsActiveProperty));
    }
}