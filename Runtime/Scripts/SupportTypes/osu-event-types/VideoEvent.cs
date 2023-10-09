using System.Collections;
using System.Collections.Generic;
using HDyar.OSUImporter;
using UnityEngine;

public class VideoEvent : OSUEvent
{
    public string Filename;
    public int xOffset;
    public int yOffset;

    public static bool IsEventType(string e)
    {
        return e.ToLower() == "video" || e == "1";
    }

    public new static bool TryParse(string line, out OSUEvent e)
    {
        Debug.Log("Parsing a video event");
        var props = line.Split(',');
        var b = new VideoEvent();
        b.raw = line;
        b.eventType = props[0];
        b.eventTypeInt = 1;
        int.TryParse(b.eventType, out b.eventTypeInt);
        b.startTime = 0;

        int.TryParse(props[1], out b.startTime);

        b.Filename = props[2];

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
