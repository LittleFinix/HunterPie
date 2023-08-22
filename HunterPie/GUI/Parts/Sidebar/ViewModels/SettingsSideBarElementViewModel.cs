using Avalonia.Threading;
using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Generics;
using HunterPie.Features.Account.Config;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.GUI.Parts.Settings.Views;
using HunterPie.Internal.Initializers;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Controls.Flags;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Settings;
using System.Linq;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class SettingsSideBarElementViewModel : ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_SETTINGS");

    public string Text => Localization.FindString("Client", "Tabs", "Tab", "SETTINGS_STRING");

    public bool IsActivable => true;

    public bool IsEnabled => true;

    public bool ShouldNotify => false;

    public SettingsSideBarElementViewModel()
    {
        RefreshWindowOnChange(ClientConfig.Config.Client.LastConfiguredGame);
        RefreshWindowOnChange(ClientConfig.Config.Client.EnableFeatureFlags);
    }

    public void ExecuteOnClick() => RefreshSettingsWindow();

    private async void RefreshSettingsWindow(bool forceRefresh = false)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            ISettingElement[] settingTabs = VisualConverterManager.Build(ClientConfig.Config);

            ISettingElement[] gameSpecificTabs = VisualConverterManager.Build(
                ClientConfigHelper.GetGameConfigBy(ClientConfig.Config.Client.LastConfiguredGame.Value)
            );

            ISettingElement[] accountConfig = await LocalAccountConfig.CreateAccountSettingsTab();

            accountConfig = accountConfig.Concat(settingTabs)
                .Concat(gameSpecificTabs)
                .ToArray();

            GenericFileSelector _ = ClientConfig.Config.Client.Language;

            SettingHostViewModel vm = new(accountConfig);
            var host = new SettingHost { DataContext = vm };

            // Also add feature flags if enabled
            if (ClientConfig.Config.Client.EnableFeatureFlags)
                vm.Elements.Add(new FeatureFlagsView(FeatureFlagsInitializer.Features.Flags));

            Navigator.Navigate(host, forceRefresh);
        });
    }

    private void RefreshWindowOnChange(Bindable observable) => observable.PropertyChanged += (_, __) =>
    {
        if (!Navigator.IsInstanceOf<SettingHost>())
            return;

        RefreshSettingsWindow(true);
    };

}
