using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using HunterPie.Core.Architecture;
using HunterPie.Core.Logger;
using System;

namespace HunterPie.UI.Architecture;

public class ObservableTemplate : ITypedDataTemplate, IRecyclingDataTemplate
{
    [Content, TemplateContent]
    public object? Content { get; set; }
    
    public UIElement? Build(object? param)
    {
        return Build(param, null);
    }

    public UIElement? Build(object? data, UIElement? existing)
    {
        try
        {
            existing ??= TemplateContent.Load(Content)?.Result ?? new ContentPresenter();
        }
        catch (Exception e)
        {
            Log.Error($"Failed to create template for type {ObservableType}: {e}");
            throw;
        }

        existing.DataContext = data;

        return existing;
    }

    public bool Match(object? data)
    {
        if (ObservableType is not null)
            return data?.GetType() == DataType;
        return data?.GetType()?.GetGenericTypeDefinition() == DataType;
    }

    public Type? ObservableType { get; set; }
    
    [DataType]
    public Type? DataType =>
        ObservableType is not null ? typeof(Observable<>).MakeGenericType(ObservableType) : typeof(Observable<>);
}