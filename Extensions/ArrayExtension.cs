using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Serialization.Extensions
{
    public static class ArrayExtension
    {
        public static byte[] GetBytes(this Array value)
        {
            List<byte> data = new List<byte>();
                
            data.AddRange(ObjectExtension.GetBytes(value.Length));
            foreach (var item in value)
                data.AddRange(ObjectExtension.GetBytes(item));

            return data.ToArray(); 
        }
        
        public static Array GetValue(Type type, byte[] data, ref int offset)
        {
            int count = (int)ObjectExtension.GetValue(typeof(int), data, ref offset);
            Array result = Array.CreateInstance(type.GetElementType(), count);

            for (int i = 0; i < count; i++)
            {
                var value = ObjectExtension.GetValue(type.GetElementType(), data, ref offset);
                result.SetValue(value, i);
            }

            return result;
        }
    }
}