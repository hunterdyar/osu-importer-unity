namespace HDyar.OSUImporter
{
	[System.Serializable]
	public struct HitSample
	{
		public SampleSet NormalSet;
		public SampleSet AdditionSet;
		public int Index;
		public int Volume;
		public string Filename;//for now

		//we use a static factory everywhere else but a constructor here. I should change that to be consistent.
		public HitSample(string source)
		{
			if (source == "")
			{
				source = "0:0:0:0:";
			}
			var data = source.Split(':');
			NormalSet = (SampleSet)int.Parse(data[0]);
			AdditionSet = (SampleSet)int.Parse(data[1]);
			Index = int.Parse(data[2]);
			Volume = int.Parse(data[3]);
			if (data.Length <= 4)
			{
				Filename = "";
			}
			else
			{
				Filename = data[4];
			}
		}
	}
}