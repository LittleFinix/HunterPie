using Avalonia;
using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Events;
using HunterPie.Core.Extensions;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Linux.Memory;
using HunterPie.Core.System.Windows.Memory;
using HunterPie.Core.System.Windows.Native;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace HunterPie.Core.System.Common;

internal abstract class ProcessManagerBase : IProcessManager, IEventDispatcher
{

    protected Thread? _pooler;
    private bool _isProcessForeground;
    private bool _shouldPauseThread;
    protected bool ShouldPollProcess = true;

    private IMemory? _memory;
    private IntPtr pHandle;

    public event EventHandler<ProcessEventArgs>? OnGameStart;
    public event EventHandler<ProcessEventArgs>? OnGameClosed;
    public event EventHandler<ProcessEventArgs>? OnGameFocus;
    public event EventHandler<ProcessEventArgs>? OnGameUnfocus;

    public abstract string Name { get; }
    public abstract GameProcess Game { get; }

    /// <inheritdoc />
    public bool? HasExitedNormally { get; private set; }

    public int Version { get; private set; }

    public Process? Process { get; private set; }

    public int ProcessId { get; private set; }

    public bool IsRunning { get; private set; }
    
    public virtual PixelRect GameArea => Process.GetWindowClientArea();

    public bool IsProcessForeground
    {
        get => _isProcessForeground;
        private set
        {
            if (_isProcessForeground != value)
            {
                _isProcessForeground = value;

                this.Dispatch(
                    value ? OnGameFocus
                          : OnGameUnfocus,
                    new ProcessEventArgs(Name)
                );
            }
        }
    }

    public IMemory Memory => _memory!;

    public void Initialize()
    {
        Log.Info($"Started scanning for process {Name}...");

        _pooler = new Thread(ExecutePolling) { Name = "PollingBackgroundThread", IsBackground = true, };
        _pooler.Start();
    }

    private void ExecutePolling()
    {
        while (ShouldPollProcess)
        {
            if (_shouldPauseThread)
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException)
                {
                    continue;
                }

            PollProcessInfo();
            Thread.Sleep(150);
        }
    }

    private void PollProcessInfo()
    {
        if (Process?.HasExited == true)
        {
            OnProcessExit();
            return;
        }

        string name = Name;

        // Process name is limited to 16 characters on linux
        if (OperatingSystem.IsLinux())
            name = name[..15];

        Process? mhProcess = Process.GetProcessesByName(name).FirstOrDefault();

        if (mhProcess is null)
            return;

        if (Process is not null)
        {
            IsProcessForeground = Process.GetMainWindow()?.IsFocused == true;
            mhProcess.Dispose();
            return;
        }

        if (!ShouldOpenProcess(mhProcess))
            return;

        try
        {
            if (mhProcess.MainModule is null)
                throw new InvalidOperationException("Process main module is null.");

            Process = mhProcess;
            ProcessId = mhProcess.Id;
            HasExitedNormally = null;
            // We want to retrieve process exit code, so force Process to call OpenProcess by explicitly retrieving its SafeHandle.
            // Otherwise there will be InvalidOperationException: Process was not started by this object, so requested information cannot be determined.
            _ = Process.SafeHandle;

            IsRunning = true;

            if (OperatingSystem.IsWindows())
                _memory = new WindowsMemory(ProcessId);
            else if (OperatingSystem.IsLinux())
                _memory = new LinuxMemory(Process);
            else
                throw new PlatformNotSupportedException();

            bool identified = false;
            
            for (int i = 0; i < Process.Modules.Count; i++)
            {
                if (Process.Modules[i].ModuleName.StartsWith("MonsterHunter"))
                {
                    AddressMap.Add("BASE", (long)Process.Modules[i].BaseAddress);
                    identified = true;
                    break;
                }
            }
            
            if (!identified && Process.MainModule.ModuleName.StartsWith("MonsterHunter"))
            {
                AddressMap.Add("BASE", (long)Process.MainModule.BaseAddress);
                identified = true;
            }
            
            if (!identified)
            {
                Log.Warn("Failed to find base address, using default");
                AddressMap.Add("BASE", 0x140000000L);
            }

            this.Dispatch(OnGameStart, new(Name));
        }
        catch (Exception ex)
        {
            Log.Error("Failed to open game process. Run HunterPie as Administrator!");
            Log.Info("Error details: {0}", ex);
            ShouldPollProcess = false;
        }
    }

    protected abstract bool ShouldOpenProcess(Process process);

    private void OnProcessExit()
    {
        Debug.Assert(Process != null);

        HasExitedNormally = Process.ExitCode == 0;
        Process.Dispose();
        Process = null;

        _ = Kernel32.CloseHandle(pHandle);

        pHandle = IntPtr.Zero;

        this.Dispatch(OnGameClosed, new(Name));
    }

    public void Pause() => _shouldPauseThread = true;

    public void Resume()
    {
        _shouldPauseThread = false;
        _pooler?.Interrupt();
    }

}