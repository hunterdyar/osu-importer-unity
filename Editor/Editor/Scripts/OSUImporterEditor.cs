using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using HDyar.OSUImporter.Editor;

[CustomEditor(typeof(OSUImporter))]
public class OSUImporterEditor : ScriptedImporterEditor
{
	private bool showFoldout;
	public override void OnInspectorGUI()
	{
		var importer = (OSUImporter)target;
		var overrides = serializedObject.FindProperty("useOverrides");
		EditorGUILayout.PropertyField(overrides);
		SerializedProperty mapping;
		if (!overrides.boolValue)
		{
			GUI.enabled = false;
			mapping = serializedObject.FindProperty("sourceClipMapping");
			
		}
		else
		{
			mapping = serializedObject.FindProperty("overrideClipMapping");
		}

		showFoldout = EditorGUILayout.Foldout(showFoldout,"Audio Clip Mappings");
		if (showFoldout)
		{
			for (var i = 0; i < mapping.arraySize; i++)
			{
				EditorGUILayout.PropertyField(mapping.GetArrayElementAtIndex(i));
			}
		}
		

		
		// EditorGUILayout.PropertyField(prop, colorShift);
		base.ApplyRevertGUI();
	}
}
