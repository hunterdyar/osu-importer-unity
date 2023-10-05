namespace HDyar.OSUImporter
{
	public interface IOSUSection
	{
		public bool AssignPropertyFromString(string property, object value);

		public bool SetDataFromString(string line)
		{
			var lineData = line.Split(':');

			if (lineData.Length == 2)
			{
				return AssignPropertyFromString(lineData[0].Trim(), lineData[1].Trim());
			}
			//process as CSV. Hit samples may contain:, but we know what section will use them.
			return false;
		}
	}
}