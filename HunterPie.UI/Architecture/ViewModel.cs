using Avalonia.Threading;
using HunterPie.Core.Architecture;

namespace HunterPie.UI.Architecture;

public class ViewModel : Bindable
{
    public Dispatcher UIThread => Dispatcher.UIThread;
}
