using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Serialization.Extensions
{
    public static class ListExtension
    {
        public static byte[] GetBytes(this IList list)
        {
            List<byte> data = new List<byte>();
            
            data.AddRange(ObjectExtension.GetBytes(list.Count));

            foreach (var item in list)
                data.AddRange(BinarySerialization.GetBytes(item));

            return data.ToArray();
        }
        
        public static IList GetValue(Type type, byte[] data, ref int offset)
        {
            Type[] arguments = type.GetGenericArguments();
            Type valueType = arguments[0];

            var result = type.Assembly.CreateInstance(type.FullName) as IList;
            int count = (int)ObjectExtension.GetValue(typeof(int), data, ref offset);

            for (int i = 0; i < count; i++)
            {
                var value = BinarySerialization.GetValue(valueType, data, ref offset);
                if(value == null)
                    BinarySerialization.Deserialization(valueType, data, ref offset);
                result.Add(value);
            }
            
            return result;
        }
    }
}