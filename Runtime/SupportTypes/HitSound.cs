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
}