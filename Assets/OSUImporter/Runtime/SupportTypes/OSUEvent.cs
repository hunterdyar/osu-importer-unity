using System;
using UnityEngine;

namespace HDyar.OSUImporter
{
	[System.Serializable]
	public struct OSUEvent
	{
		public string raw;
		public string eventType;
		/// <summary>
		/// -1 when not set by the osu file.
		/// </summary>
		public int eventTypeInt;
		public int startTime;
		public string[] eventParams;


		public static bool TryParse(string line, out OSUEvent e)
		{
			var lines = line.Split(',');
			e = new OSUEvent();
			e.raw = line;
			e.eventType = lines[0];
			e.eventTypeInt = -1;
			int.TryParse(e.eventType, out e.eventTypeInt);

			if (!int.TryParse(lines[1], out e.startTime))
			{
				Debug.LogError("cannot parse event start time. raw event: "+line);
			}

			if (lines.Length > 2)
			{
				e.eventParams = new string[lines.Length - 2];
				Array.Copy(lines, 2, e.eventParams, 0, lines.Length - 2);
			}
			

			return true;
		}
	}
}