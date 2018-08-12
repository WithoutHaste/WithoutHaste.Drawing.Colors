﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WithoutHaste.Drawing.ColorPalette
{
	internal struct Word
	{
		public byte First;
		public byte Second;

		public Word(byte a, byte b)
		{
			First = a;
			Second = b;
		}

		public Word ConvertBetweenBigEndianAndLittleEndian()
		{
			return new Word(Second, First);
		}

		public int ToInt()
		{
			return BitConverter.ToInt16(new byte[] { First, Second }, 0);
		}

		public uint ToUInt()
		{
			return BitConverter.ToUInt32(new byte[] { First, Second, 0, 0 }, 0);
		}
	}
}