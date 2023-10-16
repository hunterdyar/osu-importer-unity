using System;
using System.Linq;
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
		public int EndTime = -1;
		public HitSound HitSound;
		public bool HitCircle;
		public bool Slider;
		public bool NewComboStart;
		public bool Spinner;
		public bool ManiaHold;
		public HitSample HitSample;
		/// <summary>
		/// How many combo colours to skip. Only relevant if the object starts a new combo.
		/// </summary>
		public int ColourHax;
		public bool ManiaHoldNote;
		public string[] ObjectParams;

		public int ManiaColumn(int colCount = 7)
		{
			//todo: pass in mania columnCount or turn this into a utility function.
			return Mathf.FloorToInt(X * colCount / 512f);
		}
		public static bool TryParse(string line, out OSUHitObject hit)
		{
			//time,beatLength,meter,sampleSet,sampleIndex,volume,uninherited,effects
			//483,192,1660,5,0,0:0:0:40:LR_CymbalCCR.wav
			
			//128,192,28199,128,0,28745:1:0:0:100:


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
			else
			{
				hit.ObjectParams = Array.Empty<string>();
			}

			var sampleLine = data[^1];
			if (sampleLine.Contains(':'))
			{
				//are we a mania hold or regular?
				if (sampleLine.Count(x => x == ':') == 5)
				{
					hit.ManiaHold = true;
					var all = sampleLine.Split(':');
					hit.EndTime = int.Parse(all[0]);
					//cut this part out and parse the sample like normal.
					sampleLine = sampleLine.Substring(sampleLine.IndexOf(':')+1);
				}
				
				
				if (HitSample.TryParse(sampleLine, out var hitSample))
				{
					hit.HitSample = hitSample;
				}
			}
			else
			{
				hit.HitSample = HitSample.Parse("0:0:0:0:");
			}
			
			//

			if (hit.ObjectParams.Length != 0)
			{
				//this is something different....
				//128,192,28199,128,0,28745:1:0:0:100:

			}
			else
			{
				
				//mania holds have same number of objectParams. We caught that already with the extra : above.
				if (!hit.ManiaHold)
				{
					hit.HitCircle = true;
				}
			}
			return true;
		}
	}
}