using System;
using System.Collections;
using System.Collections.Generic;

namespace Serialization.Extensions
{
    public static class ListExtension
    {
        public static byte[] GetBytes(this IList value)
        {
            List<byte> data = new List<byte>();
                
            data.AddRange(ObjectExtension.GetBytes(value.Count));
            foreach (var item in value)
                data.AddRange(ObjectExtension.GetBytes(item));

            return data.ToArray(); 
        }
        public static IList GetValue(Type type, byte[] data, ref int offset)
        {
            int count = (int)ObjectExtension.GetValue(typeof(int), data, ref offset);
            
            IList result;
            if (type.GetElementType() != null)
            {
                result = Array.CreateInstance(type.GetElementType(), count);
                for (int i = 0; i < count; i++)
                {
                    var value = ObjectExtension.GetValue(type.GetElementType(), data, ref offset);
                    (result as Array).SetValue(value, i);
                }
            }
            else
            {
                result = type.Assembly.CreateInstance(type.FullName) as IList;
                for (int i = 0; i < count; i++)
                {
                    var value = ObjectExtension.GetValue(type.GetGenericArguments()[0], data, ref offset);
                    result.Add(value);
                }
            }

            return result;
        }
    }
}