#region License

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

namespace Balder.Rendering.Silverlight.Drawing
{
	public class GouraudTriangle : Triangle
	{
		protected override void DrawSpan(int offset)
		{
			float z;

			var subPixelX = 1f - (X1 - X1Int);
			var zz = Z1 + subPixelX * ZInterpolateX;

			var rr = (int)((RScanline + subPixelX * RScanlineInterpolate) * 65535f);
			var gg = (int)((GScanline + subPixelX * GScanlineInterpolate) * 65535f);
			var bb = (int)((BScanline + subPixelX * BScanlineInterpolate) * 65535f);
			var aa = (int)((AScanline + subPixelX * AScanlineInterpolate) * 65535f);
			var rInterpolate = (int)(RScanlineInterpolate * 65535f);
			var gInterpolate = (int)(GScanlineInterpolate * 65535f);
			var bInterpolate = (int)(BScanlineInterpolate * 65535f);
			var aInterpolate = (int)(AScanlineInterpolate * 65535f);

			for (var x = X1Int; x < X2Int; x++)
			{
				z = 1f / zz;
				var bufferZ = (UInt32)(1.0f - (z*ZMultiplier));
				if (bufferZ > DepthBuffer[offset] &&
					z >= Near &&
					z < Far
					)
				{
					var red = (rr >> 8) & 0xff;
					var green = (gg >> 8) & 0xff;
					var blue = (bb >> 8) & 0xff;

					//var alpha = (uint)(aStart >> 8) & 0xff;

					var colorAsInt = Color.AlphaFull |
										(red << 16) |
										(green << 8) |
										blue;

					SetPixel(offset,colorAsInt);
					//Framebuffer[offset] = colorAsInt;
					DepthBuffer[offset] = bufferZ;
				}

				offset++;
				zz += ZInterpolateX;
				
				rr += rInterpolate;
				gg += gInterpolate;
				bb += bInterpolate;
				aa += aInterpolate;
			}
		}
	}
}
#endif