using HunterPie.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class SystemTrayInitializer : IInitializer, IDisposable
{
    public void Dispose() {}

    public Task Init()
    {
        return Task.CompletedTask;
    }
}
