﻿namespace HDyar.OSUImporter
{
	[System.Serializable]
	public class OSUMetadata : OSUSection
	{
		public string Title;
		public string TitleUnicode;
		public string Artist;
		public string ArtistUnicode;
		public string Creator;
		public string Version;
		public string Source;
		public string[] Tags;
		public int BeatmapID;
		public int BeatmapSetID;
	}
}