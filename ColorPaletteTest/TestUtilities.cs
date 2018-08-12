﻿using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WithoutHaste.Drawing.ColorPalette;

namespace ColorPaletteTest
{
	[TestClass]
	public class TestUtilities
	{
		[TestMethod]
		public void ColorFromHSV_ColorLibrary()
		{
			foreach(ColorLibrary.Name name in ColorLibrary.Library.Keys)
			{
				//arrange
				TestColor testColor = ColorLibrary.Library[name];
				//act
				Color result = Utilities.ColorFromHSV(testColor.HSV);
				//assert
				Assert.AreEqual(testColor.Color.R, result.R);
				Assert.AreEqual(testColor.Color.G, result.G);
				Assert.AreEqual(testColor.Color.B, result.B);
			}
		}

		[TestMethod]
		public void HSVFromColor_ColorLibrary()
		{
			foreach(ColorLibrary.Name name in ColorLibrary.Library.Keys)
			{
				//arrange
				TestColor testColor = ColorLibrary.Library[name];
				//act
				HSV result = Utilities.HSVFromColor(testColor.Color);
				//assert
				Assert.AreEqual(testColor.HSV.Hue, result.Hue);
				Assert.AreEqual(testColor.HSV.Saturation, result.Saturation);
				Assert.AreEqual(testColor.HSV.Value, result.Value);
			}
		}

	}
}
