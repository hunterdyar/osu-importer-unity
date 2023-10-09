namespace HDyar.OSUImporter
{
	public class SpriteEvent : OSUEvent
	{
		public string Layer;
		public string Origin;//todo make emum
		public string Filename;
		public int xOffset;
		public int yOffset;

		public static bool IsEventType(string e)
		{
			return e.ToLower() == "sprite";
		}

		public new static bool TryParse(string line, out OSUEvent e)
		{
			var props = line.Split(',');
			var b = new SpriteEvent();
			b.raw = line;
			b.eventType = props[0];
			b.Layer = props[1];
			b.Origin = props[2];
			b.Filename = props[3].Trim('"'); //trim double quotes from around file.

			if (props.Length > 4)
			{
				//todo tryParse and report errors.
				b.xOffset = int.Parse(props[4]);
				b.yOffset = int.Parse(props[5]);
			}

			e = b;
			return true;
		}
	}
}