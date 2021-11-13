using System;

namespace Serialization.Extensions
{
    public static class ByteExtension
    {
        public static byte[] GetBytes(this byte value)
        {
            return new[]{ value };
        }
        public static byte GetValue(byte[] data)
        {
            return data[0];
        }
        public static byte GetValue(byte[] data, ref int offset)
        {
            return data[offset++];
        }
    }
}