#region License
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
using System;


#if(SILVERLIGHT)
using System.ComponentModel;
using System.Runtime.InteropServices;
using Balder.Silverlight.TypeConverters;
using SysColor = System.Windows.Media.Color;
#else
#if(!IOS)
using System.Runtime.InteropServices;
using SysColor = System.Drawing.Color;
#endif
#endif
using Balder.Math;

namespace Balder
{
	/// <summary>
	/// Represents a color
	/// </summary>
#if(SILVERLIGHT)
	[TypeConverter(typeof(ColorConverter))]
#endif
#if(WINDOWS_PHONE)
	[StructLayout(LayoutKind.Sequential, Size = 4)]
#else
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
#endif
    public struct Color : IEquatable<Color>
	{
		private static readonly Random Rnd = new Random();

		/// <summary>
		/// Creates an instance of <see cref="Color"/> with all channels initialized
		/// </summary>
		/// <param name="red">Value for Red channel</param>
		/// <param name="green">Value for Green channel</param>
		/// <param name="blue">Value for Blue channel</param>
		/// <param name="alpha">Value for Alpha channel</param>
		public Color(byte red, byte green, byte blue, byte alpha)
			: this()
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		/// <summary>
		/// Gets or sets the Alpha channel value
		/// </summary>
		public byte Alpha { get; set; }

		/// <summary>
		/// Gets or sets the Red channel value
		/// </summary>
		public byte Red { get; set; }

		/// <summary>
		/// Gets or sets the Green channel value
		/// </summary>
		public byte Green { get; set; }

		/// <summary>
		/// Gets or sets the Blue channel value
		/// </summary>
		public byte Blue { get; set; }



		#region Public Static Methods
		/// <summary>
		/// Create a random color
		/// </summary>
		/// <returns>Newly created color</returns>
		public static Color Random()
		{
			var red = (byte)Rnd.Next(0, 64);
			var green = (byte)Rnd.Next(0, 64);
			var blue = (byte)Rnd.Next(0, 64);
			var color = new Color(red, green, blue, 0xff);
			return color;
		}

		/// <summary>
		/// Create a color from given channel values
		/// </summary>
		/// <param name="alpha">Alpha channel value</param>
		/// <param name="red">Red channel value</param>
		/// <param name="green">Green channel value</param>
		/// <param name="blue">Blue channel value</param>
		/// <returns>Newly created color</returns>
		public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
		{
			var color = new Color
							{
								Red = red,
								Green = green,
								Blue = blue,
								Alpha = alpha
							};
			return color;
		}

#if(!IOS)
		/// <summary>
		/// Create a color from an existing <see cref="SysColor"/>
		/// </summary>
		/// <param name="systemColor"></param>
		/// <returns></returns>
		public static Color FromSystemColor(SysColor systemColor)
		{
			var color = new Color
			{
				Red = systemColor.R,
				Green = systemColor.G,
				Blue = systemColor.B,
				Alpha = systemColor.A
			};
			return color;
		}
#endif
		#endregion

		#region Public Methods

#if(!IOS)
		public SysColor ToSystemColor()
		{
			var sysColor = SysColor.FromArgb(Alpha, Red, Green, Blue);
			return sysColor;
		}
#endif

		public UInt32 ToUInt32()
		{
			var uint32Color = (((UInt32)Alpha) << 24) |
								(((UInt32)Red) << 16) |
								(((UInt32)Green) << 8) |
								(UInt32)Blue;
			return uint32Color;
		}

		public int ToInt()
		{
			return (int)ToUInt32();
		}

		public Color Additive(Color secondColor)
		{
			return Cluts.Add(this, secondColor);
		}

		public Color Subtract(Color secondColor)
		{
			var red = (int)Red - (int)secondColor.Red;
			var green = (int)Green - (int)secondColor.Green;
			var blue = (int)Blue - (int)secondColor.Blue;
			var alpha = (int)Alpha - (int)secondColor.Alpha;

			var result = new Color
			{
				Red = (byte)(red < 0 ? 0 : red),
				Green = (byte)(green < 0 ? 0 : green),
				Blue = (byte)(blue < 0 ? 0 : blue),
				Alpha = (byte)(alpha < 0 ? 0 : alpha),
			};
			return result;
		}

		public Color Average(Color secondColor)
		{
			var red = (int)Red + (int)secondColor.Red;
			var green = (int)Green + (int)secondColor.Green;
			var blue = (int)Blue + (int)secondColor.Blue;
			var alpha = (int)Alpha + (int)secondColor.Alpha;

			var result = new Color
			{
				Red = (byte)(red >> 1),
				Green = (byte)(green >> 1),
				Blue = (byte)(blue >> 1),
				Alpha = (byte)(alpha >> 1),
			};
			return result;
		}

		public static Color Scale(Color color, float scale)
		{
			var redAsFloat = ((float)color.Red) / 255f;
			var greenAsFloat = ((float)color.Green) / 255f;
			var blueAsFloat = ((float)color.Blue) / 255f;
			var alphaAsFloat = ((float)color.Alpha) / 255f;

			redAsFloat = redAsFloat * scale;
			greenAsFloat = greenAsFloat * scale;
			blueAsFloat = blueAsFloat * scale;
			alphaAsFloat = alphaAsFloat * scale;

			var newColor = new Color(
				(byte)(MathHelper.Saturate(redAsFloat)*255f),
				(byte)(MathHelper.Saturate(greenAsFloat)*255f),
				(byte)(MathHelper.Saturate(blueAsFloat)*255f),
				(byte)(MathHelper.Saturate(alphaAsFloat)*255f));
			return newColor;
		}


		public bool Equals(Color other)
		{
			return other.Red == Red &&
				   other.Green == Green &&
				   other.Blue == Blue &&
				   other.Alpha == Alpha;
		}

		public override string ToString()
		{
			var colorAsString = string.Format("R: {0}, G: {1}, B: {2}, A: {3}", Red, Green, Blue, Alpha);
			return colorAsString;
		}
		#endregion

		#region Operators
		/// <summary>
		/// Add colors - look at <seealso cref="Additive"/> for more details on the operation
		/// </summary>
		/// <param name="firstColor">First color in addition</param>
		/// <param name="secondColor">Second color in addition</param>
		/// <returns>Combined color</returns>
		public static Color operator +(Color firstColor, Color secondColor)
		{
			var result = firstColor.Additive(secondColor);
			return result;
		}

		/// <summary>
		/// Subtract colors - look at <seealso cref="Subtract"/> for more details on the operation
		/// </summary>
		/// <param name="firstColor">First color in subtraction</param>
		/// <param name="secondColor">Second color in subtraction</param>
		/// <returns>Combined color</returns>
		public static Color operator -(Color firstColor, Color secondColor)
		{
			var newColor = firstColor.Subtract(secondColor);
			return newColor;
		}

		public static Color operator *(Color firstColor, Color secondColor)
		{
			var newColor = Cluts.Multiply(firstColor, secondColor);
			return newColor;
		}

		public static Color operator *(float value, Color color)
		{
			return Scale(color, value);
		}


		public static Color operator *(Color color, float value)
		{
			return Scale(color, value);
		}

#if(!IOS)
		/// <summary>
		/// Implicitly convert to <see cref="Color"/> from <see cref="SysColor"/>
		/// </summary>
		/// <param name="color"></param>
		/// <returns>Converted color</returns>
		public static implicit operator Color(SysColor color)
		{
			var newColor = FromSystemColor(color);
			return newColor;
		}
#endif

#if(XNA)
        public static implicit operator Microsoft.Xna.Framework.Color(Color color)
        {
            var newColor = new Microsoft.Xna.Framework.Color(color.Blue, color.Green, color.Red, color.Alpha);
            return newColor;
        }
#endif

		#endregion

	}
}
