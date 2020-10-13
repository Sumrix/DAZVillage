using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ClonableExtension
{
    private static MethodInfo CloneMethod;
    private static Dictionary<object, object> links;

    static ClonableExtension()
    {
        CloneMethod = typeof(object).
            GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
    }
    public static T Clone<T>(this T obj)
    {
        links = new Dictionary<object, object>();
        var newValue = (T)CloneObject(obj);
        links = null;
        return newValue;
    }
    private static object CloneObject(object oldValue)
    {
        if (oldValue == null)
        {
            return null;
        }
        object newValue;

        if (oldValue.GetType() == typeof(string))
        {
            string temp = ((string)oldValue);
            return temp;
        }
        else if (oldValue.GetType().IsArray)
        {
            var arr = (Array)oldValue;
            newValue = arr.Clone();
            int rank = oldValue.GetType().GetArrayRank();
            int[] indices = new int[rank];
            CopyArray((Array)newValue, (Array)oldValue, indices, rank - 1);
        }
        else
        {
            newValue = CloneMethod.Invoke(oldValue, null);
            DeepCloneObject(oldValue, newValue);
        }
        return newValue;
    }
    private static void CopyArray(Array dst, Array src, int[] indices, int rank)
    {
        int len = dst.GetLength(rank);
        for (int i = 0; i < len; i++)
        {
            indices[rank] = i;
            if (rank == 0)
            {
                dst.SetValue(CloneObject(src.GetValue(indices)), indices);
            }
            else
            {
                CopyArray(dst, src, indices, rank - 1);
            }
        }
    }
    private static void DeepCloneObject(object oldValue, object newValue)
    {
        var type = oldValue.GetType();
        if (type.IsClass
            && type != typeof(string))
        {
            object tmp;
            if (links.TryGetValue(oldValue, out tmp))
            {
                newValue = tmp;
            }
            else
            {
                foreach (var fieldInfo in type.GetAllFields().Where(x=>!x.IsSpecialName))
                {
                    var field = CloneObject(fieldInfo.GetValue(oldValue));
                    fieldInfo.SetValue(newValue, field);
                    links[oldValue] = newValue;
                }
            }
        }
    }
}