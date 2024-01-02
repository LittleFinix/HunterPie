namespace HunterPie.Core.Domain.Memory;

public interface IMemoryWriter
{
    public void Write<T>(long address, T data) where T : unmanaged;
    public void Write<T>(long address, T[] data) where T : unmanaged;
    public void InjectAsm(long address, byte[] asm);
    public void Inject(string dll);
}
