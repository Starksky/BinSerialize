using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BinSerialize.Extensions
{
    public static class TypeExtension
    {
        public static FieldInfo[] GetReverseFields(this Type type)
        {
            var listFields = new List<FieldInfo>();
            
            if (type.BaseType != null)
                listFields.AddRange(GetReverseFields(type.BaseType));
            
            foreach (var field in type.GetFields())
                if(listFields.All(t => t.Name != field.Name))
                    listFields.Add(field);

            return listFields.ToArray();
        }
        
        public static FieldInfo[] GetReverseFields(this Type type, BindingFlags flags)
        {
            var listFields = new List<FieldInfo>();
            
            if (type.BaseType != null)
                listFields.AddRange(GetReverseFields(type.BaseType, flags));
            
            foreach (var field in type.GetFields(flags))
                if(listFields.All(t => t.Name != field.Name))
                    listFields.Add(field);

            return listFields.ToArray();
        }
        
        public static PropertyInfo[] GetReverseProperties(this Type type)
        {
            var listProperties = new List<PropertyInfo>();
            
            if (type.BaseType != null)
                listProperties.AddRange(GetReverseProperties(type.BaseType));
            
            foreach (var property in type.GetProperties())
                if(listProperties.All(t => t.Name != property.Name))
                    listProperties.Add(property);

            return listProperties.ToArray();
        }
        
        public static PropertyInfo[] GetReverseProperties(this Type type, BindingFlags flags)
        {
            var listFields = new List<PropertyInfo>();
            
            if (type.BaseType != null)
                listFields.AddRange(GetReverseProperties(type.BaseType, flags));
            
            foreach (var property in type.GetProperties(flags))
                if(listFields.All(t => t.Name != property.Name))
                    listFields.Add(property);

            return listFields.ToArray();
        }
    }
} 