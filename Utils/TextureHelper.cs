using System;
using System.Drawing;
using System.IO;

using BCnEncoder.Decoder;
using BCnEncoder.Encoder;
using BCnEncoder.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace IGAE_GUI.Utils
{
	/*public static class TextureHelper
	{
		private static readonly byte[] ddsHeader = new byte[0x80]
		{
			0x44, 0x44, 0x53, 0x20,		// "DDS "
			0x7C, 0x00, 0x00, 0x00,		// Version Info
			0x07, 0x10, 0x0A, 0x00,		// More Version Info
			0x00, 0x00, 0x00, 0x00,		// Height
			0x00, 0x00, 0x00, 0x00,		// Width
			0x00, 0x00, 0x00, 0x00,		// Size
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// Mipmaps 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x20, 0x00, 0x00, 0x00,		// 
			0x04, 0x00, 0x00, 0x00,		// 
			0x44, 0x58, 0x54, 0x30,		// "DXT0"
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x08, 0x10, 0x40, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00,		// 
			0x00, 0x00, 0x00, 0x00 		// 
		};
		public static Bitmap BitmapFromDDS(Stream dds)
		{
			BcDecoder dec = new BcDecoder();
			Image<Rgba32> image = dec.DecodeToImageRgba32(dds);
			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(image.Width, image.Height);
			for(int y = 0; y < image.Height; y++)
			{
		        Span<Rgba32> pixelRowSpan = image.GetPixelRowSpan(y);
				for(int x = 0; x < image.Width; x++)
				{
					bmp.SetPixel(x, image.Height - y - 1, System.Drawing.Color.FromArgb(pixelRowSpan[x].A, pixelRowSpan[x].R, pixelRowSpan[x].G, pixelRowSpan[x].B));
				}
			}
			return bmp;
		}
		public static void Extract(Stream src, Stream dst, uint width, uint height, uint size, uint mipmapCount, IGZ_TextureFormat format, bool leaveOpen = false)
		{
			dst.Write(ddsHeader, 0x00, 0x80);
			dst.Seek(0x0C, SeekOrigin.Begin);
			dst.Write(BitConverter.GetBytes((uint)height), 0x00, 0x04);
			dst.Write(BitConverter.GetBytes((uint)width), 0x00, 0x04);
			dst.Write(BitConverter.GetBytes(size), 0x00, 0x04);
			dst.Seek(0x1C, SeekOrigin.Begin);
			dst.Write(BitConverter.GetBytes((uint)mipmapCount), 0x00, 0x04);
			dst.Seek(0x57, SeekOrigin.Begin);
			switch(SimplifyTextureFormat(format))
			{
				case IGZ_TextureFormat.dxt1:
					dst.Write(BitConverter.GetBytes((byte)0x31), 0x00, 0x01);
					break;
				case IGZ_TextureFormat.dxt5:
					dst.Write(BitConverter.GetBytes((byte)0x35), 0x00, 0x01);
					break;
			}
			dst.Seek(0x80, SeekOrigin.Begin);
			byte[] srcBytes = new byte[size];
			src.Read(srcBytes, 0x00, (int)size);
			dst.Write(srcBytes, 0x00, (int)size);
			dst.Flush();
			if(!leaveOpen)
			{
				dst.Close();
			}
		}

		public static uint CalculateTextureSize(IGZ_TextureFormat simpliefiedFormat, uint width, uint height, uint mipmapCount = 1)
		{
			switch(simpliefiedFormat)
			{
				case IGZ_TextureFormat.dxt1:
				case IGZ_TextureFormat.dxt5:
					uint basicSize = (uint)(Math.Max( 1, ((width+3)/4) ) * Math.Max(1, ( (height + 3) / 4 ) ));		//Taken from the DDS programming guide
					uint finalSize = 0;
					for(uint i = 0; i < mipmapCount; i++)
					{
						finalSize += (uint)Math.Max(basicSize / Math.Pow(4, i), (simpliefiedFormat == IGZ_TextureFormat.dxt1 ? 0x08 : 0x10));
					}
					return finalSize * (uint)(simpliefiedFormat == IGZ_TextureFormat.dxt1 ? 0x08 : 0x10);
				//case IGZ_TextureFormat.dxt3:
				default:
					throw new NotImplementedException("This format is unsupported");
			}
		}
		public static void Replace(Stream src, Stream dst, uint width, uint height, uint size, uint mipmapCount, IGZ_TextureFormat format)
		{
			src.Seek(0x00, SeekOrigin.Begin);
			byte[] magic = new byte[4];
			src.Read(magic, 0x00, 0x04);
			Image<Rgba32> image = null;
			src.Seek(0x00, SeekOrigin.Begin);
			if(System.Text.Encoding.ASCII.GetString(magic) == " SDD")
			{
				BcDecoder dec = new BcDecoder();
				image = dec.DecodeToImageRgba32(src);
			}
			else
			{
				image = SixLabors.ImageSharp.Image.Load<Rgba32>(src);
			}

			image.Mutate(n => n.Resize((int)width, (int)height));

			IGZ_TextureFormat simpleFormat = SimplifyTextureFormat(format);

			BcEncoder enc = new BcEncoder();
			enc.OutputOptions.GenerateMipMaps = true;
			enc.OutputOptions.MaxMipMapLevel = (int)mipmapCount;
			enc.OutputOptions.Quality = CompressionQuality.BestQuality;
			enc.OutputOptions.FileFormat = BCnEncoder.Shared.OutputFileFormat.Dds;
			if(simpleFormat == IGZ_TextureFormat.dxt1)
			{
				enc.OutputOptions.Format = BCnEncoder.Shared.CompressionFormat.Bc1;
			}
			else if(simpleFormat == IGZ_TextureFormat.dxt5)
			{
				Console.WriteLine("DXT5 Texture");
				enc.OutputOptions.Format = BCnEncoder.Shared.CompressionFormat.Bc3;
			}
			MemoryStream oms = new MemoryStream((int)size + 0x80);
			//(int)CalculateTextureSize(IGZ_TextureFormat.dxt1, width, height, mipmapCount)
			enc.EncodeToStream(image, oms);
			oms.Seek(0x80, SeekOrigin.Begin);
			oms.CopyTo(dst);
			oms.Close();
			dst.Flush();
		}
		public static IGZ_TextureFormat SimplifyTextureFormat(IGZ_TextureFormat format)
		{
			switch(format)
			{
				case IGZ_TextureFormat.dxt1:
				case IGZ_TextureFormat.dxt1_android:
				case IGZ_TextureFormat.dxt1_big_ps3:
				case IGZ_TextureFormat.dxt1_big_wii:
				case IGZ_TextureFormat.dxt1_big_xenon:
				case IGZ_TextureFormat.dxt1_cafe:
				case IGZ_TextureFormat.dxt1_durango:
				case IGZ_TextureFormat.dxt1_dx:
				case IGZ_TextureFormat.dxt1_dx11:
				case IGZ_TextureFormat.dxt1_lgtv:
				case IGZ_TextureFormat.dxt1_linux:
				case IGZ_TextureFormat.dxt1_nx:
				case IGZ_TextureFormat.dxt1_old:
				case IGZ_TextureFormat.dxt1_osx:
				case IGZ_TextureFormat.dxt1_ps4:
				case IGZ_TextureFormat.dxt1_wgl:
				case IGZ_TextureFormat.dxt1_srgb:
				case IGZ_TextureFormat.dxt1_srgb_android:
				case IGZ_TextureFormat.dxt1_srgb_big_ps3:
				case IGZ_TextureFormat.dxt1_srgb_big_wii:
				case IGZ_TextureFormat.dxt1_srgb_big_xenon:
				case IGZ_TextureFormat.dxt1_srgb_cafe:
				case IGZ_TextureFormat.dxt1_srgb_durango:
				case IGZ_TextureFormat.dxt1_srgb_dx:
				case IGZ_TextureFormat.dxt1_srgb_dx11:
				case IGZ_TextureFormat.dxt1_srgb_linux:
				case IGZ_TextureFormat.dxt1_srgb_nx:
				case IGZ_TextureFormat.dxt1_srgb_osx:
				case IGZ_TextureFormat.dxt1_srgb_ps4:
				case IGZ_TextureFormat.dxt1_srgb_wgl:
				case IGZ_TextureFormat.dxt1_tile:
				case IGZ_TextureFormat.dxt1_tile_android:
				case IGZ_TextureFormat.dxt1_tile_big_ps3:
				case IGZ_TextureFormat.dxt1_tile_big_wii:
				case IGZ_TextureFormat.dxt1_tile_big_xenon:
				case IGZ_TextureFormat.dxt1_tile_cafe:
				case IGZ_TextureFormat.dxt1_tile_durango:
				case IGZ_TextureFormat.dxt1_tile_dx:
				case IGZ_TextureFormat.dxt1_tile_dx11:
				case IGZ_TextureFormat.dxt1_tile_linux:
				case IGZ_TextureFormat.dxt1_tile_nx:
				case IGZ_TextureFormat.dxt1_tile_osx:
				case IGZ_TextureFormat.dxt1_tile_ps4:
				case IGZ_TextureFormat.dxt1_tile_wgl:
					return IGZ_TextureFormat.dxt1;
				case IGZ_TextureFormat.dxt5:
				case IGZ_TextureFormat.dxt5_android:
				case IGZ_TextureFormat.dxt5_big_ps3:
				case IGZ_TextureFormat.dxt5_big_wii:
				case IGZ_TextureFormat.dxt5_big_xenon:
				case IGZ_TextureFormat.dxt5_cafe:
				case IGZ_TextureFormat.dxt5_durango:
				case IGZ_TextureFormat.dxt5_dx:
				case IGZ_TextureFormat.dxt5_dx11:
				case IGZ_TextureFormat.dxt5_lgtv:
				case IGZ_TextureFormat.dxt5_linux:
				case IGZ_TextureFormat.dxt5_nx:
				case IGZ_TextureFormat.dxt5_old:
				case IGZ_TextureFormat.dxt5_osx:
				case IGZ_TextureFormat.dxt5_ps4:
				case IGZ_TextureFormat.dxt5_wgl:
				case IGZ_TextureFormat.dxt5_srgb:
				case IGZ_TextureFormat.dxt5_srgb_android:
				case IGZ_TextureFormat.dxt5_srgb_big_ps3:
				case IGZ_TextureFormat.dxt5_srgb_big_wii:
				case IGZ_TextureFormat.dxt5_srgb_big_xenon:
				case IGZ_TextureFormat.dxt5_srgb_cafe:
				case IGZ_TextureFormat.dxt5_srgb_durango:
				case IGZ_TextureFormat.dxt5_srgb_dx:
				case IGZ_TextureFormat.dxt5_srgb_dx11:
				case IGZ_TextureFormat.dxt5_srgb_linux:
				case IGZ_TextureFormat.dxt5_srgb_nx:
				case IGZ_TextureFormat.dxt5_srgb_osx:
				case IGZ_TextureFormat.dxt5_srgb_ps4:
				case IGZ_TextureFormat.dxt5_srgb_wgl:
				case IGZ_TextureFormat.dxt5_tile:
				case IGZ_TextureFormat.dxt5_tile_android:
				case IGZ_TextureFormat.dxt5_tile_big_ps3:
				case IGZ_TextureFormat.dxt5_tile_big_wii:
				case IGZ_TextureFormat.dxt5_tile_big_xenon:
				case IGZ_TextureFormat.dxt5_tile_cafe:
				case IGZ_TextureFormat.dxt5_tile_durango:
				case IGZ_TextureFormat.dxt5_tile_dx:
				case IGZ_TextureFormat.dxt5_tile_dx11:
				case IGZ_TextureFormat.dxt5_tile_linux:
				case IGZ_TextureFormat.dxt5_tile_nx:
				case IGZ_TextureFormat.dxt5_tile_osx:
				case IGZ_TextureFormat.dxt5_tile_ps4:
				case IGZ_TextureFormat.dxt5_tile_wgl:
					return IGZ_TextureFormat.dxt5;
				default:
					throw new NotImplementedException($"{((uint)format).ToString("X08")} is unsupported");
			}
		}
	}*/
}