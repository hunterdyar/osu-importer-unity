namespace HDyar.OSUImporter
{
	public class OSUGeneral : IOSUSection
	{
		public string AudioFilename;
		public int AudioLeadIn;
		public string AudioHash;//deprecated
		public int PreviewTime;
		public int Countdown;
		
		
		public bool AssignPropertyFromString(string property, object value)
		{
			//we can do this cleverly with reflection! Then use class instead of interface.
			if (property.ToLower() == "audiofilename")
			{
				AudioFilename = (string)value;
			}
			return true;
		}
	}
}