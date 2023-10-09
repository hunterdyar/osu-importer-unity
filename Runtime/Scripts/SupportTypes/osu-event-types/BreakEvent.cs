using UnityEngine;

namespace HDyar.OSUImporter
{
	public class BreakEvent : OSUEvent
	{
		public int endTime;
		
		public static bool IsEventType(string e)
		{
			return e.ToLower() == "break" || e == "2";
		}

		public new static bool TryParse(string line, out OSUEvent e)
		{
			Debug.Log("Parsing a Break");
			var props = line.Split(',');
			var b = new BreakEvent();
			b.raw = line;
			b.eventType = props[0];
			b.eventTypeInt = 2;
			int.TryParse(b.eventType, out b.eventTypeInt);
			b.startTime = 0;

			//background doesn't use starttime, should be 0.
			//parsing it anyway to find errors.
			if (!int.TryParse(props[1], out b.startTime))
			{
				Debug.LogError("cannot parse event start time. raw event: " + line);
			}

			if (!int.TryParse(props[2], out b.endTime))
			{
				Debug.LogError("cannot parse event start time. raw event: " + line);
			}
			e = b;
			return true;
		}
	}
}