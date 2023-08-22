using System;
using System.Runtime.InteropServices;

namespace HunterPie.Core.System.Common.Exceptions;

public class NativeErrorException : Exception
{
    public int ErrorCode { get; }
    
    public NativeErrorException(int errno, string message) : base($"{Marshal.GetPInvokeErrorMessage(errno)}: {message}")
    {
        ErrorCode = errno;
    }
    
    public NativeErrorException(int errno) : base(Marshal.GetPInvokeErrorMessage(errno))
    {
        ErrorCode = errno;
    }
    
    public NativeErrorException() : this(Marshal.GetLastSystemError())
    {
    }
}