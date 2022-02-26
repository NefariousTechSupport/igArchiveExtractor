namespace IGAE_GUI.IGZ
{
	public class IGZ_igObjectList : IGZ_igObject
	{
		public uint numObjects;
		public List<uint> offsets;

		public override void ReadWithFields()
		{
			base.ReadWithFields();

			numObjects = parent.sh.ReadUInt32();

			if(parent.version <= 0x06)
			{
				//numObjects--;
			}

			offsets = new List<uint>();

			parent.sh.BaseStream.Seek(absoluteOffset + 0x18, SeekOrigin.Begin);
			for(int i = 0; i < numObjects; i++)
			{
				uint currentOffset = parent.ReadOffset();
				if(currentOffset == 0xFFFFFFFF) continue;
				offsets.Add(currentOffset);
				Console.WriteLine($"object: {i.ToString("X04")}; offset: {offsets[i].ToString("X08")}");
			}
			numObjects = (uint)offsets.Count;
		}
	}
}