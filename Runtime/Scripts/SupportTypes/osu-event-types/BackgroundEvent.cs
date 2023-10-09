using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace HDyar.OSUImporter
{
	/*
	 *
	 * 0,0,filename,xOffset,yOffset

    filename (String): Location of the background image relative to the beatmap directory. 
    Double quotes are usually included surrounding the filename, but they are not required.
    xOffset (Integer) and yOffset (Integer): Offset in osu! pixels from the centre of the screen. 
    For example, an offset of 50,100 would have the background shown 50 osu! 
    pixels to the right and 100 osu! pixels down from the centre of the screen. 
    If the offset is 0,0, writing it is optional.

	 */
	public class BackgroundEvent : OSUEvent
	{
		public string Filename;
		public int xOffset;
		public int yOffset;

		public static bool IsEventType(string e)
		{
			return e.ToLower() == "background" || e == "0";
		}

		public new static bool TryParse(string line, out OSUEvent e)
		{
			Debug.Log("Parsing a background");
			var props = line.Split(',');
			var b = new BackgroundEvent();
			b.raw = line;
			b.eventType = props[0];
			b.eventTypeInt = -1;
			int.TryParse(b.eventType, out b.eventTypeInt);
			b.startTime = 0;
			
			int.TryParse(props[1], out b.startTime);

			b.Filename = props[2].Trim('"');//trim double quotes from around file.

			if (props.Length > 3)
			{
				//todo tryParse and report errors.
				b.xOffset = int.Parse(props[3]);
				b.yOffset = int.Parse(props[4]);
			}

			e = b;
			return true;
		}
	}
}