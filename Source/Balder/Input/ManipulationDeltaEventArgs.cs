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

using Balder.Execution;
using Balder.Materials;
using Balder.Objects.Geometries;

namespace Balder.Input
{
	public class ManipulationDeltaEventArgs : BubbledEventArgs
	{
		public ManipulationDeltaEventArgs(
			Material material,
			Face face,
			int faceIndex,
			int deltaX,
			int deltaY,
			ManipulationDirection direction)
		{
			Material = material;
			Face = face;
			FaceIndex = faceIndex;
			DeltaX = deltaX;
			DeltaY = deltaY;
			Direction = direction;
		}

		public Material Material { get; private set; }
		public int DeltaX { get; private set; }
		public int DeltaY { get; private set; }

		public Face Face { get; private set; }
		public int FaceIndex { get; private set; }

		public ManipulationDirection Direction { get; private set; }
	}
}