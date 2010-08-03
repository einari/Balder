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
#if(SILVERLIGHT)
namespace Balder.Rendering.Silverlight.Drawing
{
	public abstract class Triangle
	{
		protected void GetSortedPoints(ref RenderVertex vertexA,
										ref RenderVertex vertexB,
										ref RenderVertex vertexC)
		{
			var point1 = vertexA;
			var point2 = vertexB;
			var point3 = vertexC;

			if (point2.TranslatedScreenCoordinates.Y < point1.TranslatedScreenCoordinates.Y)
			{
				var p = point1;
				point1 = point2;
				point2 = p;
			}

			if (point3.TranslatedScreenCoordinates.Y < point2.TranslatedScreenCoordinates.Y)
			{
				var p = point2;
				point2 = point3;
				point3 = p;
			}


			if (point2.TranslatedScreenCoordinates.Y < point1.TranslatedScreenCoordinates.Y)
			{
				var p = point1;
				point1 = point2;
				point2 = p;
			}

			vertexA = point1;
			vertexB = point2;
			vertexC = point3;
		}

		protected void GetSortedPoints(RenderFace face,
										ref RenderVertex vertexA,
										ref RenderVertex vertexB,
										ref RenderVertex vertexC)
		{
			var point1 = vertexA;
			var point2 = vertexB;
			var point3 = vertexC;

			if (point2.TranslatedScreenCoordinates.Y < point1.TranslatedScreenCoordinates.Y)
			{
				var p = point1;
				point1 = point2;
				point2 = p;

				var ca = face.CalculatedColorA;
				var cb = face.CalculatedColorB;
				face.CalculatedColorB = ca;
				face.CalculatedColorA = cb;
			}

			if (point3.TranslatedScreenCoordinates.Y < point2.TranslatedScreenCoordinates.Y)
			{
				var p = point2;
				point2 = point3;
				point3 = p;

				var cb = face.CalculatedColorB;
				var cc = face.CalculatedColorC;
				face.CalculatedColorC = cb;
				face.CalculatedColorB = cc;
			}


			if (point2.TranslatedScreenCoordinates.Y < point1.TranslatedScreenCoordinates.Y)
			{
				var p = point1;
				point1 = point2;
				point2 = p;

				var ca = face.CalculatedColorA;
				var cb = face.CalculatedColorB;
				face.CalculatedColorB = ca;
				face.CalculatedColorA = cb;
			}

			vertexA = point1;
			vertexB = point2;
			vertexC = point3;
		}

		public abstract void Draw(RenderFace face, RenderVertex[] vertices);
	}
}
#endif