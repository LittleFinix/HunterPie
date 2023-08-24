using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;

namespace HunterPie.UI.Overlay.Widgets.Monster.Views;

public class BossMonsterPartViewBase : AutoVisibilityBase
{
    public static readonly StyledProperty<bool> IsPartBrokenProperty = AvaloniaProperty.Register<BossMonsterPartViewBase, bool>(
        "IsPartBroken");

    public bool IsPartBroken
    {
        get => GetValue(IsPartBrokenProperty);
        set => SetValue(IsPartBrokenProperty, value);
    }

    public static readonly StyledProperty<bool> IsPartSeveredProperty = AvaloniaProperty.Register<BossMonsterPartViewBase, bool>(
        "IsPartSevered");

    public bool IsPartSevered
    {
        get => GetValue(IsPartSeveredProperty);
        set => SetValue(IsPartSeveredProperty, value);
    }

    public static readonly StyledProperty<bool> IsKnownPartProperty = DependencyObject.Register<BossMonsterPartViewBase, bool>(
        "IsKnownPart");

    public bool IsKnownPart
    {
        get => GetValue(IsKnownPartProperty);
        set => SetValue(IsKnownPartProperty, value);
    }
    
    protected override void OnInitialized()
    {
        base.OnInitialized();

        this[!IsPartBrokenProperty] = new Binding("IsPartBroken"); 
        this[!IsPartSeveredProperty] = new Binding("IsPartSevered");
        this[!IsKnownPartProperty] = new Binding("IsKnownPart"); 
        
        PseudoClasses.Set(":part-broken", this.GetObservable(IsPartBrokenProperty));
        PseudoClasses.Set(":part-severed", this.GetObservable(IsPartSeveredProperty));
        PseudoClasses.Set(":known", this.GetObservable(IsKnownPartProperty));
    }
}