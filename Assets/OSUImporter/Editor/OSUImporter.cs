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
		[SerializeField] private AudioClip _audio;

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
				//First we get all of the text data for the beats
				beatmap.Init(data);
				//Then we pull back the audio clips we need, and update the asset settings... and grab them from the asset settings.
				
				//then we reassign these back into the data as direct references
				
				
				
				//create the assets.
				
				//for custom importers, I like having the raw text also available.
				ctx.AddObjectToAsset(assetName + " body", rawtext);
				rawtext.name = assetName + " raw text";
			}

			EditorUtility.SetDirty(beatmap);
			ctx.AddObjectToAsset("Beatmap", beatmap);
			ctx.SetMainObject(beatmap);
		}
	}
}