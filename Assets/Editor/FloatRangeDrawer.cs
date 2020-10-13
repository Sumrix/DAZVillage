using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(FloatRangeAttribute))]
public class FloatRangeDrawer :
    PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
        if (property.type != typeof(FloatRange).ToString())
            Debug.LogWarning("Use only with FloatRange type");
        else
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            FloatRangeAttribute range = attribute as FloatRangeAttribute;
            SerializedProperty minValue = property.FindPropertyRelative("RangeStart");
            SerializedProperty maxValue = property.FindPropertyRelative("RangeEnd");
            float fMin = minValue.floatValue;
            float fMax = maxValue.floatValue;

            // Calculate rects
            float space = 5;
            float textWidth = 30;
            var minRect = new Rect(position.x, position.y, textWidth, position.height);
            var mmRect = new Rect(minRect.x + minRect.width + space, position.y, position.width - (space + textWidth) * 2, position.height);
            var maxRect = new Rect(mmRect.x + mmRect.width + space, position.y, textWidth, position.height);

            EditorGUI.MinMaxSlider(mmRect, ref fMin, ref fMax, range.MinLimit, range.MaxLimit);
            fMin = EditorGUI.FloatField(minRect, fMin);
            fMax = EditorGUI.FloatField(maxRect, fMax);

            fMin = Mathf.Min(Mathf.Max(fMin, range.MinLimit), fMax, range.MaxLimit);
            fMax = Mathf.Max(Mathf.Min(fMax, range.MaxLimit), fMin, range.MinLimit);

            minValue.floatValue = fMin;
            maxValue.floatValue = fMax;

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}