using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using System;

namespace HunterPie.UI.Architecture.Templates;

public class MonsterPartDataTemplate : ITypedDataTemplate, IRecyclingDataTemplate
{
    [TemplateContent, Content]
    public object Content { get; set; }
    
    public PartType? PartType { get; set; }
    
    public UIElement? Build(object? param)
    {
        return Build(param, null);
    }

    public bool Match(object? data) => data is MonsterPartViewModel vm && (vm.Type == PartType || PartType is null);

    public UIElement? Build(object? data, UIElement? existing)
    {
        if (data is not MonsterPartViewModel vm)
            return null;

        existing ??= TemplateContent.Load(Content).Result;
        existing.DataContext = data;

        return existing;
    }

    public Type? DataType => typeof(MonsterPartViewModel);
}
