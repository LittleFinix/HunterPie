using HunterPie.Core.Domain.Memory;

namespace HunterPie.Core.Extensions;

public static class MemoryExtensions
{
    public const long NULLPTR = 0;
    
    public static long Read(this IMemoryReader reader, long address, int[] offsets)
    {
        foreach (int offset in offsets)
        {
            long tmp = reader.Read<long>(address);

            if (tmp == NULLPTR)
                return NULLPTR;

            address = tmp + offset;
        }

        return address;
    }
    
    public static long ReadPtr(this IMemoryReader reader, long address, int[] offsets)
    {
        foreach (int offset in offsets)
        {
            long newAddress = address + offset;
            long tmp = reader.Read<long>(newAddress);

            if (tmp == NULLPTR)
                return NULLPTR;

            address = tmp;
        }

        return address;
    }

    public static T Deref<T>(this IMemoryReader reader, long address, int[] offsets) where T : struct
    {
        long ptr = reader.Read(address, offsets);
        return reader.Read<T>(ptr);
    }

    public static T DerefPtr<T>(this IMemoryReader reader, long address, int[] offsets) where T : struct
    {
        long ptr = reader.ReadPtr(address, offsets);
        return reader.Read<T>(ptr);
    }
}