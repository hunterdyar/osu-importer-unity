using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using HDyar.OSUImporter.Editor;

[CustomPropertyDrawer(typeof(AudioLookup))]
public class AudioLookupEditor : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		// Create property container element.
		var container = new VisualElement();

		// Create property fields.
		var filenameField = new PropertyField(property.FindPropertyRelative("originalFilename"));
		var clipField = new PropertyField(property.FindPropertyRelative("clip"));

		// Add fields to the container.
		//container.Add(filenameField);
		container.Add(clipField);

		return container; 
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var amountRect = new Rect(position.x, position.y, position.width, position.height);
		
		// Draw fields - pass GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("clip"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		
		EditorGUI.EndProperty();
	}
}
