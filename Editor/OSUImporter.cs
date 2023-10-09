using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Serialization;

namespace HDyar.OSUImporter
{
	[ScriptedImporter(1, exts: new[] { "osu" }, importQueueOffset: 750)]
	public class OSUImporter : ScriptedImporter
	{
		public bool useOverrides = false;
		[SerializeField] private AudioLookup[] sourceClipMapping;
		[SerializeField] private AudioLookup[] overrideClipMapping;
		Dictionary<string, AudioClip> _map = new Dictionary<string, AudioClip>();

		//todo: this needs to not be empty by default
		//but we don't know if there are any classes?
		//so throw errors correctly and skip frontmatter until assigned
		[SerializeField] private AudioClip _audio;

		public override void OnImportAsset(AssetImportContext ctx)
		{
			var beatmap = ScriptableObject.CreateInstance<OSUBeatmap>();
			var assetName = Path.GetFileNameWithoutExtension(ctx.assetPath);
			var assetFolder = Path.GetDirectoryName(ctx.assetPath);
			beatmap.name = assetName + " beatmap";

			//parse the text file.

			if (ctx.assetPath != null)
			{
				var data = File.ReadAllText(ctx.assetPath);
			
				var rawtext = new TextAsset(data);
				//First we get all of the text data for the beats
				beatmap.Init(data);
				var filenames = beatmap.GetAllAudioClipFilenames();
				sourceClipMapping = new AudioLookup[filenames.Length];
				assetFolder = assetFolder.Replace("\\", "/");
				var searchFolders = new[]
				{
					assetFolder
				};
				
				//get the filenames.
				for (var i = 0; i < filenames.Length; i++)
				{
					
					//setup our overrides.
					string clipName = filenames[i];
					clipName = Path.GetFileNameWithoutExtension(clipName);
					sourceClipMapping[i].originalFilename = filenames[i];
					var clips = AssetDatabase.FindAssets(clipName, searchFolders);
					if (clips.Length > 0)
					{
						var clipPath = AssetDatabase.GUIDToAssetPath(clips[0]);
						sourceClipMapping[i].clip = AssetDatabase.LoadAssetAtPath<AudioClip>(clipPath);
					}
					else
					{
						sourceClipMapping[i].clip = null;
					}
				}
				
				//Initialize overrides.
				if (!useOverrides || overrideClipMapping == null || overrideClipMapping.Length == 0)
				{
					overrideClipMapping = sourceClipMapping;
				}
				else
				{
					//do we need to partially update overrides?
				}
				
				//Then we pull back the audio clips we need, and update the asset settings... and grab them from the asset settings.
				//then we reassign these back into the data as direct references
				_map.Clear();
				foreach (var lookup in overrideClipMapping)
				{
					_map.Add(lookup.originalFilename,lookup.clip);
				}

				beatmap.SetAudioClipsFromMap(_map);
				
				
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