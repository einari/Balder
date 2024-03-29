﻿using System;
using Balder.Imaging;

namespace Balder.Tests.Fakes
{
	public class FakeImageContext : IImageContext
	{
		private static readonly ImageFormat[] ImageFormats = new[]
		                                                     	{
		                                                     		new ImageFormat {PixelFormat = PixelFormat.RGBAlpha, Depth = 32}
		                                                     	};

		public void SetFrame(byte[] frameBytes, int width, int height)
		{

		}

		public void SetFrame(ImageFormat format, byte[] frameBytes)
		{

		}

		public void SetFrame(ImageFormat format, byte[] frameBytes, ImagePalette palette)
		{

		}

		public int[] GetPixelsAs32BppARGB()
		{
			throw new NotImplementedException();
		}

		public ImageFormat[] SupportedImageFormats { get { return ImageFormats; }}
	}
}