using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Serialization.Extensions
{
    public static class ObjectExtension
    {
        public static byte[] GetBytes(this object value)
        {
            List<byte> data = new List<byte>();
            
            if (value is string s)
            {
                var bytes = s.GetBytes();
                data.AddRange(bytes.Length.GetBytes());
                data.AddRange(bytes);
                return data.ToArray();
            }
            
            if(value is int i)
            {
                var bytes = i.GetBytes();
                data.AddRange(bytes.Length.GetBytes());
                data.AddRange(bytes);
                return data.ToArray();
            }

            if (value is float f)
            {
                var bytes = f.GetBytes();
                data.AddRange(bytes.Length.GetBytes());
                data.AddRange(bytes);
                return data.ToArray();
            }

            if (value is byte b)
            {
                var bytes = b.GetBytes();
                data.AddRange(bytes.Length.GetBytes());
                data.AddRange(bytes);
                return data.ToArray();
            }
            
            if (value is bool bo)
            {
                var bytes = bo.GetBytes();
                data.AddRange(bytes.Length.GetBytes());
                data.AddRange(bytes);
                return data.ToArray();
            }
            
            if (value is IDictionary dictionary)
            {
                var bytes = dictionary.GetBytes();
                data.AddRange(bytes.Length.GetBytes());
                data.AddRange(bytes);
                return data.ToArray();
            }
            
            if (value is IList list)
            {
                var bytes = list.GetBytes();
                data.AddRange(bytes.Length.GetBytes());
                data.AddRange(bytes);
                return data.ToArray();
            }
            
            return default;
        }
        
        public static object GetValue(Type type, byte[] data, ref int offset)
        {
            int count = IntExtension.GetValue(data, ref offset);
            
            if (type == typeof(string))
                return StringExtension.GetValue(data, ref offset);

            if (type == typeof(int)) 
                return IntExtension.GetValue(data, ref offset);
        
            if (type == typeof(float)) 
                return FloatExtension.GetValue(data, ref offset);
        
            if(type == typeof(byte)) 
                return ByteExtension.GetValue(data, ref offset);

            if (type == typeof(bool))
                return BoolExtension.GetValue(data, ref offset);
            
            if (type.GetInterfaces().Any(i => i == typeof(IList)))
                return ListExtension.GetValue(type, data, ref offset);
            
            if (type.GetInterfaces().Any(i => i == typeof(IDictionary)))
                return DictionaryExtension.GetValue(type, data, ref offset);

            return GetValue(data, ref offset, count);
        }
        
        public static object GetValue(byte[] data, ref int offset, int count)
        {
            byte[] result = new byte[count];
            Array.Copy(data, offset, result, 0, count);
            offset += count;
            return result;
        }
        
        public static object GetValue(Type type, byte[] data)
        {
            int offset = 0;
            return GetValue(type, data, ref offset);
        }
    }
}