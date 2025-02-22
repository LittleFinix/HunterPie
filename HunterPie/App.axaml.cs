using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Client;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Core.System;
using HunterPie.Features;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Backups;
using HunterPie.Features.Overlay;
using HunterPie.Integrations;
using HunterPie.Integrations.Discord;
using HunterPie.Internal;
using HunterPie.UI.Overlay;
using HunterPie.Update;
using HunterPie.Update.Presentation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public class App : Application
{
    private static readonly RemoteAccountConfigService RemoteConfigService = new();
    private IProcessManager? _process;
    private RichPresence? _richPresence;
    private Context? _context;

    public static MainWindow? UI { get; private set; }

    public override async void Initialize()
    {
#if DEBUG
        if (Design.IsDesignMode)
        {
            base.Initialize();
            return;
        }
#endif

        CheckForRunningInstances();

        base.Initialize();

        
        await InitializerManager.Initialize();

        UpdateService.CleanupOldFiles();

        SetRenderingMode();

// #if RELEASE
//         if (await SelfUpdate())
//             return;
// #endif

        UI = Dispatcher.UIThread.Invoke(() => new MainWindow());

        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
            desktop.MainWindow = UI;
        }
        
        UI.InitializeComponent();

        if (!ClientConfig.Config.Client.EnableSeamlessStartup)
            UI.Show();

        _ = WidgetManager.Instance;

        InitializeProcessScanners();
        SetUIThreadPriority();
    }

    private void CheckForRunningInstances()
    {
        Process[] processes = Process.GetProcessesByName("HunterPie")
            .Where(p => p.Id != Environment.ProcessId
                    && p.MainModule?.FileName == ClientInfo.ClientFileName)
            .ToArray();

        foreach (Process process in processes)
            process.Kill();
    }

    private void SetUIThreadPriority()
    {
        // Dispatcher.UIThread.Priority = ThreadPriority.Highest;
    } 

    private static async Task<bool> SelfUpdate()
    {
        if (!ClientConfig.Config.Client.EnableAutoUpdate)
            return false;

        UpdateViewModel vm = new();
        UpdateView view = new() { DataContext = vm };

        if (!ClientConfig.Config.Client.EnableSeamlessStartup)
            view.Show();

        bool result = await UpdateUseCase.Exec(vm);

        view.Close();

        if (!result)
            return result;

        InitializerManager.Unload();
        Restart();

        return result;
    }

    private void InitializeProcessScanners()
    {
        ProcessManager.Start();
        ProcessManager.OnProcessFound += OnProcessFound;
        ProcessManager.OnProcessClosed += OnProcessClosed;
    }

    private static void SetRenderingMode()
    {
        // RenderOptions.ProcessRenderMode = ClientConfig.Config.Client.Render == RenderingStrategy.Hardware
        //     ? RenderMode.Default
        //     : RenderMode.SoftwareOnly;
    }

    private void OnProcessClosed(object? sender, ProcessManagerEventArgs e)
    {
        if (_process is null)
            return;

        UnhookEvents();
        _richPresence?.Dispose();
        _richPresence = null;

        ScanManager.Stop();
        _context?.Dispose();

        _process = null;
        _context = null;

        Dispatcher.UIThread.Invoke(WidgetInitializers.Unload);
        WidgetManager.Dispose();

        Log.Info("{0} has been closed", e.ProcessName);

        SmartEventsTracker.DisposeEvents();

        ContextInitializers.Dispose();

        if (e.Process.HasExitedNormally == false
            && e.Process.Game == GameProcess.MonsterHunterWorld
            && ClientConfig.Config.Client.EnableNativeModule)
            Log.Info(
                "{0} has exited abnormally. If you have not installed Stracker's Loader and CRC bypass mod, turning off \"Enable native module\" in Client Settings may help.",
                e.ProcessName
            );

        if (ClientConfig.Config.Client.ShouldShutdownOnGameExit)
            Dispatcher.UIThread.Invoke(Close);
    }

    private async void OnProcessFound(object? sender, ProcessManagerEventArgs e)
    {
        if (_process is not null)
        {
            Log.Info("HunterPie is already hooked to another process.");
            return;
        }

        try
        {
            _process = e.Process;
            Context context = GameIntegrationService.CreateNewGameContext(e.ProcessName, _process);

            Log.Debug("Initialized game context");
            _context = context;

            HookEvents();
            _richPresence = DiscordPresenceController.GetPresenceBy(context);

            WidgetManager.Hook(context);

            await ContextInitializers.InitializeAsync(context).ConfigureAwait(false);

            await Dispatcher.UIThread.InvokeAsync(() => WidgetInitializers.Initialize(context));

            ScanManager.Start();

            Log.Debug("Active events: {0} with {1} total references", SmartEventsTracker.ActiveEvents(), SmartEventsTracker.CountReferences());
        }
        catch (Exception ex)
        {
            Log.Error("HunterPie fails to initialize on the {0} process. {1}", e.ProcessName, ex);
        }

        _ = await GameSaveBackupService.ExecuteBackup().ConfigureAwait(false);
    }
    
    // private void OnUIException(object sender, DispatcherUnhandledExceptionEventArgs e)
    // {
    //     e.Handled = true;
    //
    //     ExceptionTracker.TrackException(e.Exception);
    // }

    private void HookEvents()
    {
        _context!.Game.Player.OnLogin += OnPlayerLogin;
        _context.Game.Player.OnStageUpdate += OnStageUpdate;
    }

    private void UnhookEvents()
    {
        _context!.Game.Player.OnLogin -= OnPlayerLogin;
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;
    }

    private void OnPlayerLogin(object? sender, EventArgs e) => Log.Info($"Logged in as {_context!.Game.Player.Name}");
    private void OnStageUpdate(object? sender, EventArgs e) => Log.Debug("StageId: {0} | InHuntingZone: {1}", _context!.Game.Player.StageId, _context.Game.Player.InHuntingZone);

    public static async void Restart()
    {
        Dispatcher.UIThread.InvokeAsync(() => UI.Hide());

        await RemoteConfigService.UploadClientConfig();

        if (OperatingSystem.IsWindows())
            Process.Start(typeof(MainWindow).Assembly.Location.Replace(".dll", ".exe"));
        else if (OperatingSystem.IsWindows())
            Process.Start(typeof(MainWindow).Assembly.Location.Replace(".dll", ""));
        
        if (Current.ApplicationLifetime is IControlledApplicationLifetime desktop)
            desktop.Shutdown();
    }

    private void Close()
    {
        InitializerManager.Unload();
        if (Current.ApplicationLifetime is IControlledApplicationLifetime desktop)
            desktop.Shutdown();
    }
}
