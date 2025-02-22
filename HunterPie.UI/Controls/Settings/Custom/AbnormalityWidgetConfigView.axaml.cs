﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Controls.Settings.Custom.Abnormality;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Controls.TextBox.Events;
using HunterPie.UI.Settings;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace HunterPie.UI.Controls.Settings.Custom;
/// <summary>
/// Interaction logic for AbnormalityWidgetConfigView.xaml
/// </summary>
public partial class AbnormalityWidgetConfigView : UserControl, INotifyPropertyChanged
{
    // TODO: Separate View from ViewModel

    private AbnormalityCollectionViewModel _selectedElement;

    public ObservableCollection<AbnormalityCollectionViewModel> Collections { get; } = new();
    public ObservableCollection<ISettingElementType> Elements { get; } = new();

    private AbnormalityCollectionViewModel _selectedCollection;

    public static readonly DirectProperty<AbnormalityWidgetConfigView, AbnormalityCollectionViewModel> SelectedCollectionProperty = AvaloniaProperty.RegisterDirect<AbnormalityWidgetConfigView, AbnormalityCollectionViewModel>(
        "SelectedCollection", o => o.SelectedCollection);

    public AbnormalityCollectionViewModel SelectedCollection
    {
        get => _selectedCollection;
        private set
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (_selectedCollection is null)
                SetAndRaise(SelectedCollectionProperty, ref _selectedCollection, value);
        }
    }

    public readonly AbnormalityWidgetConfig Config;

    public AbnormalityWidgetConfigView(AbnormalityWidgetConfig config)
    {
        Config = config;
        DataContext = config;
        InitializeComponent();

        BuildVisualConfig();
        LoadAbnormalities();
    }


    private void BuildVisualConfig()
    {
        foreach (ISettingElementType element in VisualConverterManager.BuildSubElements(Config))
            Elements.Add(element);
    }

    private void LoadAbnormalities()
    {
        Collections.Clear();

        AbnormalityCollectionViewModel[] collections = AbnormalitiesViewHelper.GetViewModelsBy(
            ClientConfig.Config.Client.LastConfiguredGame.Value,
            Config
        );

        foreach (AbnormalityCollectionViewModel coll in collections)
            Collections.Add(coll);
    }

    private void OnSearchTextChanged(object sender, SearchTextChangedEventArgs e)
    {
        if (SelectedCollection is null)
            return;

        foreach (AbnormalityViewModel vm in SelectedCollection.Abnormalities)
            vm.IsMatch = string.IsNullOrEmpty(e.Text) || Regex.IsMatch(vm.Name, e.Text, RegexOptions.IgnoreCase);
    }

    private void OnSelectAllClick()
    {
        if (SelectedCollection is null)
            return;

        foreach (AbnormalityViewModel vm in SelectedCollection.Abnormalities)
            vm.IsEnabled = true;
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        Navigator.Return();
    }

    private void SaveAbnormalities()
    {
        Config.AllowedAbnormalities.Clear();

        foreach (AbnormalityCollectionViewModel collection in Collections)
            foreach (AbnormalityViewModel abnorm in collection.Abnormalities.Where(a => a.IsEnabled))
                Config.AllowedAbnormalities.Add(abnorm.Id);
    }

    private void OnUnload(object sender, RoutedEventArgs e)
    {
        SaveAbnormalities();
    }
}
