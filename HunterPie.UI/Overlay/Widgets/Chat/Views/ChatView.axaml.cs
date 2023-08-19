using Avalonia.Controls;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Chat.Views;

/// <summary>
/// Interaction logic for ChatView.xaml
/// </summary>
public partial class ChatView : View<ChatViewModel>, IWidget<ChatWidgetConfig>, IWidgetWindow, IEventDispatcher
{
    private WidgetType _widgetType = WidgetType.ClickThrough;

    public ChatView(ChatWidgetConfig config)
    {
        Settings = config;
        InitializeComponent();
    }

    public ChatWidgetConfig Settings { get; }

    public string Title => "Chat Widget";

    public WidgetType Type
    {
        get => _widgetType;
        internal set
        {
            if (value != _widgetType)
            {
                _widgetType = value;
                this.Dispatch(OnWidgetTypeChange, _widgetType);
            }
        }
    }

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public event EventHandler<WidgetType> OnWidgetTypeChange;

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (sender is ScrollViewer scrollViewer)
        {
            double scrollableSize = scrollViewer.Height;
            double scrollPosition = scrollViewer.Offset.Y;
            double extentHeight = scrollViewer.Height;

            if (scrollableSize + scrollPosition == extentHeight || extentHeight < scrollableSize)
                scrollViewer.ScrollToEnd();
        }
    }
}
