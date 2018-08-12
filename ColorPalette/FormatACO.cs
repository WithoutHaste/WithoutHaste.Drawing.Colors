﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WithoutHaste.Drawing.ColorPalette
{
	/// <summary>
	/// Photoshop *.aco color palette file format.
	/// </summary>
	internal class FormatACO
	{
//		private static Rounding PHOTOSHOP_ROUNDING = Rounding.Down;

		private ColorPalette colorPalette;
		public ColorPalette ColorPalette { get { return colorPalette; } }

		private Word[] words;

		public FormatACO(byte[] bytes)
		{
			words = Utilities.BreakIntoWords(bytes).Select(word => word.ConvertBetweenBigEndianAndLittleEndian()).ToArray();
			int version = words[0].ToInt();
			int colorCount = words[1].ToInt();
			int index = 2;
			if(version == 1)
			{
				index = LoadVersion1(colorCount, index);
			}
			if(EndOfFile(index)) return;

			version = words[index].ToInt();
			index++;
			colorCount = words[index].ToInt();
			index++;
			if(version == 2)
			{
				index = LoadVersion2(colorCount, index);
			}

		}

		/// <summary>
		/// Load version 1 data.
		/// </summary>
		/// <param name="index">Starting words[index] after the color count.</param>
		/// <returns>Starting index of next section of the file.</returns>
		private int LoadVersion1(int colorCount, int index)
		{
			colorPalette = new ColorPalette();
			for(int i=0; i<colorCount; i++)
			{
				int colorSpace = (int)words[index].ToUInt();
				int w = (int)words[index+1].ToUInt();
				int x = (int)words[index+2].ToUInt();
				int y = (int)words[index+3].ToUInt();
				int z = (int)words[index+4].ToUInt();
				switch(colorSpace)
				{
					case 0: ConvertColorSpace0(w, x, y); break;
					case 1: ConvertColorSpace1(w, x, y); break;
					case 2: ConvertColorSpace2(w, x, y, z); break;
					case 7: throw new NotImplementedException(ErrorMessages.ACOFormatColorSpace7NotSupported);
					case 8: throw new NotImplementedException(ErrorMessages.ACOFormatColorSpace8NotSupported);
					case 9: throw new NotImplementedException(ErrorMessages.ACOFormatColorSpace9NotSupported);
					default: throw new Exception(ErrorMessages.ACOFormatColorSpaceUnknown, new Exception("Unknown color space: " + colorSpace));
				}
				index += 5;
			}
			return index;
		}

		/// <summary>
		/// Load version 2 data.
		/// </summary>
		/// <param name="index">Starting words[index] after the color count.</param>
		/// <returns>Starting index of next section of the file.</returns>
		private int LoadVersion2(int colorCount, int index)
		{
			colorPalette = new ColorPalette();
			for(int i = 0; i < colorCount; i++)
			{
				int colorSpace = (int)words[index].ToUInt();
				int w = (int)words[index + 1].ToUInt();
				int x = (int)words[index + 2].ToUInt();
				int y = (int)words[index + 3].ToUInt();
				int z = (int)words[index + 4].ToUInt();
				switch(colorSpace)
				{
					case 0: ConvertColorSpace0(w, x, y); break;
					case 1: ConvertColorSpace1(w, x, y); break;
					case 2: ConvertColorSpace2(w, x, y, z); break;
					case 7: throw new NotImplementedException(ErrorMessages.ACOFormatColorSpace7NotSupported);
					case 8: throw new NotImplementedException(ErrorMessages.ACOFormatColorSpace8NotSupported);
					case 9: throw new NotImplementedException(ErrorMessages.ACOFormatColorSpace9NotSupported);
					default: throw new Exception(ErrorMessages.ACOFormatColorSpaceUnknown, new Exception("Unknown color space: " + colorSpace));
				}
				index += 5;
				index++; //skip zero
				int nameLength = ((int)words[index].ToUInt()) - 1;
				index++; //skip nameLength
				index += nameLength; //skip name
				index++; //skip zero
			}
			return index;
		}

		private void ConvertColorSpace0(int w, int x, int y)
		{
			w = (int)Math.Floor((decimal)(w / 256));
			x = (int)Math.Floor((decimal)(x / 256));
			y = (int)Math.Floor((decimal)(y / 256));
			colorPalette.Add(Utilities.ColorFromRGB(w, x, y));
		}

		private void ConvertColorSpace1(int w, int x, int y)
		{
			w = (int)Math.Floor((decimal)(w / 182.04));
			x = (int)Math.Floor((decimal)(x / 655.35));
			y = (int)Math.Floor((decimal)(y / 655.35));
			colorPalette.Add(Utilities.ColorFromHSV(w, x / 100, y / 100));
		}

		private void ConvertColorSpace2(int w, int x, int y, int z)
		{
			w = (int)Math.Floor((decimal)((100 - w) / 655.35));
			x = (int)Math.Floor((decimal)((100 - x) / 655.35));
			y = (int)Math.Floor((decimal)((100 - y) / 655.35));
			z = (int)Math.Floor((decimal)((100 - z) / 655.35));
			colorPalette.Add(Utilities.ColorFromCMYK(w / 100, x / 100, y / 100, z / 100));
		}

		private bool EndOfFile(int index)
		{
			return (index >= words.Length);
		}
	}
}