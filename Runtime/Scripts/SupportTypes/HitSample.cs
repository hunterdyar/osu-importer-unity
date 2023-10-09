using System;
using UnityEngine;

namespace HDyar.OSUImporter
{
	[System.Serializable]
	public class HitSample
	{
		public SampleSet NormalSet;
		public SampleSet AdditionSet;
		public int Index;
		public int Volume;
		public string Filename;//for now
		public AudioClip Clip;
		//we use a static factory everywhere else but a constructor here. I should change that to be consistent.

		public HitSample()
		{
			
		}

		public void SetClip(AudioClip clip)
		{
			Clip = clip;
		}
		
		public static bool TryParse(string source, out HitSample sample)
		{
			if (source == "")
			{
				source = "0:0:0:0:";
			}

			sample = new HitSample();
			var data = source.Split(':');
			if (data.Length <= 3)
			{
				return false;
			}
			sample.NormalSet = (SampleSet)int.Parse(data[0]);
			sample.AdditionSet = (SampleSet)int.Parse(data[1]);
			sample.Index = int.Parse(data[2]);
			sample.Volume = int.Parse(data[3]);
			if (data.Length <= 4)
			{
				sample.Filename = "";
			}
			else
			{
				sample.Filename = data[^1];
			}

			sample.Clip = null;
			return true;
		}

		public static HitSample Parse(string line)
		{
			if (HitSample.TryParse(line, out var sample))
			{
				return sample;
			}
			//else...
			throw new FormatException($"Cannot parse '{line}' into HitSample");
		}

		
	}
}