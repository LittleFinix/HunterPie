using Avalonia;
using Avalonia.Threading;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using System;
using System.ComponentModel;

namespace HunterPie.UI.Overlay.Widgets.Classes.Views;

/// <summary>
/// Interaction logic for ClassView.xaml
/// </summary>
public partial class ClassView : View<ClassViewModel>, IWidget<ClassWidgetConfig>, IWidgetWindow, INotifyPropertyChanged
{
    private ClassWidgetConfig _config;

    public WidgetType Type => WidgetType.ClickThrough;

    private IWidgetSettings _settings;

    public static readonly DirectProperty<ClassView, IWidgetSettings> SettingsProperty = AvaloniaProperty.RegisterDirect<ClassView, IWidgetSettings>(
        "Settings", o => o.Settings);

    public IWidgetSettings Settings
    {
        get => _settings;
        private set
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (_settings is null)
                SetAndRaise(SettingsProperty, ref _settings!, value);
        }
    }

    public event EventHandler<WidgetType> OnWidgetTypeChange;

    ClassWidgetConfig IWidget<ClassWidgetConfig>.Settings => _config;

    public string Title => "Class Widget";


    public ClassView()
    {
        InitializeComponent();

        // TODO: create interface for widget ViewModels that has a settings property that the view can bind to avoid this hacky solution
        ViewModel.PropertyChanged += OnPropertyChanged;
        Settings = ViewModel.CurrentSettings;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ClassViewModel.CurrentSettings))
            return;

        Dispatcher.UIThread.Invoke(() => Settings = ViewModel.CurrentSettings);
    }
}