using System;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace HDyar.OSUImporter
{
	[ScriptedImporter(1, new[] { "osu" })]
	public class OSUImporter : ScriptedImporter
	{

		//todo: this needs to not be empty by default
		//but we don't know if there are any classes?
		//so throw errors correctly and skip frontmatter until assigned
		[SerializeField] private string selectedTypeName;

		public override void OnImportAsset(AssetImportContext ctx)
		{
			var beatmap = ScriptableObject.CreateInstance<OSUBeatmap>();
			var assetName = Path.GetFileNameWithoutExtension(ctx.assetPath);
			beatmap.name = assetName + " beatmap";

			//parse the text file.

			if (ctx.assetPath != null)
			{
				var data = File.ReadAllText(ctx.assetPath);
			
				var rawtext = new TextAsset(data);
				beatmap.Init(data);
				ctx.AddObjectToAsset(assetName + " body", rawtext);
				rawtext.name = assetName + " raw text";
			}

			EditorUtility.SetDirty(beatmap);
			ctx.AddObjectToAsset("Beatmap", beatmap);
			ctx.SetMainObject(beatmap);
		}
	}
}