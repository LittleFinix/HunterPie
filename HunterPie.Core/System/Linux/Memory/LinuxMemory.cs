using HunterPie.Core.Domain.Memory;
using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;

namespace HunterPie.Core.System.Linux.Memory;

[SupportedOSPlatform("Linux")]
public class LinuxMemory : IMemory, IDisposable
{
    private readonly Process _process;

    private readonly ThreadLocal<FileStream> _mem;
    private readonly ArrayPool<byte> _bufferPool = ArrayPool<byte>.Shared;

    public LinuxMemory(Process process)
    {
        _process = process;
        
        Posix.Attach(_process.Id);
        _mem = new ThreadLocal<FileStream>();
    }

    private Stream GetMem(long addr, FileAccess access)
    {
        if (_process.HasExited)
            throw new InvalidOperationException();
        
        if ((ulong)addr < 4 * 1024) // lowest page is definitely unmapped
            throw new NullReferenceException();
        
        if (!_mem.IsValueCreated)
        {
            var opts = FileOptions.RandomAccess;

            if (access.HasFlag(FileAccess.Write))
                opts |= FileOptions.WriteThrough;
            
            _mem.Value = new FileStream($"/proc/{_process.Id}/mem",
                FileMode.Open,
                access,
                FileShare.ReadWrite,
                bufferSize: 0,
                options: opts
            );
        }

        _mem.Value!.Position = addr;
        return _mem.Value!;
    }

    private Stream GetReader(long addr, Encoding? encoding = null)
    {
        return GetMem(addr, FileAccess.Read);
    }
    private Stream GetWriter(long addr, Encoding? encoding = null)
    {
        return GetMem(addr, FileAccess.Write);
    }
    
    public string Read(long address, uint length, Encoding? encoding = null)
    {
        byte[] bytes = _bufferPool.Rent((int)length);

        try
        {
            var span = bytes.AsSpan(0, (int)length);
            var reader = GetReader(address, encoding);
            reader.ReadExactly(span);

            string str = (encoding ?? Encoding.UTF8).GetString(span[..]);
            int idx = str.IndexOf('\0');

            return idx < 0 ? str : str[..idx];
        }
        catch (IOException)
        {
            return null!;
        }
        finally
        {
            _bufferPool.Return(bytes);
        }
    }

    public T Read<T>(long address) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        byte[] bytes = _bufferPool.Rent(size);

        try
        {
            var span = bytes.AsSpan(0, size);
            var reader = GetReader(address);
        
            reader.ReadExactly(span);
            return MemoryMarshal.Read<T>(span);
        }
        catch (IOException)
        {
            return default;
        }
        finally
        {
            _bufferPool.Return(bytes);
        }
    }

    public T[] Read<T>(long address, uint count) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        byte[] bytes = _bufferPool.Rent(size);

        try
        {
            var span = bytes.AsSpan(0, size);
            var reader = GetReader(address);

            var arr = new T[count];

            for (int i = 0; i < count; i++)
            {
                reader.ReadExactly(span);
                arr[i] = MemoryMarshal.Read<T>(span);
            }

            return arr;
        }
        catch (IOException)
        {
            return null!;
        }
        finally
        {
            _bufferPool.Return(bytes);
        }
    }

    public void Write<T>(long address, T data) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        byte[] bytes = _bufferPool.Rent(size);
        
        try
        {
            var span = bytes.AsSpan(0, size);
            var writer = GetWriter(address);
            MemoryMarshal.Write(bytes.AsSpan(0, size), ref data);
        
            writer.Write(bytes);
        }
        finally
        {
            _bufferPool.Return(bytes);
        }
    }

    public void Write<T>(long address, T[] data) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        byte[] bytes = _bufferPool.Rent(size);

        try
        {
            var span = bytes.AsSpan(0, size);
            var writer = GetWriter(address);

            foreach (var item in data)
            {
                var nitem = item;
                MemoryMarshal.Write(span, ref nitem);
                writer.Write(span);
            }
        }
        finally
        {
            _bufferPool.Return(bytes);
        }
    }

    public void InjectAsm(long address, byte[] asm)
    {
        Write(address, asm);
    }

    public void Inject(string dll)
    {
        throw new NotSupportedException();
    }

    private void ReleaseUnmanagedResources()
    {
        Posix.Detach(_process.Id);
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            if (_mem.IsValueCreated)
                _mem.Value?.Dispose();

            _mem.Dispose();
            _process.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~LinuxMemory()
    {
        Dispose(false);
    }
}