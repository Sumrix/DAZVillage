using UnityEngine;
using System;
using System.Collections.Generic;

public static class GameObjectExtension
{
    public static string GetPath(this Transform transform)
    {
        string path = "/" + transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = "/" + transform.name + path;
        }
        return path;
    }
    public static GameObject FindParentWithTag<T>(this T childObject, string tag)
        where T : MonoBehaviour
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }
    public static object FindParentObjectOfType<T>(this T childObject, Type type)
        where T : MonoBehaviour
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            var component = t.parent.GetComponent(type);
            if (component != null)
            {
                return component;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given type.
    }
    public static object[] FindParentObjectsOfType<T>(this T childObject, Type type)
        where T : MonoBehaviour
    {
        Transform t = childObject.transform;
        List<object> components = new List<object>();
        while (t.parent != null)
        {
            var component = t.parent.GetComponent(type);
            if (component != null)
            {
                components.Add(component);
            }
            t = t.parent.transform;
        }
        return components.ToArray();
    }
    public static void LogError<T>(this T obj, string message)
        where T : MonoBehaviour
    {
        Debug.LogErrorFormat(
            "Error in '{0}'({1}): {2}",
            obj.transform.GetPath(), obj.GetType().Name, message);
    }
    public static void LogWarning<T>(this T obj, string message)
        where T : MonoBehaviour
    {
        Debug.LogWarningFormat(
            "Warning in '{0}'({1}): {2}",
            obj.transform.GetPath(), obj.GetType().Name, message);
    }
    public static void Log<T>(this T obj, string title, string message = "")
        where T : MonoBehaviour
    {
        Debug.LogFormat(
            "{3} in '{0}'({1}): {2}",
            obj.transform.GetPath(), obj.GetType().Name, message, title);
    }
    public static void Destroy<T>(this T obj)
        where T : MonoBehaviour
    {
        var core = obj as CoreObject;
        if (core != null)
        {
            core.Destroy();
        }
        else
        {
            GameObject.Destroy(obj.gameObject);
        }
    }
}
