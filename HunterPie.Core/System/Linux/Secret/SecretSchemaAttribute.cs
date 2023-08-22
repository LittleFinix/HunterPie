using System.Runtime.InteropServices;

namespace HunterPie.Core.System.Linux;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public partial struct SecretSchemaAttribute
{
    [NativeTypeName("const gchar *")]
    [MarshalAs(UnmanagedType.LPStr)]
    public nint name;

    public SecretSchemaAttributeType type;
}
