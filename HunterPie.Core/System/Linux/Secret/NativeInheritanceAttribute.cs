using System;
using System.Diagnostics;

namespace HunterPie.Core.System.Linux;

/// <summary>Defines the base type of a struct as it was in the native signature.</summary>
[AttributeUsage(AttributeTargets.Struct)]
[Conditional("DEBUG")]
internal sealed class NativeInheritanceAttribute : Attribute
{
    private readonly string _name;

    /// <summary>Initializes a new instance of the <see cref="NativeInheritanceAttribute" /> class.</summary>
    /// <param name="name">The name of the base type that was inherited from in the native signature.</param>
    public NativeInheritanceAttribute(string name)
    {
        _name = name;
    }

    /// <summary>Gets the name of the base type that was inherited from in the native signature.</summary>
    public string Name => _name;
}
