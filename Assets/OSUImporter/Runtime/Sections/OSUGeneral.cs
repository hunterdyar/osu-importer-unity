using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEditor;
using UnityEngine;

namespace HDyar.OSUImporter
{
	[System.Serializable]
	public class OSUGeneral : OSUSection
	{
		public string AudioFilename;
		public int AudioLeadIn = 0;
		public string AudioHash;//deprecated
		public int PreviewTime = -1;
		public int Countdown = 1;
		public string SampleSet = "Normal";
		public float StackLeniency = 0.7f;
		public OSUGameMode Mode = OSUGameMode.osu;
		public bool LetterboxInBreaks = false;
		public bool StoryFireInFront = true;
		public bool UseSkinSprites = false;
		public bool AlwaysShowPlayfield = false;//deprecated
		public string OverlayPosition = "NoChange";//change to enum
		public string SkinPreference;
		public bool EpilepsyWarning = false;
		public int CountdownOffset = 0;
		public bool SpecialStyle = false;
		public bool WidescreenStoryboard = false;
		public bool SamplesMatchPlaybackRate = false;
	}
}