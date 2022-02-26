namespace IGAE_GUI.IGZ
{
	public class IGZ_igObject
	{
		public IGZ_File parent;
		public uint typeIndex;
		public uint unknown;
		public uint absoluteOffset;

		public static T ReadWithoutFields<T>(IGZ_File file)
		{
			T igObj = (T)typeof(T).GetConstructor(new Type[0]).Invoke(new object[0]);
			(igObj as IGZ_igObject).absoluteOffset = (uint)file.sh.BaseStream.Position;
			(igObj as IGZ_igObject).typeIndex = file.sh.ReadUInt32();
			(igObj as IGZ_igObject).unknown = file.sh.ReadUInt32();
			(igObj as IGZ_igObject).parent = file;
			return igObj;
		}

		public virtual void ReadWithFields()
		{
			parent.sh.BaseStream.Seek(absoluteOffset + 0x08, SeekOrigin.Begin);
		}
	}
}