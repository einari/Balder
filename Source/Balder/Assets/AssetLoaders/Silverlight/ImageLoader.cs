﻿#region License
//
// Author: Einar Ingebrigtsen <einar@dolittle.com>
// Copyright (c) 2007-2011, DoLittle Studios
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
#if(SILVERLIGHT)
using System;
using System.Windows.Media.Imaging;
using Balder.Content;
using Balder.Imaging;

namespace Balder.Assets.AssetLoaders.Silverlight
{
	public class ImageLoader : AssetLoader
	{
		public ImageLoader(IFileLoaderManager fileLoaderManager, IContentManager contentManager)
			: base(fileLoaderManager, contentManager)
		{
		}


		public override Type SupportedAssetType { get { return typeof(Image); } }

		public override IAssetPart[] Load(string assetName)
		{
			var fileLoader = FileLoaderManager.GetFileLoader(assetName);
			var stream = fileLoader.GetStream(assetName);

			var bitmapImage = new BitmapImage();
			bitmapImage.SetSource(stream);


			var writeableBitmap = new WriteableBitmap(bitmapImage);
			var width = writeableBitmap.PixelWidth;
			var height = writeableBitmap.PixelHeight;
			var frame = ContentManager.CreateAssetPart<Image>();
			frame.Width = width;
			frame.Height = height;

			var imageAsBytes = new byte[width * height * 4];

			Buffer.BlockCopy(writeableBitmap.Pixels, 0, imageAsBytes, 0, imageAsBytes.Length);

			frame.ImageContext.SetFrame(imageAsBytes, width, height);

			return new[] { frame };
		}


		public override string[] FileExtensions
		{
			get { return new[] { "png", "jpg" }; }
		}
	}
}
#endif