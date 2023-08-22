using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Supporter;
using HunterPie.Integrations.Poogie.Supporter.Models;
using System;
using System.Reflection;
using System.Security.Principal;

namespace HunterPie.GUI.Parts;

public class HeaderBarViewModel : Bindable
{
    private readonly PoogieSupporterConnector _supporterConnector = new();
    private bool _isSupporter;
    private bool _isFetchingSupporter;
    private bool _isNotificationsToggled;

    public string Version
    {
        get
        {
            Assembly self = typeof(App).Assembly;
            AssemblyName name = self.GetName();
            Version ver = name.Version;

            return $"v{ver}";
        }
    }

    public bool IsSupporter { get => _isSupporter; set => SetValue(ref _isSupporter, value); }
    public bool IsFetchingSupporter { get => _isFetchingSupporter; set => SetValue(ref _isFetchingSupporter, value); }
    public bool IsNotificationsToggled { get => _isNotificationsToggled; set => SetValue(ref _isNotificationsToggled, value); }

    public bool IsRunningAsAdmin => GetAdminState();

    public void MinimizeApplication()
    {
        MainWindow? win = Window;
        
        if (win is null)
            return;
        
        win.WindowState = WindowState.Minimized;
        
        if (ClientConfig.Config.Client.MinimizeToSystemTray && OperatingSystem.IsWindows())
            win.Hide();
    }

    public async void FetchSupporterStatus()
    {
        IsFetchingSupporter = true;

        PoogieResult<SupporterValidationResponse> res = await _supporterConnector.IsValidSupporter();

        IsSupporter = res?.Response?.IsValid ?? false;

        IsFetchingSupporter = false;
    }
    
    private MainWindow? Window => (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow as MainWindow;

    public void CloseApplication() => Window?.Close();

    public void DragApplication(PointerPressedEventArgs e)
    {
        Window?.BeginMoveDrag(e);
    }

    private bool GetAdminState()
    {
        if (OperatingSystem.IsWindows())
        {
            var winIdentity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(winIdentity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        if (OperatingSystem.IsLinux())
        {
            return Environment.GetEnvironmentVariable("UID") == "0";
        }

        return false;
    }
}
