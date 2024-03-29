﻿#region License
//
// Author: Einar Ingebrigtsen <einar@dolittle.com>
// Copyright (c) 2007-2010, DoLittle Studios
//
// Licensed under the Microsoft Permissive License (Ms-PL), Version 1.1 (the "License")
// you may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://balder.codeplex.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
#if(XNA)
using System;
using System.Runtime.InteropServices;
using Balder.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#if(SILVERLIGHT)
using D = Balder.Display.Silverlight5.Display;
#else
using D = Balder.Display.Xna.Display;
#endif
using Microsoft.Xna.Framework.Silverlight;


namespace Balder.Rendering.Xna
{
    public class ImageContext : IImageContext
    {
		public Texture2D Texture { get; private set; }
    	private int[] _texels;

        public void SetFrame(byte[] frameBytes, int width, int height)
        {
			for (var pixelIndex = 0; pixelIndex < frameBytes.Length; pixelIndex += 4)
			{
				var red = frameBytes[pixelIndex];
				frameBytes[pixelIndex] = frameBytes[pixelIndex + 2];
				frameBytes[pixelIndex + 2] = red;
			}
			_texels = new int[width*height];

			Array.Copy(frameBytes, _texels, _texels.Length);

			Texture = new Texture2D(GraphicsDeviceManager.Current.GraphicsDevice, width, height, false, SurfaceFormat.Color);
			Texture.SetData(0, new Rectangle(0,0,width,height), frameBytes, 0, frameBytes.Length);
        }

        public void SetFrame(ImageFormat format, byte[] frameBytes)
        {
            throw new NotImplementedException();
        }

        public void SetFrame(ImageFormat format, byte[] frameBytes, ImagePalette palette)
        {
            throw new NotImplementedException();
        }

        public int[] GetPixelsAs32BppARGB()
        {
        	return _texels;
        }

        public ImageFormat[] SupportedImageFormats
        {
            get { throw new NotImplementedException(); }
        }
    }
}
#endif