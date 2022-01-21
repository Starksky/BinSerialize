using System;
using System.Collections.Generic;

namespace Serialization.Extensions
{
    public static class DoubleExtension
    {
        public static byte[] GetBytes(this double value)
        {
            return BitConverter.GetBytes(value);
        }
        public static double GetValue(byte[] data)
        {
            return BitConverter.ToDouble(data, 0);
        }
        public static double GetValue(byte[] data, ref int offset)
        {
            double result = BitConverter.ToDouble(data, offset);
            offset += sizeof(double);
            return result;
        }
    }
}