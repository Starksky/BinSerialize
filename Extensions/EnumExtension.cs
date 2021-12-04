using System;
using System.Reflection;

namespace BinSerialize.Extensions
{
    public static class EnumExtension
    {
        public static byte[] GetBytes(this Enum value)
        {
            var type = value.GetType();
            var assembly = type.Assembly;
            object result = new object();
            FieldInfo field = type.GetField(value.ToString());
            field.GetValue(result);
            return BitConverter.GetBytes((int)result);
        }
        public static T GetValue<T>(byte[] data) where T: Enum
        {
            return (T)Enum.Parse(typeof(T),BitConverter.ToInt32(data, 0).ToString());
        }
        public static object GetValue(Type type, byte[] data, ref int offset)
        {
            var result = Enum.Parse(type,BitConverter.ToInt32(data, offset).ToString());
            offset += 4;
            return result;
        }
        public static T GetValue<T>(byte[] data, ref int offset)
        {
            T result = (T)Enum.Parse(typeof(T),BitConverter.ToInt32(data, offset).ToString());
            offset += 4;
            return result;
        }
    }
}