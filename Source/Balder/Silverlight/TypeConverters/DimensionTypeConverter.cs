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
#if(XAML)
using System;
using System.ComponentModel;
using System.Globalization;
using Balder.Math;

namespace Balder.Silverlight.TypeConverters
{
	public class DimensionTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType.Equals(typeof(string));
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType.Equals(typeof(Dimension));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			var stringValue = value as string;
			var trimmedStringValue = stringValue.Replace(" ", string.Empty);
			var values = trimmedStringValue.Split(',');
			if (values.Length != 2)
			{
				throw new ArgumentException("The format needs to be ([width],[height])");
			}
			var dimension = new Dimension
			                	{
			                		Width = float.Parse(values[0], CultureInfo.InvariantCulture),
			                		Height = float.Parse(values[1], CultureInfo.InvariantCulture),
			                	};
			return dimension;
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			var dimension = (Dimension)value;
			var dimensionAsString = string.Format(CultureInfo.InvariantCulture, "{0},{1}", dimension.Width, dimension.Height);
			return dimensionAsString;
		}
	}
}
#endif