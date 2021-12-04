using System;

namespace BinSerialize.Extensions
{
    public static class LongExtension
    {
        public static byte[] GetBytes(this long value)
        {
            return BitConverter.GetBytes(value);
        }
        public static long GetValue(byte[] data)
        {
            return BitConverter.ToInt64(data, 0);
        }
        public static long GetValue(byte[] data, ref int offset)
        {
            long result = BitConverter.ToInt64(data, offset);
            offset += sizeof(long);
            return result;
        }
    }
}