using UnityEngine;

namespace HDyar.OSUImporter
{
	public class OSUBeatmap : ScriptableObject
	{
		public OSUGeneral general;

		public void Init(string data)
		{

			IOSUSection currentSection = null;
			var lines = data.Split("/n");
			//delete the first line?
			foreach (var line in lines)
			{
				//skip doing anything on the first line.
				if (line == "oosu file format v14")
				{
					continue;
				}

				//skip empty lines
				if (line == "")
				{
					continue;
				}

				//if we hit a new section
				if (line[0] == '[')
				{
					currentSection = CreateEmptySectionByName(line);
				}
				
				//else, we must be on a property.
				if (currentSection != null)
				{
					currentSection.SetDataFromString(line);
				}
				
			}
		}

		private IOSUSection CreateEmptySectionByName(string line)
		{
			line = line.ToLower().Trim();
			if (line == "[general]")
			{
				general = new OSUGeneral();
				return general;
			}

			return null;
		}
	}
}