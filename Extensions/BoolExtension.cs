using System;
using System.Collections.Generic;

namespace Serialization.Extensions
{
    public static class BoolExtension
    {
        public static byte[] GetBytes(this bool value)
        {
            return BitConverter.GetBytes(value);
        }
        public static bool GetValue(byte[] data)
        {
            return BitConverter.ToBoolean(data, 0);
        }
        public static bool GetValue(byte[] data, ref int offset)
        {
            bool result = BitConverter.ToBoolean(data, offset);
            offset += 1;
            return result;
        }
    }
}