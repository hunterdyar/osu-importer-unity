using System;
using UnityEngine;

namespace HDyar.OSUImporter
{
	[Serializable]
	public class OSUEditor : OSUSection
	{
		public int[] Bookmarks;
		public double DistanceSpacing;
		public int BeatDivisor;
		public int GridSize;
		public double TimelineZoom;
	}
}