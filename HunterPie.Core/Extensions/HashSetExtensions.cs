using System.Collections.Generic;

namespace HunterPie.Core.Extensions;

public static class HashSetExtensions
{

    public static T? FindPriority<T>(this HashSet<T> self, T[] values)
    {
        foreach (T value in values)
        {
            if (self.Contains(value))
                return value;
        }

        return default;
    }

}
