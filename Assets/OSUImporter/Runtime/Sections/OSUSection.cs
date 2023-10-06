using System;
using System.Linq;
using UnityEngine;

namespace HDyar.OSUImporter
{
	public class OSUSection
	{
		public bool AssignPropertyFromString(string property, string value)
		{
			var properties = this.GetType().GetFields();

			foreach (var field in properties)
			{
				if (field.Name.ToLower() == property.ToLower())
				{
					if (field.FieldType == typeof(string))
					{
						field.SetValue(this, value);
						return true;
					}else if (field.FieldType == typeof(int))
					{
						if (int.TryParse(value, out int val))
						{
							field.SetValue(this, val);
							return true;
						}
						Debug.LogError($"osu import error. Cannot parse {value} to integer for {property}");
					}
					else if (field.FieldType == typeof(bool))
					{
						if (int.TryParse(value, out int ival))
						{
							field.SetValue(this, ival == 1);
							return true;
						}
						//if 0 or 1 doesn't work, we can try normal bool parse too.
						if (bool.TryParse(value, out bool val))
						{
							field.SetValue(this, val);
							return true;
						}

						Debug.LogError($"osu import error. Cannot parse {value} to bool for {property}");
					}
					else if (field.FieldType == typeof(float))
					{
						if (float.TryParse(value, out float val))
						{
							field.SetValue(this, val);
							return true;
						}

						Debug.LogError($"osu import error. Cannot parse {value} to float for {property}");
					}
					else if (field.FieldType == typeof(double))
					{
						if (double.TryParse(value, out double val))
						{
							field.SetValue(this, val);
							return true;
						}

						Debug.LogError($"osu import error. Cannot parse {value} to float for {property}");
					}
					else if (field.FieldType == typeof(OSUGameMode))
					{
						if (int.TryParse(value, out int val))
						{
							field.SetValue(this, (OSUGameMode)val);
							return true;
						}

						Debug.LogError($"osu import error. Cannot parse {value} to osugamemode (via int) for {property}");
					}
					else if (field.FieldType == typeof(int[]))
					{
						var vstrings = value.Split(',');
						
						//todo: try catch this so we can report error.
						var vints = vstrings.Select(int.Parse).ToArray(); 
						field.SetValue(this, vints);
						return true;
					}
					else if (field.FieldType == typeof(string[]))
					{
						var vstrings = Array.Empty<string>();
						if (value.Length>0)
						{
							vstrings = value.Split(' ');
						}
						field.SetValue(this, vstrings);
						return true;
					}
					else if (field.FieldType == typeof(OSUEvent))
					{
						if (OSUEvent.TryParse(value, out OSUEvent val))
						{
							field.SetValue(this, val);
							return true;
						}
					}
				}
			}
			
			Debug.LogWarning("Unable to Assign osu property "+property);
			return false;
		}

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