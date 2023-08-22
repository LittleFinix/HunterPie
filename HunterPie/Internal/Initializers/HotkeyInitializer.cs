using HunterPie.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class HotkeyInitializer : IInitializer, IDisposable
{
    public Task Init()
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}
