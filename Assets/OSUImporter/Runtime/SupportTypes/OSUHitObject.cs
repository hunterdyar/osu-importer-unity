using System;
using System.Numerics;
using UnityEngine;

namespace HDyar.OSUImporter
{
	[Serializable]
	public class OSUHitObject
	{
		[HideInInspector]
		public string Raw;
		public int X;
		public int Y;
		public Vector2Int Position;
		public int Time;

		
		public HitSound HitSound;
		public bool HitCircle;
		public bool Slider;
		public bool NewComboStart;
		public bool Spinner;
		public HitSample HitSample;
		/// <summary>
		/// How many combo colours to skip. Only relevant if the object starts a new combo.
		/// </summary>
		public int ColourHax;
		public bool ManiaHoldNote;
		public string[] ObjectParams;

		public static bool TryParse(string line, out OSUHitObject hit)
		{
			//time,beatLength,meter,sampleSet,sampleIndex,volume,uninherited,effects
			//483,192,1660,5,0,0:0:0:40:LR_CymbalCCR.wav

			var data = line.Split(',');
			hit = new OSUHitObject();
			hit.Raw = line;
			if (!int.TryParse(data[0], out hit.X))
			{
				return false;
			}

			if (!int.TryParse(data[1], out hit.Y))
			{
				return false;
			}

			hit.Position = new Vector2Int(hit.X, hit.Y);

			if (!int.TryParse(data[2], out hit.Time))
			{
				return false;
			}

			hit.HitSound = HitSound.Normal;
			if (int.TryParse(data[3], out var hs))
			{
				if (hs == 0)
				{
					hit.HitSound = HitSound.Normal;
				}
				else
				{
					hit.HitSound = (OSUImporter.HitSound)hs;
				}
		
			}
			else
			{
				//return false;
			}
			
			if (int.TryParse(data[4], out var t))
			{
				hit.HitCircle = (t & 0b00000001) > 0;
				hit.Slider = (t & 0b00000010) > 0;
				hit.NewComboStart = (t & 0b00000100) > 0;
				hit.Spinner = (t & 0b00001000) > 0;
				hit.ManiaHoldNote = (t & 0b10000000) > 0;
				hit.ColourHax = (t & 0b01110000) >> 4;
			}
			else
			{
				return false;
			}

			int par = data.Length - 6;
			if (par > 0)
			{
				hit.ObjectParams = new string[par];
				Array.Copy(data,5,hit.ObjectParams,0,par);
			}

			var sample = data[^1];
			if (sample.Contains(':'))
			{
				hit.HitSample = new HitSample(sample);
			}
			else
			{
				hit.HitSample = new HitSample("0:0:0:0:");
			}
			
			return true;
		}
	}
}