using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HunterPie.Core.System.Linux;

/// <summary>Specifies that the given method sets the last system error and it can be retrieved via <see cref="Marshal.GetLastSystemError" />.</summary>
[AttributeUsage(AttributeTargets.Method)]
[Conditional("DEBUG")]
internal sealed class SetsLastSystemErrorAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="SetsLastSystemErrorAttribute" /> class.</summary>
    public SetsLastSystemErrorAttribute()
    {
    }
}
