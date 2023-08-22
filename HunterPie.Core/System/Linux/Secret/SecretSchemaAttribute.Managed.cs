using System;
using System.Runtime.InteropServices;

namespace HunterPie.Core.System.Linux;

public partial struct SecretSchemaAttribute
{
    public string Name
    {
        get => Marshal.PtrToStringUTF8(name)!;
        set
        {
            if (name != IntPtr.Zero)
                Marshal.FreeHGlobal(name);

            if (value is not null)
                name = Marshal.StringToHGlobalAnsi(value);
            else
                name = IntPtr.Zero;
        }
    }
}
