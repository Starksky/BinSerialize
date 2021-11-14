using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Serialization.Extensions
{
    public static class DictionaryExtension
    {
        public static byte[] GetBytes(this IDictionary dictionary)
        {
            List<byte> data = new List<byte>();
            
            data.AddRange(ObjectExtension.GetBytes(dictionary.Count));
            
            foreach (var item in dictionary)
            {
                var property = item.GetType().GetProperty("Key");
                var value = property.GetValue(item);
                data.AddRange(ObjectExtension.GetBytes(value));
                
                property = item.GetType().GetProperty("Value");
                value = property.GetValue(item);
                data.AddRange(ObjectExtension.GetBytes(value));
            }

            return data.ToArray();
        }
        
        public static IDictionary GetValue(Type type, byte[] data, ref int offset)
        {
            Type[] arguments = type.GetGenericArguments();
            Type keyType = arguments[0];
            Type valueType = arguments[1];
        
            var result = type.Assembly.CreateInstance(type.FullName) as IDictionary;
            int count = (int)ObjectExtension.GetValue(typeof(int), data, ref offset);
            
            for (int i = 0; i < count; i++)
            {
                var key = ObjectExtension.GetValue(keyType, data, ref offset);
                var value = ObjectExtension.GetValue(valueType, data, ref offset);
                result.Add(key, value);
            }

            return result;
        }
    }
}