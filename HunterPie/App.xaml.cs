﻿using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using HunterPie.Core.Client;
using HunterPie.Core.System;
using HunterPie.Internal;
using System.Windows.Media;
using HunterPie.Core.Client.Configuration.Enums;
using System.Windows.Interop;
using HunterPie.Integrations.Discord;
using HunterPie.UI.Overlay;
using HunterPie.Update;
using HunterPie.Update.Presentation;
using System.Threading.Tasks;
using System.Linq;
using HunterPie.Features.Overlay;
using HunterPie.Core.Events;
using HunterPie.Core.Domain;
using HunterPie.Internal.Poogie;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProcessManager _process;
        private RiseRichPresence _richPresence;
        private Context _context;

        protected override async void OnStartup(StartupEventArgs e)
        {
            CheckForRunningInstances();

            base.OnStartup(e);

            InitializerManager.Initialize();
            SetRenderingMode();

            await SelfUpdate();

            ShutdownMode = ShutdownMode.OnMainWindowClose;
            
            MainWindow window = new MainWindow();
            window.Show();

            InitializeProcessScanners();
        }

        private void CheckForRunningInstances()
        {
            Process[] processes = Process.GetProcessesByName("HunterPie")
                .Where(p => p.Id != Environment.ProcessId)
                .ToArray();

            foreach (Process process in processes)
                process.Kill();
        }

        private async Task SelfUpdate()
        {
            if (!ClientConfig.Config.Client.EnableAutoUpdate)
                return;

            UpdateViewModel vm = new();
            UpdateView view = new() { DataContext = vm };
            view.Show();

            bool result = await UpdateUseCase.Exec(vm);

            view.Close();

            if (result)
                Restart();

        }

        private void InitializeProcessScanners()
        {
            ProcessManager.Start();
            ProcessManager.OnProcessFound += OnProcessFound;
            ProcessManager.OnProcessClosed += OnProcessClosed;
        }

        private static void SetRenderingMode()
        {
            RenderOptions.ProcessRenderMode = ClientConfig.Config.Client.Rendering == RenderingStrategy.Hardware
                ? RenderMode.Default
                : RenderMode.SoftwareOnly;

        }

        private void OnProcessClosed(object sender, ProcessManagerEventArgs e)
        {
            if (_process is null)
                return;

            UnhookEvents();
            _richPresence?.Dispose();

            ScanManager.Stop();

            _process = null;
            _context = null;

            WidgetInitializers.Unload();

            Dispatcher.InvokeAsync(WidgetManager.Dispose);
        }

        private void OnProcessFound(object sender, ProcessManagerEventArgs e)
        {
            if (_process is not null)
            {
                Log.Info("HunterPie is already hooked to another process.");
                return;
            }

            _process = e.Process;
            GameManager.InitializeGameData(e.ProcessName);
            Context context = GameManager.GetGameContext(e.ProcessName, _process);

            Log.Debug("Initialized game context");
            _context = context;

            _process.OnGameFocus += OnGameFocus;
            _process.OnGameUnfocus += OngameUnfocus;

            HookEvents();
            _richPresence = new(context);
            
            Dispatcher.InvokeAsync(() => WidgetInitializers.Initialize(context));
            
            ScanManager.Start();
        }

        private void OngameUnfocus(object sender, ProcessEventArgs e) => WidgetManager.Instance.IsGameFocused = false;
        private void OnGameFocus(object sender, ProcessEventArgs e) => WidgetManager.Instance.IsGameFocused = true;

        private void OnUIException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            RemoteCrashReporter.Send(e.Exception);
            Log.Error(e.Exception.ToString());
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (e.ApplicationExitCode == 0)
                ConfigManager.SaveAll();

            InitializerManager.Unload();
            base.OnExit(e);
        }

        private void HookEvents()
        {
            _context.Game.Player.OnLogin += OnPlayerLogin;
        }


        private void UnhookEvents()
        {
            _process.OnGameFocus -= OnGameFocus;
            _process.OnGameUnfocus -= OngameUnfocus;
            _context.Game.Player.OnLogin -= OnPlayerLogin;
        }

        private void OnPlayerLogin(object sender, EventArgs e) => Log.Info($"Logged in as {_context.Game.Player.Name}");

        public static void Restart()
        {
            Process.Start(typeof(MainWindow).Assembly.Location.Replace(".dll", ".exe"));
            Current.Shutdown();
        }
    }
}
