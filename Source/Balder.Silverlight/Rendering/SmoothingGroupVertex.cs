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

using Balder.Core;
using Balder.Core.Math;

namespace Balder.Silverlight.Rendering
{
	public class SmoothingGroupVertex
	{
		public SmoothingGroupVertex()
		{
			CalculatedColor = new ColorAsFloats(0,0,0,1);
			IsColorCalculated = false;
		}

		public int Number { get; set; }
		public Vector Normal { get; set; }
		public Vector TransformedNormal { get; set; }
		public ColorAsFloats CalculatedColor { get; set; }
		public bool IsColorCalculated { get; set; }
	}
}