using HunterPie.Core.System.Common.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace HunterPie.Core.System.Linux;

[SupportedOSPlatform("Linux")]
public static class Posix
{
    public enum PTRACE
    {
        DETACH = 17,
        SEIZE = 0x4206,
    }
    
    [DllImport("c", CharSet = CharSet.Ansi)]
    private static extern int readlink([In] [MarshalAs(UnmanagedType.LPStr)] string pathname,
        [Out] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] char[] buf, [In] uint bufsiz = 0);

    [DllImport("c", CharSet = CharSet.Ansi)]
    private static extern long ptrace(
        [In] PTRACE request,
        [In] int pid,
        [In] IntPtr addr,
        [In, Out] IntPtr data
    );

    /// <summary>
    /// Resolves the given path, if it is a symlink
    /// </summary>
    /// <param name="pathname">The symlink to resolve</param>
    /// <returns>The resolved path</returns>
    /// <exception cref="NativeErrorException">see man 2 readlink for more info</exception>
    public static string Readlink(string pathname)
    {
        char[] buf = new char[2048];
        int len = readlink(pathname, buf, (uint)buf.Length);

        if (len > 0 && len < buf.Length)
            return new(buf[..len]);
        throw new NativeErrorException();
    }

    /// <summary>
    /// Resolves the given path, if it is a symlink.
    /// </summary>
    /// <remarks>
    /// If <paramref name="pathname"/> is not a symlink, <paramref name="resolved"/> is set to the current value of <paramref name="pathname"/>,
    /// otherwise the resolved path is put in <paramref name="resolved"/>.
    /// </remarks>
    /// <param name="pathname">The symlink to resolve</param>
    /// <param name="resolved">The resolved path</param>
    /// <exception cref="NativeErrorException">see man 2 readlink for more info, never throws EINVAL (22).</exception>
    /// <returns>True if <paramref name="pathname"/> was a symlink, false otherwise.</returns>
    public static bool TryReadLink(string pathname, out string resolved)
    {
        resolved = pathname;
        
        try
        {
            resolved = Readlink(pathname);
            return true;
        }
        catch (NativeErrorException e) when (e.ErrorCode == 22) // EINVAL
        {
            return false;
        }
    }
    
    public static void Attach(int pid)
    {
        ptrace(PTRACE.SEIZE, pid, 0, 0);
    }
    
    public static void Detach(int pid)
    {
        ptrace(PTRACE.DETACH, pid, 0, 0);
    }
}