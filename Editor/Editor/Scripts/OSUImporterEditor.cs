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
		if (overrides != null)
		{
			EditorGUILayout.PropertyField(overrides);
		}
		else
		{
			base.ApplyRevertGUI();
			return;
		}
		SerializedProperty mapping = null;
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
			if (mapping != null)
			{
				for (var i = 0; i < mapping.arraySize; i++)
				{
					EditorGUILayout.PropertyField(mapping.GetArrayElementAtIndex(i));
				}
			}
		}
		

		
		// EditorGUILayout.PropertyField(prop, colorShift);
		base.ApplyRevertGUI();
	}
}
