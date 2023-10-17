using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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
		public HitObjectType hitType;
		public int EndTime = -1;
		public HitSound HitSound;
		public HitSample HitSample;
		/// <summary>
		/// How many combo colours to skip. Only relevant if the object starts a new combo.
		/// </summary>
		public int ColourHax;
		public bool ManiaHoldNote;
		public string[] ObjectParams;
		
		//slider specific
		public CurveType CurveType;
		public Vector2Int[] CurvePoints;
		public int Slides = -1;
		public double Length = 0;
		public int[] edgeSounds;
		public (SampleSet normalSet, SampleSet additionalSet)[] EdgeSets;

		public bool IsCircle => (hitType & HitObjectType.Circle) != 0;
		public bool IsComboStart => (hitType & HitObjectType.ComboStart) != 0;
		public bool IsSpinner => (hitType & HitObjectType.Spinner) != 0;
		public bool IsSlider => (hitType & HitObjectType.Slider) != 0;

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
			
			if (int.TryParse(data[3], out var t))
			{
				hit.hitType = (HitObjectType)t;
			}
			else
			{
				return false;
			}

			hit.HitSound = HitSound.Normal;
			if (int.TryParse(data[4], out var hs))
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

			//hit type CIRCLE is nothing extra
			
			//hit type Slider
			if (hit.IsSlider)
			{
				var curveData = data[5].Split('|');
				//curve type
				hit.CurveType = LetterToCurveType(curveData[0]);
				//curve points
				hit.CurvePoints = new Vector2Int[curveData.Length - 1];
				for (int i = 1; i < curveData.Length; i++)
				{
					var points = curveData[i].Split(':');
					hit.CurvePoints[i-1] = new Vector2Int(int.Parse(points[0]), int.Parse(points[1]));
				}
				//slides
				hit.Slides = int.Parse(data[6]);
				hit.Length = float.Parse(data[7]);
				
				//it seems that edgeSounds and edgeSets are optional.
				if (data.Length > 8)
				{
					//edge sounds
					var edgeSounds = data[8].Split('|');
					hit.edgeSounds = new int[edgeSounds.Length];
					for (int i = 0; i < edgeSounds.Length; i++)
					{
						hit.edgeSounds[i] = int.Parse(edgeSounds[i]);
					}

					//edge sets
					var edgeSets = data[9].Split('|');
					hit.EdgeSets = new (SampleSet normalSet, SampleSet additionalSet)[edgeSounds.Length];
					for (int i = 0; i < edgeSets.Length; i++)
					{
						var sets = edgeSets[i].Split(':');
						hit.EdgeSets[i] = ((SampleSet)int.Parse(sets[0]), (SampleSet)int.Parse(sets[1]));
					}
				}
			}else if (hit.IsSpinner) //Hit type Spinner
			{
				hit.EndTime = int.Parse(data[5]);
			}else if (hit.IsComboStart)
			{
				// todo ?
			}
			
			
			
			//HitSample is always the last property.

			if (sampleLine.Contains(':'))
			{
				//are we a mania hold or regular?
				if (hit.hitType == HitObjectType.ManiaHold)
				{
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
				//no : data, use defaults.
				hit.HitSample = HitSample.Parse("0:0:0:0:");
			}
			
			//
			return true;
		}

		public static CurveType LetterToCurveType(string c)
		{
			if (string.IsNullOrEmpty(c))
			{
				return CurveType.NoCurve;
			}
			return CharToCurveType(c[0]);
		}

		public static CurveType CharToCurveType(char c)
		{
			switch (c)
			{
				case 'B':
				case 'b': return CurveType.Bezier;
				case 'C':
				case 'c': return CurveType.Catmull;
				case 'L': 
				case 'l': return CurveType.Linear;
				case 'P':
				case 'p': return CurveType.PerfectCircle;
				default: return CurveType.NoCurve;
			}
		}
	}
}