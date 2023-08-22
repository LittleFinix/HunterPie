using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Logger;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class DebugLoggerInitializer : IInitializer
{
    public async Task Init()
    {
        if (Debugger.IsAttached)
            Log.Add(new DebugLogger());
    }
}