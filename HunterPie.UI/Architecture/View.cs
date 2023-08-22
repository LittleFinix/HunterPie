using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using HunterPie.Core.Architecture;
using HunterPie.UI.Overlay;
using System;

namespace HunterPie.UI.Architecture;

public class View<TViewModel> : UserControl, IDisposable, IView<TViewModel>
    where TViewModel : Bindable
{
    public TViewModel ViewModel => (TViewModel)DataContext;

    public Dispatcher UIThread => Dispatcher.UIThread;

    protected virtual TViewModel InitializeViewModel(params object[] args)
    {
        if (this is not IWidgetWindow)
            return Activator.CreateInstance<TViewModel>();

        try
        {
            return (TViewModel)Activator.CreateInstance(typeof(TViewModel), args);
        }
        catch { }

        return Activator.CreateInstance<TViewModel>();
    }

    protected bool IsDesignMode => false; // DesignerProperties.GetIsInDesignMode(this);

    public View()
    {
        DataContext = InitializeViewModel();
    }

    public View(params object[] args)
    {
        DataContext = InitializeViewModel(args);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (IsDesignMode)
            return;

        Unloaded += OnViewUnloaded;

        Initialize();
    }

    private void OnViewUnloaded(object sender, RoutedEventArgs e)
    {
        if (ViewModel is IDisposable vm)
            vm.Dispose();

        Unloaded -= OnViewUnloaded;

        Dispose();
    }

    protected virtual void Initialize() { }

    public virtual void Dispose() { }
}
