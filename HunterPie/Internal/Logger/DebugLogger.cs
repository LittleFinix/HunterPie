using HunterPie.Core.Logger;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie.Internal.Logger;

using Log = Debug;

internal class DebugLogger : ILogger
{
    public async Task Debug(string message)
    {
        Log.WriteLine("[DEBUG] " + message);
    }

    public async Task Debug(string format, params object[] args)
    {
        Log.WriteLine("[DEBUG] " + format, args);        
    }
    
    public async Task Info(string message)
    {
        Log.WriteLine("[INFO ] " + message);        
    }

    public async Task Info(string format, params object[] args)
    {
        Log.WriteLine("[INFO ] " + format, args);        
    }
    
    public async Task Warn(string message)
    {
        Log.WriteLine("[WARN ] " + message);        
    }

    public async Task Warn(string format, params object[] args)
    {
        Log.WriteLine("[WARN ] " + format, args);        
    }

    public async Task Native(string message)
    {
        Log.WriteLine("[NATIV] " + message);        
    }

    public async Task Native(string format, params object[] args)
    {
        Log.WriteLine("[NATIV] " + format, args);        
    }

    public async Task Error(string message)
    {
        Log.WriteLine("[ERROR] " + message);        
    }

    public async Task Error(string format, params object[] args)
    {
        Log.WriteLine("[ERROR] " + format, args);        
    }

    public async Task Benchmark(string message)
    {
        Log.WriteLine("[BMARK] " + message);        
    }
    
    public async Task Benchmark(string format, params object[] args)
    {
        Log.WriteLine("[BMARK] " + format, args);        
    }
}