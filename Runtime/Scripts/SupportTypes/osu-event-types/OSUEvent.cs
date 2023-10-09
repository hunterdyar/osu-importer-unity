using System;
using System.Linq;
using System.Reflection;
using PlasticGui.EventTracking;
using UnityEngine;

namespace HDyar.OSUImporter
{
	[System.Serializable]
	public class OSUEvent
	{
		public string raw;
		public string eventType;
		/// <summary>
		/// -1 when not set by the osu file.
		/// </summary>
		public int eventTypeInt;
		public int startTime;
		
		//handle default
		public string[] eventParams;
		
		public new static bool TryParse(string line, out OSUEvent e)
		{
			var lines = line.Split(',');
			e = new OSUEvent();
			e.raw = line;
			e.eventType = lines[0];
			e.eventTypeInt = -1;
			int.TryParse(e.eventType, out e.eventTypeInt);

			if (!int.TryParse(lines[1], out e.startTime))
			{
				Debug.LogWarning("cannot parse event start time. raw event: "+line+". If you wan't to use this event, extend OSUEvent to support it. It is being ignored.");
				return false;
			}

			if (lines.Length > 2)
			{
				e.eventParams = new string[lines.Length - 2];
				Array.Copy(lines, 2, e.eventParams, 0, lines.Length - 2);
			}
			
			return true;
		}


		/// <summary>
		/// Loops through all children classes of OSUEvent and trys calling "IsEventType" on them, passing the first property of the event. It then parses and returns the appropriate child object.
		/// this, instead of a simple switch statement, so one can add their own child classes for extending to own events.
		/// .... you would need to add the child classes to the assembly.... wait is this pointless?
		/// This is slower, but it's at import time not runtime.
		/// </summary>
		public static OSUEvent GetOSUEventFromLine(string line)
		{
			var property = line.Split(',')[0];
			var sublassTypes = Assembly.GetAssembly(typeof(OSUEvent)).GetTypes()
				.Where(t => t.IsSubclassOf(typeof(OSUEvent)));
			foreach (var t in sublassTypes)
			{
				var isType = t.GetMethod("IsEventType").Invoke(null, new []{ property });
				if ((bool)isType)
				{
					object[] parameters = new object[] { line, null };
					var tryParse = t.GetMethod("TryParse").Invoke(null, parameters);
					if ((bool)tryParse)
					{
						return (OSUEvent)parameters[1];
					}
				}
			}
			//if this didn't work, return default type.
			if (TryParse(line, out var ev))
			{
				return ev;
			}
			
			Debug.LogError("Unable to parse event "+line);
			return null;
		}
	}
}