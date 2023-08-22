using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class UITracerInitializer : IInitializer
{
    public Task Init()
    {
        // PresentationTraceSources.Refresh();
        // _ = PresentationTraceSources.DataBindingSource.Listeners.Add(new LogTracer());
        // PresentationTraceSources.DataBindingSource.Switch.Level = ClientConfig.Config.Development.PresentationSourceLevel;

        return Task.CompletedTask;
    }
}
