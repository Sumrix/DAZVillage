using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

public static class TypeHelper
{
    public static bool CanConvert(Type to, Type from)
    {
        return to.IsAssignableFrom(from)
            || to.GetMethod("op_Implicit", new[] { from }) != null
            || to.GetMethod("op_Explicit", new[] { from }) != null;
    }
    public static object GetDefault(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }
    public static IEnumerable<FieldInfo> GetAllFields(this Type type)
    {
        if (type == null)
        {
            return Enumerable.Empty<FieldInfo>();
        }

        BindingFlags flags = BindingFlags.Public |
                             BindingFlags.NonPublic |
                             BindingFlags.Instance;
        
        return type.GetFields(flags).Union(GetAllFields(type.BaseType));
    }
}
