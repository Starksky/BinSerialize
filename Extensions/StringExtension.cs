using System.Collections.Generic;
using System.Text;

namespace Serialization.Extensions
{
    public static class StringExtension
    {
        public static byte[] GetBytes(this string value)
        {
            List<byte> data = new List<byte>();
            var bytes = new UTF8Encoding(true).GetBytes(value);
            data.AddRange(bytes.Length.GetBytes());
            data.AddRange(bytes);
            return data.ToArray();
        }
        public static string GetValue(byte[] data)
        {
            return new UTF8Encoding(true).GetString(data);
        }
        public static string GetValue(byte[] data, ref int offset)
        {
            int count = IntExtension.GetValue(data, ref offset);
            string result = new UTF8Encoding(true).GetString(data, offset, count);
            offset += count;
            return result;
        }
    }
}