using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace HunterPie.Core.System.Linux;

[SupportedOSPlatform("Linux")]
public static class Secret
{
    [DllImport("secret", CallingConvention = CallingConvention.Cdecl, EntryPoint = "secret_password_store_sync", ExactSpelling = true)]
    [return: NativeTypeName("gboolean")]
    public static extern bool password_store_sync(
        [In] IntPtr schema,
        [In] [MarshalAs(UnmanagedType.LPStr)] string? collection,
        [In] [MarshalAs(UnmanagedType.LPStr)] string label,
        [In] [MarshalAs(UnmanagedType.LPStr)] string password,
        [In] IntPtr cancellable,
        [In] IntPtr error, __arglist);

    [DllImport("secret", CallingConvention = CallingConvention.Cdecl, EntryPoint = "secret_password_lookup_sync", ExactSpelling = true)]
    [return: NativeTypeName("gchar *")]
    public static extern string password_lookup_sync(__arglist);

    [DllImport("secret", CallingConvention = CallingConvention.Cdecl, EntryPoint = "secret_password_clear_sync", ExactSpelling = true)]
    [return: NativeTypeName("gboolean")]
    public static extern bool password_clear_sync(__arglist);

    [DllImport("secret", CallingConvention = CallingConvention.Cdecl, EntryPoint = "secret_password_free", ExactSpelling = true)]
    public static extern void password_free();
}
