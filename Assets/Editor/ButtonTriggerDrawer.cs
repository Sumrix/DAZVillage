using UnityEditor;
using UnityEngine;
using ActiveObjects.Triggers;

// RecipeItemDrawer
[CustomPropertyDrawer(typeof(ButtonTrigger))]
public class ButtonTriggerDrawer :
    PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);
        
        EditorGUI.PropertyField(position, property.FindPropertyRelative("Message"), label);
        //EditorGUI.LabelField(position, new GUIContent("", tooltip));

        EditorGUI.EndProperty();
    }
}