using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BinSerialize.Extensions;
using Serialization.Extensions;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class NonSerializedAttribute : Attribute { }

namespace Serialization
{
    public class BinarySerialization
    {
        public static byte[] GetBytes(object obj)
        {
            var bytes = obj.GetBytes();
            if (bytes.Length > 0)
                return bytes;
            
            var type = obj.GetType();
            bool isStruct = type.IsValueType && !type.IsEnum;
            if (type.IsClass || isStruct)
                return Serialization(obj);

            return null;
        }

        public static object GetValue(Type type, in byte[] data, ref int offset)
        {
            var result = ObjectExtension.GetValue(type, data, ref offset);
            if(result != null)
                return result;
            
            return null;
        }

        public static byte[] Serialization(object obj)
        {
            Type type = obj.GetType();
            if (type.GetCustomAttribute(typeof(SerializableAttribute), true) == null) return default;

            if (obj is IDictionary || obj is IList)
            {
                var value = GetBytes(obj);
                if (value != null) return value;
            }
            
            List<byte> result = new List<byte>();
        
            foreach (var field in type.GetReverseFields())
            {
                if ( field.FieldType.GetCustomAttribute(typeof(SerializableAttribute), true) == null ||
                     field.GetCustomAttribute(typeof(NonSerializedAttribute), true) != null)
                    continue;

                //Debug.Log(field.Name);
                
                var data = GetBytes(field.GetValue(obj));
                result.AddRange(data);
            }
        
            foreach (var property in type.GetReverseProperties())
            {
                if ( property.PropertyType.GetCustomAttribute(typeof(SerializableAttribute), true) == null || 
                     property.GetCustomAttribute(typeof(NonSerializedAttribute), true) != null)
                    continue;
            
                var data = GetBytes(property.GetValue(obj));
                result.AddRange(data);
            }
        
            return result.ToArray();
        }
    
        public static object Deserialization(Type type, in byte[] data, ref int offset)
        {
            if (type.GetCustomAttribute(typeof(SerializableAttribute), true) == null) return default;

            if (type.GetInterfaces().Any(i => i == typeof(IList) || i == typeof(IDictionary)))
                return GetValue(type, data, ref offset);

            object result = type.Assembly.CreateInstance(type.FullName);

            foreach (var field in type.GetReverseFields())
            {
                if ( field.FieldType.GetCustomAttribute(typeof(SerializableAttribute), true) == null ||
                     field.GetCustomAttribute(typeof(NonSerializedAttribute), true) != null)
                    continue;

                field.SetValue(result, GetValue(field.FieldType, data, ref offset));
            }
        
            foreach (var property in type.GetReverseProperties())
            {
                if ( property.PropertyType.GetCustomAttribute(typeof(SerializableAttribute), true) == null || 
                     property.GetCustomAttribute(typeof(NonSerializedAttribute), true) != null)
                    continue;
                
                property.SetValue(result, GetValue(property.PropertyType, data, ref offset));
            }
        
            return result;
        }
    
        public static T Deserialization<T>(in byte[] data) where T: class, new()
        {
            try
            {
                Type type = typeof(T);
                int offset = 0;
                return Deserialization(type, data, ref offset) as T;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }
    }
}