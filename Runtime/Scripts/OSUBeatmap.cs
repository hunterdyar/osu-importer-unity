using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HDyar.OSUImporter
{
	public class OSUBeatmap : ScriptableObject
	{
		public OSUGeneral General;
		public OSUEditor Editor;
		public OSUMetadata Metadata;
		public OSUDifficulty Difficulty;
		[SerializeReference]
		public OSUEvent[] Events;
		public OSUTimingEvent[] TimingEvents;
		public OSUHitObject[] HitObjects;

		//todo: make as static factory?
		public void Init(string data)
		{
			int parseSection = -1; //0 = kv pairs, 1 = events, 2 = timingpoints, 3 = hitobjects
			OSUSection currentSection = null;
			var lines = data.Split('\n');
			List<OSUEvent> osuevents = new List<OSUEvent>();
			List<OSUTimingEvent> timingevents = new List<OSUTimingEvent>();
			List<OSUHitObject> hitObjects = new List<OSUHitObject>();
			//delete the first line?
			foreach (var linedata in lines)
			{
				var line = linedata.Trim();
				//skip doing anything on the first line.
				if (line == "osu file format v14")
				{
					continue;
				}

				//skip empty lines and comments.
				if (line == "" || line.Substring(0,2) == "//")
				{
					continue;
				}

				//if we hit a new section
				if (line[0] == '[')
				{
					currentSection = SetEmptySectionByName(line, ref parseSection);
					continue;
				}

				if (parseSection == 0)
				{
					//parse property for Key:Value section. 
					if (currentSection != null)
					{
						currentSection.SetDataFromString(line);
						continue;
					}
					else
					{
						Debug.LogError("parsing error.");
					}
				}else if (parseSection == 1)
				{
					//parse events
					var e = OSUEvent.GetOSUEventFromLine(line);
					if (e != null)
					{
						osuevents.Add(e);
					}
				}
				else if (parseSection == 2)
				{
					//parse events
					if (OSUTimingEvent.TryParse(line, out var te))
					{
						timingevents.Add(te);
					}
				}else if (parseSection == 3)
				{
					//parse events
					if (OSUHitObject.TryParse(line, out var hit))
					{
						hitObjects.Add(hit);
					}
				}
			}

			Events = osuevents.ToArray();
			TimingEvents = timingevents.ToArray();
			HitObjects = hitObjects.ToArray();
		}

		public OSUSection SetEmptySectionByName(string line, ref int parseSection)
		{
			//strip brackets and whitespace, and lowercase it.
			line = line.Replace("[","").Replace("]","").ToLower().Trim();
			if (line == "general")
			{
				General = new OSUGeneral();
				parseSection = 0;
				return General;
			}else if (line == "editor")
			{
				Editor = new OSUEditor();
				parseSection = 0;
				return Editor;
			}
			else if (line == "metadata")
			{
				Metadata = new OSUMetadata();
				parseSection = 0;
				return Metadata;
			}
			else if (line == "difficulty")
			{
				Difficulty = new OSUDifficulty();
				parseSection = 0;
				return Difficulty;
			}else if (line == "events")
			{
				//events = new event...
				parseSection = 1;
				return null;
			}else if (line == "timingpoints")
			{
				parseSection = 2;
				return null;
			}
			else if (line == "hitobjects")
			{
				parseSection = 3;
				return null;
			}

			//a section we don't support... yet?
			parseSection = -1;
			return null;
		}

		public string[] GetAllAudioClipFilenames()
		{
			//hashsets are garunteed unique, so we use this type to avoid "if includes" checks
			HashSet<string> names = new HashSet<string> { General.AudioFilename };
			foreach (OSUHitObject hitObject in HitObjects)
			{
				names.Add(hitObject.HitSample.Filename);
			}

			if (names.Contains(""))
			{
				names.Remove("");
			}

			return names.ToArray();
		}

		public void SetAudioClipsFromMap(Dictionary<string, AudioClip> map)
		{
			if(map.TryGetValue(General.AudioFilename,out var clip))
			{
				General.Clip = clip;
			}

			for (var i = 0; i < HitObjects.Length; i++)
			{
				var hit = HitObjects[i];
				if (map.TryGetValue(hit.HitSample.Filename, out var hitclip))
				{
					hit.HitSample.SetClip(hitclip);
				}
			}
		}
	}
}