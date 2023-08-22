using Avalonia.Markup.Xaml.Templates;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Classes.Presentation;

public class ClassTemplateSelector : DataTemplateSelector
{
    public DataTemplate? InsectGlaiveTemplate { get; set; }
    public DataTemplate? ChargeBladeTemplate { get; set; }
    public DataTemplate? DualBladesTemplate { get; set; }
    public DataTemplate? EmptyTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return item is IClassViewModel viewModel
            ? viewModel.WeaponId switch
            {
                Weapon.InsectGlaive => InsectGlaiveTemplate,
                Weapon.ChargeBlade => ChargeBladeTemplate,
                Weapon.DualBlades => DualBladesTemplate,
                _ => throw new NotImplementedException($"Missing implementation for {viewModel.WeaponId}")
            }
            : EmptyTemplate;
    }
}
