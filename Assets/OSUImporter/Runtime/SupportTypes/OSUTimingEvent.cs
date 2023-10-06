using System;

namespace HDyar.OSUImporter
{
	[Serializable]
	public struct OSUTimingEvent
	{
		public int Time;
		public double beatLength;
		public int Meter;
		/// <summary>
		/// Same data as the DefaultSampleSet enum. Use this if you know file isn't using default sampleset.
		/// </summary>
		public int SampleSetInt;
		public SampleSet SampleSet;
		public int SampleIndex;
		public int Volume;
		public Inheritance Inheritance;
		/// <summary>
		/// Same data as the Inheritance enum, as the osu! file provides.
		/// </summary>
		public bool Uninherited;
		/// <summary>
		/// File data used to determin KiaiTime and whether or not to omit first barline.
		/// </summary>
		public int EffectsInt;
		public bool KiaiTime;
		public bool OmitFirstBarline;
		
		
		public static bool TryParse(string line, out OSUTimingEvent timingEvent)
		{
			//time,beatLength,meter,sampleSet,sampleIndex,volume,uninherited,effects
			var data = line.Split(',');
			timingEvent = new OSUTimingEvent();

			if (data.Length != 8)
			{
				return false;
			}
			if (!int.TryParse(data[0], out timingEvent.Time))
			{
				return false;
			}

			if (!double.TryParse(data[1], out timingEvent.beatLength))
			{
				return false;
			}

			if (!int.TryParse(data[2], out timingEvent.Meter))
			{
				return false;
			}

			if (int.TryParse(data[3], out timingEvent.SampleSetInt))
			{
				timingEvent.SampleSet = (SampleSet)timingEvent.SampleSetInt;
			}
			else
			{
				return false;
			}

			if (!int.TryParse(data[4], out timingEvent.SampleIndex))
			{
				return false;
			}

			if (!int.TryParse(data[5], out timingEvent.Volume))
			{
				return false;
			}

			if (int.TryParse(data[6], out var inheritedInt))
			{
				timingEvent.Uninherited = inheritedInt == 1;
				timingEvent.Inheritance = (Inheritance)inheritedInt;
			}
			else
			{
				return false;
			}

			if (int.TryParse(data[7], out var effects))
			{
				timingEvent.EffectsInt = effects;
				timingEvent.KiaiTime = (effects & 0b0001) == 1;
				timingEvent.OmitFirstBarline = (effects & 0b0100) == 1;
			}


			return true;
		}
	}
}