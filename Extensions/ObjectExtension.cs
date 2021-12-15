using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BinSerialize.Extensions;
using UnityEngine;

namespace Serialization.Extensions
{
    public static class ObjectExtension
    {
        public static byte[] GetBytes(this object value)
        {
            List<byte> data = new List<byte>();

            if (value is string s)
                return StringExtension.GetBytes(s);

            if (value is int i)
                return IntExtension.GetBytes(i);

            if (value is float f)
                return FloatExtension.GetBytes(f);

            if (value is long l)
                return LongExtension.GetBytes(l);
            
            if (value is byte b)
                return ByteExtension.GetBytes(b);

            if (value is bool bo)
                return BoolExtension.GetBytes(bo);
            
            if (value is Enum)
                return IntExtension.GetBytes(((int)value));
            
            if (value is Array array)
                return ArrayExtension.GetBytes(array);
            
            if (value is IDictionary dictionary)
                return DictionaryExtension.GetBytes(dictionary);

            if (value is IList list)
                return ListExtension.GetBytes(list);
            
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
            
            if (type == typeof(long)) 
                return LongExtension.GetValue(data, ref offset);
        
            if(type == typeof(byte)) 
                return ByteExtension.GetValue(data, ref offset);

            if (type == typeof(bool))
                return BoolExtension.GetValue(data, ref offset);
            
            if (type.IsEnum)
                return EnumExtension.GetValue(type, data, ref offset);
            
            if (type.IsArray)
                return ArrayExtension.GetValue(type, data, ref offset);
            
            if (type.GetInterfaces().Any(i => i == typeof(IList)))
                return ListExtension.GetValue(type, data, ref offset);
            
            if (type.GetInterfaces().Any(i => i == typeof(IDictionary)))
                return DictionaryExtension.GetValue(type, data, ref offset);

            if(type.IsClass)
                return BinarySerialization.Deserialization(type, data, ref offset);
            
            return default;
        }
        
        public static object GetValue(byte[] data, ref int offset)
        {
            int count = IntExtension.GetValue(data, ref offset);
            byte[] result = new byte[count];
            Array.Copy(data, offset, result, 0, count);
            offset += count;
            return result;
        }
        
        public static object GetValue(Type type, byte[] data, int startOffset = 0)
        {
            int offset = startOffset;
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
                
                else if (typeof(T) == typeof(long)) 
                    result = LongExtension.GetValue(array, ref offset);
                
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