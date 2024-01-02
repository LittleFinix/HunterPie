using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Windows.Native;
using HunterPie.Core.Utils;
using System;
using System.Buffers;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace HunterPie.Core.System.Windows.Memory;

[SupportedOSPlatform("Windows")]
public class WindowsMemory : IMemory
{
    private readonly IntPtr _pHandle;
    private readonly ArrayPool<byte> _bufferPool = ArrayPool<byte>.Shared;

    public WindowsMemory(IntPtr processHandle)
    {
        _pHandle = processHandle;
    }

    public WindowsMemory(int pid)
    {
        _pHandle = Kernel32.OpenProcess(Kernel32.PROCESS_ALL_ACCESS, false, pid);

        if (_pHandle == IntPtr.Zero)
            throw new Win32Exception();

    }

    public string Read(long address, uint length, Encoding encoding = null)
    {
        byte[] buffer = _bufferPool.Rent((int)length);

        _ = Kernel32.ReadProcessMemory(_pHandle, (IntPtr)address, buffer, (int)length, out _);

        string raw = (encoding ?? Encoding.UTF8).GetString(buffer, 0, (int)length);

        _bufferPool.Return(buffer, true);
        int nullCharIdx = raw.IndexOf('\x00');

        return nullCharIdx < 0 ? raw : raw[..nullCharIdx];
    }

    public T Read<T>(long address) where T : unmanaged => Read<T>(address, 1)[0];

    public T[] Read<T>(long address, uint count) where T : unmanaged
    {
        Type type = typeof(T);

        return type.IsPrimitive ? ReadPrimitive<T>(address, count) : ReadStructure<T>(address, count);
    }

    private T[] ReadStructure<T>(long address, uint count) where T : unmanaged
    {
        int size = Marshal.SizeOf<T>() * (int)count;
        IntPtr bufferAddress = Marshal.AllocHGlobal(size);
        _ = Kernel32.ReadProcessMemory(_pHandle, (IntPtr)address, bufferAddress, size, out _);

        T[] structures = MarshalHelper.BufferToStructures<T>(bufferAddress, (int)count);

        Marshal.FreeHGlobal(bufferAddress);

        return structures;
    }

    private T[] ReadPrimitive<T>(long address, uint count) where T : unmanaged
    {
        int lpByteCount = Marshal.SizeOf<T>() * (int)count;
        var buffer = new T[count];

        _ = Kernel32.ReadProcessMemory(_pHandle, (IntPtr)address, buffer, lpByteCount, out _);

        return buffer;
    }

    public void Write<T>(long address, T data) where T : unmanaged => throw new NotImplementedException();

    public void Write<T>(long address, T[] data) where T : unmanaged
    {
        byte[] buffer = StructureToBuffer(data);

        if (!Kernel32.WriteProcessMemory(_pHandle, (IntPtr)address, buffer, buffer.Length, out int _))
            throw new Win32Exception();
    }

    public void InjectAsm(long address, byte[] asm)
    {
        if (!Kernel32.VirtualProtectEx(_pHandle, (IntPtr)address, (UIntPtr)asm.Length, 0x40, out uint oldProtect))
            throw new Win32Exception();
        Write(address, asm);
        if (!Kernel32.VirtualProtectEx(_pHandle, (IntPtr)address, (UIntPtr)asm.Length, oldProtect, out _))
            throw new Win32Exception();
    }

    public byte[] StructureToBuffer<T>(T[] array) where T : unmanaged
    {
        int size = Marshal.SizeOf<T>() * array.Length;
        IntPtr malloced = Marshal.AllocHGlobal(size);
        byte[] buffer = new byte[size];

        for (int i = 0; i < array.Length; i++)
        {
            int offset = i * Marshal.SizeOf<T>();
            Marshal.StructureToPtr(array[i], malloced + offset, false);
        }

        Marshal.Copy(malloced, buffer, 0, size);
        Marshal.FreeHGlobal(malloced);
        return buffer;
    }

    public void Inject(string dll)
    {
        byte[] dllPath = Encoding.Unicode.GetBytes(dll);

        IntPtr dllNamePtr = Kernel32.VirtualAllocEx(
            _pHandle,
            IntPtr.Zero,
            (uint)dllPath.Length + 1,
            Kernel32.AllocationType.Commit,
            Kernel32.MemoryProtection.ExecuteReadWrite
        );

        if (dllNamePtr == IntPtr.Zero)
            throw new Win32Exception();

        Write(dllNamePtr, dllPath);

        Log.Debug("Wrote DLL name at {0:X}", dllNamePtr);

        IntPtr kernel32Address = Kernel32.GetModuleHandle("kernel32");
        Log.Debug("Found kernel32 address at {0:X}", kernel32Address);
        if (kernel32Address == IntPtr.Zero)
            throw new Win32Exception();

        IntPtr loadLibraryW = Kernel32.GetProcAddress(kernel32Address, "LoadLibraryW");
        Log.Debug("kernel32::LoadLibraryW -> {0:X}", loadLibraryW);
        if (kernel32Address == IntPtr.Zero)
            throw new Win32Exception();

        IntPtr lpThreadId = IntPtr.Zero;
        IntPtr thread = Kernel32.CreateRemoteThread(
            _pHandle,
            IntPtr.Zero,
            0,
            loadLibraryW,
            dllNamePtr,
            0,
            lpThreadId
        );
        Log.Debug("thread {0:X}", thread);
        if (thread == IntPtr.Zero)
            throw new Win32Exception();
    }
}
