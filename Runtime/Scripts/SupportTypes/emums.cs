using System;

namespace HDyar.OSUImporter
{
	[Flags]
	public enum HitSound
	{
		Normal = 0b_0000_0001,
		Whistle = 0b_0000_0010,
		Finish = 0b_0000_0100,
		Clap = 0b_0000_1000
	}

	[Flags]
	public enum HitObjectType
	{
		Circle = 0b_0000_0001,
		Slider = 0b_0000_0010,
		ComboStart = 0b_0000_0100,
		Spinner = 0b_0000_1000,
		ManiaHold = 0b_1000_0000,
	}
	
	public enum CurveType
	{
		NoCurve = 0,
		Bezier,
		Catmull,
		Linear,
		PerfectCircle
	}
}