using System.Text;

namespace HunterPie.Core.Domain.Memory;

public interface IMemoryReader
{
    public string Read(long address, uint length, Encoding encoding = null);
    public T Read<T>(long address) where T : unmanaged;
    public T[] Read<T>(long address, uint count) where T : unmanaged;
}
