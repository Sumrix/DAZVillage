using UnityEditor;
using UnityEngine;
using GameUI;

// RecipeItemDrawer
[CustomPropertyDrawer(typeof(InspectorItem))]
public class InspectorItemDrawer :
    PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        SerializedProperty prefab = property.FindPropertyRelative("Prefab");
        Item item = (Item)prefab.objectReferenceValue;
        string name = item == null ? "None" : item.name;
        float labelWidth = position.width > 350 ? position.width * 0.36f : 120;
        var labelRect = new Rect(position.x, position.y, labelWidth, position.height);
        EditorGUI.LabelField(labelRect, name);
        position.x += labelWidth;
        position.width -= labelWidth;

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 40, position.height);
        var nameRect = new Rect(position.x + 45, position.y, position.width - 45, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("Count"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Prefab"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}