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
using System.ComponentModel;
using Balder.Silverlight.TypeConverters;
#endif

namespace Balder.Math
{
#if(SILVERLIGHT)
	[TypeConverter(typeof(DimensionTypeConverter))]
#endif
	public class Dimension
	{
		public float Width { get; set; }
		public float Height { get; set; }

		public void Set(float width, float height)
		{
			Width = width;
			Height = height;
		}

		public bool Equals(float width, float height)
		{
			return Width == width && Height == height;
		}

		public override bool Equals(object obj)
		{
			var dimension = obj as Dimension;
			if (dimension != null)
			{
				return dimension.Width == Width && dimension.Height == Height;
			}
			return false;
		}
	}
}