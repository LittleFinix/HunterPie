using HunterPie.Core.Logger;
using System;
using System.Linq;

namespace HunterPie.Core.Address.Map.Internal;

internal static class AddressMapParserExtensions
{
    private static int[] ParseStringToVecInt32(string stringified)
    {
        return stringified.Split(",")
                .Select(element => Convert.ToInt32(element.Trim(), 16))
                .ToArray();
    }

    public static void AddValueByType(this IAddressMapParser self, AddressMapKeyWords.AddressMapType type, string key, string value)
    {
        void TryAdd<T>(string key, T value)
        {
            try
            {
                self.Add(key, value);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        switch (type)
        {
            case AddressMapKeyWords.AddressMapType.Long:
                TryAdd(key, Convert.ToInt64(value, 16));
                break;

            case AddressMapKeyWords.AddressMapType.VecInt32:
                TryAdd(key, ParseStringToVecInt32(value));
                break;
        }
    }
}
