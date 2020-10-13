using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(IntRangeAttribute))]
public class IntRangeDrawer :
    PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
        if (property.type != typeof(IntRange).ToString())
            Debug.LogWarning("Use only with IntRange type");
        else
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            IntRangeAttribute range = attribute as IntRangeAttribute;
            SerializedProperty minValue = property.FindPropertyRelative("RangeStart");
            SerializedProperty maxValue = property.FindPropertyRelative("RangeEnd");
            int iMin = minValue.intValue;
            int iMax = maxValue.intValue;
            float fMin = iMin;
            float fMax = iMax;

            // Calculate rects
            float space = 5;
            float textWidth = 30;
            var minRect = new Rect(position.x, position.y, textWidth, position.height);
            var mmRect = new Rect(minRect.x + minRect.width + space, position.y, position.width - (space + textWidth) * 2, position.height);
            var maxRect = new Rect(mmRect.x + mmRect.width + space, position.y, textWidth, position.height);
            
            EditorGUI.MinMaxSlider(mmRect, ref fMin, ref fMax, range.MinLimit, range.MaxLimit);
            iMin = Mathf.RoundToInt(fMin);
            iMax = Mathf.RoundToInt(fMax);
            iMin = EditorGUI.IntField(minRect, iMin);
            iMax = EditorGUI.IntField(maxRect, iMax);

            iMin = Mathf.Min(Mathf.Max(iMin, range.MinLimit), iMax, range.MaxLimit);
            iMax = Mathf.Max(Mathf.Min(iMax, range.MaxLimit), iMin, range.MinLimit);

            minValue.intValue = iMin;
            maxValue.intValue = iMax;
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}