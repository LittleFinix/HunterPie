using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Events;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Windows;
using System;

namespace HunterPie.Core.System;

public static class ProcessManager
{
    public static event EventHandler<ProcessManagerEventArgs> OnProcessFound;
    public static event EventHandler<ProcessManagerEventArgs> OnProcessClosed;

    public static GameProcess Game { get; private set; } = GameProcess.None;
    
    public static IProcessManager? Current { get; private set; }

    public static IProcessManager[] Managers { get; } = {
        new MHWProcessManager(),
        new MHRProcessManager(),
        new MHRSunbreakDemoProcessManager(),
    };

    public static void Start()
    {
        if (Managers.Length == 0)
            Log.Warn("No Process Managers have been implemented!");
        
        foreach (IProcessManager manager in Managers)
        {
            manager.OnGameStart += OnGameStartCallback;
            manager.OnGameClosed += OnGameClosedCallback;

            manager.Initialize();
        }
    }

    private static void OnGameClosedCallback(object sender, ProcessEventArgs e)
    {
        if (sender is IProcessManager manager)
        {
            ResumeAllPollingThreads(manager);
            Game = GameProcess.None;
            Current = null;
            OnProcessClosed?.Invoke(sender, new(manager, e.ProcessName));
        }
    }

    private static void OnGameStartCallback(object sender, ProcessEventArgs e)
    {
        if (sender is IProcessManager manager)
        {
            PauseAllPollingThreads(manager);
            Game = manager.Game;
            Current = manager;
            OnProcessFound?.Invoke(sender, new((IProcessManager)sender, e.ProcessName));
        }
    }

    private static void PauseAllPollingThreads(IProcessManager activeManager)
    {
        foreach (IProcessManager manager in Managers)
        {
            if (manager != activeManager)
                manager.Pause();
        }
    }

    private static void ResumeAllPollingThreads(IProcessManager activeManager)
    {
        foreach (IProcessManager manager in Managers)
        {
            if (manager != activeManager)
                manager.Resume();
        }
    }
}
