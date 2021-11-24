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
                return s.GetBytes();

            if (value is int i)
                return i.GetBytes();

            if (value is float f)
                return f.GetBytes();

            if (value is byte b)
                return b.GetBytes();

            if (value is bool bo)
                return bo.GetBytes();

            if (value is IDictionary dictionary)
                return dictionary.GetBytes();

            if (value is IList list)
                return list.GetBytes();

            return data.ToArray();
        }
        
        public static object GetValue(Type type, byte[] data, ref int offset)
        {
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
            
            return GetValue(data, ref offset);
        }
        
        public static object GetValue(byte[] data, ref int offset)
        {
            int count = IntExtension.GetValue(data, ref offset);
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
        
        public static T GetValue<T>(this object value)
        {
            if (value is byte[] array)
            {
                int offset = 0;
                object result = null;
                
                if (typeof(T) == typeof(string))
                    result = StringExtension.GetValue(array, ref offset);

                else if (typeof(T) == typeof(int)) 
                    result = IntExtension.GetValue(array, ref offset);
        
                else if (typeof(T) == typeof(float)) 
                    result = FloatExtension.GetValue(array, ref offset);
        
                else if(typeof(T) == typeof(byte)) 
                    result = ByteExtension.GetValue(array, ref offset);

                else if (typeof(T) == typeof(bool))
                    result = BoolExtension.GetValue(array, ref offset);
            
                else if (typeof(T).GetInterfaces().Any(i => i == typeof(IList)))
                    result = ListExtension.GetValue(typeof(T), array, ref offset);
            
                else if (typeof(T).GetInterfaces().Any(i => i == typeof(IDictionary)))
                    result = DictionaryExtension.GetValue(typeof(T), array, ref offset);

                return (T)result;
            }
            return default;
        }
    }
}