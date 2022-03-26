using System;
using System.Linq;
using System.IO;
using IGAE_GUI.IGZ;

using BCnEncoder.Decoder;
using BCnEncoder.Encoder;
using BCnEncoder.ImageSharp;
using SixLabors.ImageSharp;

namespace IGAE_GUI.Types
{
	public class igImage2 : igObject
	{
		ushort width;
		ushort height;
		ushort depth;
		ushort mipmapCount;
		ushort array;
		IGZ_TextureFormat format;
		uint index;
		uint textureOffset;
		uint textureSize;
		int textureSection;

		public igImage2(igObject basic)
		{
			_container = basic._container;
			offset     = basic.offset;
			name       = basic.name;
			itemCount  = basic.itemCount;
			length     = basic.length;
			data       = basic.data;
			fields     = basic.fields;
			children   = basic.children;
		}

		public override void ReadObjectFields()
		{
			if(_container.version == 0x09)
			{
				_container.ebr.BaseStream.Seek(offset + 0x34, SeekOrigin.Begin);
			}
			else
			{
				_container.ebr.BaseStream.Seek(offset + 0x30, SeekOrigin.Begin);
			}
			width       = _container.ebr.ReadUInt16();
			height      = _container.ebr.ReadUInt16();
			depth       = _container.ebr.ReadUInt16();
			mipmapCount = _container.ebr.ReadUInt16();
			array       = _container.ebr.ReadUInt16();

			_container.ebr.BaseStream.Seek(0x04, SeekOrigin.Current);

			IGZ_EXID exid = _container.fixups.First(x => x.magicNumber == 0x45584944) as IGZ_EXID;
			format = (IGZ_TextureFormat)exid.hashes[_container.ebr.ReadUInt16()];

			_container.ebr.BaseStream.Seek(0x08, SeekOrigin.Current);
			IGZ_TMHN tmhn = _container.fixups.First(x => x.magicNumber == 0x544D484E) as IGZ_TMHN;
			index = _container.ebr.ReadUInt32();
			textureOffset = tmhn.offsets[index];
			textureSize   = tmhn.sizes[index];

			textureSection = (int)_container.objectSectionSpan + 1;
			if(_container.version == 0x09)
			{
				textureSection++;
			}

			Console.WriteLine($"Image Found: {width}, {height}, {mipmapCount}, {index}, {format.ToString()}");
		}
		public void Extract(Stream output)
		{
			Console.WriteLine((_container.descriptors[textureSection].offset + textureOffset).ToString("X08"));
			_container.ebr.BaseStream.Seek(_container.descriptors[textureSection].offset + textureOffset, SeekOrigin.Begin);
			IGAE_GUI.Utils.TextureHelper.Extract(_container.ebr.BaseStream, output, width, height, textureSize, mipmapCount, format, true);
		}
		public void Replace(Stream input)
		{
			_container.ebr.BaseStream.Seek(_container.descriptors[textureSection].offset + textureOffset, SeekOrigin.Begin);
			IGAE_GUI.Utils.TextureHelper.Replace(input, _container.ebr.BaseStream, width, height, textureSize, mipmapCount, format);
			input.Close();
		}
	}
}