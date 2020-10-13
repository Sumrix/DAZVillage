using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

[
#if UNITY_EDITOR
    InitializeOnLoad,
#endif
     AttributeUsage(AttributeTargets.Field)
]
public class RequiredField : Attribute
{
#if UNITY_EDITOR
    private static bool isPlaying = true; // Без этого ошибка выходит по 2 раза
    static RequiredField()
    {
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
    }

    static void OnPlaymodeStateChanged()
    {
        if (EditorApplication.isPlaying && isPlaying)
            CheckRequiredFields();
    }

    [DidReloadScripts] //Optional if you want to check whenever scripts compile.
    static void CheckRequiredFields()
    {
        MonoBehaviour firstMissingReference = null;
        MonoBehaviour[] allComponents = (MonoBehaviour[])Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour));
        foreach (var obj in allComponents)
        {
            IEnumerable<FieldInfo> fieldInfos = obj.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where((f) => f.GetCustomAttributes(typeof(RequiredField), true)
                    .Any((o) => o is RequiredField));

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                object value = fieldInfo.GetValue(obj);
                if (value == null || value.Equals(null))
                {
                    firstMissingReference = obj;
                    
                    Debug.LogErrorFormat(
                        "The field '{0}'({1}.{2}) is not initialized",
                        obj.transform.GetPath(), obj.GetType().Name, fieldInfo.Name);
                }
            }
        }

        if (firstMissingReference != null && EditorApplication.isPlaying)
            firstMissingReference.StartCoroutine(StopEditorAtEndOfFrame());
    }

    static IEnumerator StopEditorAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        isPlaying = false;
        EditorApplication.isPlaying = false;
    }
#endif
}