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

using System;
using Balder.Display;
using Balder.Execution;
using Balder.Math;
using Dbg = System.Diagnostics.Debug;
#if(DEFAULT_CONSTRUCTOR)
using Ninject;
#endif

namespace Balder.Objects.Geometries
{
	public class Cylinder : GeneratedGeometry
	{
		private const int GeneralSmoothingGroup = 0;
		private const int EndsSmoothingGroup = 1;
		private const int SpokesSmoothingGroup = 2;


		public static readonly Property<Cylinder, double> TopRadiusProp = Property<Cylinder, double>.Register(c => c.TopRadius);
		public double TopRadius
		{
			get { return TopRadiusProp.GetValue(this); }
			set
			{
				TopRadiusProp.SetValue(this, value);
				InvalidatePrepare();
			}
		}

		public static readonly Property<Cylinder, double> BottomRadiusProp = Property<Cylinder, double>.Register(c => c.BottomRadius);
		public double BottomRadius
		{
			get { return BottomRadiusProp.GetValue(this); }
			set
			{
				BottomRadiusProp.SetValue(this, value);
				InvalidatePrepare();
			}
		}

		public static readonly Property<Cylinder, bool> CapEndsProp = Property<Cylinder, bool>.Register(c => c.CapEnds);
		public bool CapEnds
		{
			get { return CapEndsProp.GetValue(this); }
			set
			{
				CapEndsProp.SetValue(this, value);
				InvalidatePrepare();
			}
		}

		public static readonly Property<Cylinder, bool> SpokesProp = Property<Cylinder, bool>.Register(c => c.Spokes);
		public bool Spokes
		{
			get { return SpokesProp.GetValue(this); }
			set
			{
				SpokesProp.SetValue(this, value);
				InvalidatePrepare();
			}
		}


		public static readonly Property<Cylinder, int> SegmentsProp = Property<Cylinder, int>.Register(c => c.Segments);
		public int Segments
		{
			get { return SegmentsProp.GetValue(this); }
			set
			{
				SegmentsProp.SetValue(this, value);
				InvalidatePrepare();
			}
		}

		public static readonly Property<Cylinder, int> StacksProp = Property<Cylinder, int>.Register(c => c.Stacks);
		public int Stacks
		{
			get { return StacksProp.GetValue(this); }
			set
			{
				StacksProp.SetValue(this, value);
				InvalidatePrepare();
			}
		}

		public static readonly Property<Cylinder, double> SizeProperty = Property<Cylinder, double>.Register(c => c.Size);
		public double Size
		{
			get { return SizeProperty.GetValue(this); }
			set
			{
				SizeProperty.SetValue(this, value);
				InvalidatePrepare();
			}
		}

		public static readonly Property<Cylinder, double> StartAngleProperty = Property<Cylinder, double>.Register(c => c.StartAngle);
		public double StartAngle
		{
			get { return StartAngleProperty.GetValue(this); }
			set
			{
				StartAngleProperty.SetValue(this, value);
				InvalidatePrepare();
			}
		}

		public static readonly Property<Cylinder, double> EndAngleProperty = Property<Cylinder, double>.Register(c => c.EndAngle);
		public double EndAngle
		{
			get { return EndAngleProperty.GetValue(this); }
			set
			{
				EndAngleProperty.SetValue(this, value);
				InvalidatePrepare();
			}
		}


#if(DEFAULT_CONSTRUCTOR)
		public Cylinder()
			: this(Runtime.Instance.Kernel.Get<IGeometryContext>())
		{
		}
#endif

		public Cylinder(IGeometryContext geometryContext)
			: base(geometryContext)
		{
			StartAngle = 0;
			EndAngle = 360;
			Segments = 8;
			Stacks = 1;
			CapEnds = true;
			Spokes = true;
		}


		private void Validate()
		{
			if( TopRadius <= 0 || BottomRadius <= 0 )
			{
				throw new ArgumentException("Top and Bottom radius must be set to a number higher than 0");
			}

			if( Segments <= 2 )
			{
				throw new ArgumentException("You must have at least 2 segments");
			}

			if( StartAngle > EndAngle )
			{
				throw new ArgumentException("StartAngle must be less than EndAngle");
			}

			if( StartAngle < 0 || EndAngle < 0 )
			{
				throw new ArgumentException("Start or End angle must be 0 or more");
			}

			if( StartAngle > 360 || EndAngle > 360 )
			{
				throw new ArgumentException("Start or End angle must be 360 or less");
			}
			
			if( Stacks < 1 )
			{
				throw new ArgumentException("You must have at least 1 stack");
			}
		}

		public override void Prepare(Viewport viewport)
		{
			Validate();

			var actualStacks = Stacks + 1;
			var actualSegments = Segments;
			var nextSegmentOffset = actualSegments + 1;

			var deltaRadius = BottomRadius - TopRadius;
			var radiusAdd = deltaRadius / actualStacks;
			var currentRadius = TopRadius;

			var deltaY = Size;
			var yAdd = (float)(deltaY / (actualStacks - 1));
			var currentY = (float)deltaY / 2;

			var uDelta = 1f;
			var vDelta = 2f;

			var uAdd = uDelta / actualSegments;
			var vAdd = vDelta / actualStacks;

			var currentV = 0f;

			var radianAdd = 0d;
			var startRadian = 0d;

			var faceSegments = 0;
			var faceOffset = 0;
			var additionalFaceSegments = 0;
			var isFull = true;
			if (StartAngle > 0 || EndAngle < 360)
			{
				faceSegments = actualSegments+1;

				startRadian = MathHelper.ToRadians((float)StartAngle);
				var endRadian = MathHelper.ToRadians((float)EndAngle);
				var radianDelta = endRadian - startRadian;
				radianAdd = radianDelta / (actualSegments-1);

				isFull = false;

				faceOffset = 0;
				additionalFaceSegments = 1;
			}
			else
			{
				faceSegments = actualSegments;
				radianAdd = (System.Math.PI * 2) / actualSegments;
				isFull = true;
				faceOffset = 1;
				additionalFaceSegments = 0;
			}

			BuildVertices(actualSegments, actualStacks, startRadian, radianAdd, currentY, currentRadius, currentV, uAdd, radiusAdd, yAdd, vAdd);
			var faceIndex = BuildFaces(actualSegments, actualStacks, nextSegmentOffset, faceSegments, faceOffset, additionalFaceSegments, isFull);
			if (CapEnds)
			{
				faceIndex = BuildEnds(actualSegments, actualStacks, nextSegmentOffset, faceSegments, faceOffset, additionalFaceSegments, faceIndex);
			}

			GeometryHelper.CalculateNormals(FullDetailLevel);
			//InitializeBoundingSphere();

			base.Prepare(viewport);
		}

		private void BuildVertices(int actualSegments, int actualStacks, double startRadian, double radianAdd, float currentY, double currentRadius, float currentV, float uAdd, double radiusAdd, float yAdd, float vAdd)
		{
			Vertex vertex;
			var vertexCount = (actualStacks * (actualSegments + 1));
			var vertexIndex = 0;

			FullDetailLevel.AllocateVertices(vertexCount);
			FullDetailLevel.AllocateTextureCoordinates(vertexCount);

			for (var y = 0; y < actualStacks; y++)
			{
				var currentU = 0f;
				var currentRadian = startRadian;

				var centerTextureCoordinate = new TextureCoordinate(0.5f, 0.5f);
				vertex = new Vertex(0, currentY, 0);
				FullDetailLevel.SetVertex(vertexIndex, vertex);
				FullDetailLevel.SetTextureCoordinate(vertexIndex, centerTextureCoordinate);
				vertexIndex++;


				for (var x = 0; x < actualSegments; x++)
				{
					var currentX = (float)(System.Math.Sin(currentRadian) * currentRadius);
					var currentZ = (float)(System.Math.Cos(currentRadian) * currentRadius);

					vertex = new Vertex(currentX, currentY, currentZ);
					FullDetailLevel.SetVertex(vertexIndex, vertex);

					var textureCoordinate = new TextureCoordinate(1f - currentU, currentV);
					FullDetailLevel.SetTextureCoordinate(vertexIndex, textureCoordinate);

					vertexIndex++;
					currentRadian += radianAdd;
					currentU += uAdd;
				}


				currentRadius += radiusAdd;
				currentY -= yAdd;
				currentV += vAdd;
			}
		}

		private int BuildFaces(int actualSegments, int actualStacks, int nextSegmentOffset, int faceSegments, int faceOffset, int additionalFaceSegments, bool isFull)
		{
			var spokes = isFull || Spokes;
			var faceIndex = 0;
			var faceCount = (actualStacks-1) * (faceSegments * 2);
			if (CapEnds)
			{
				faceCount += faceSegments * 2;
			}
			if( !spokes )
			{
				faceSegments--;
				faceCount -= 4;
			}

			FullDetailLevel.AllocateFaces(faceCount);

			Face face;
			for (var y = 0; y < actualStacks - 1; y++)
			{
				var vertexOffset = (y * actualSegments);
				var nextSegmentVertexOffset = (y + 1) * nextSegmentOffset;

				for (var x = 0; x < faceSegments; x++)
				{
					if( !spokes && x == 0 )
					{
						continue;
					}
					var actualX = x + faceOffset;
					var nextX = ((x + 1) % (actualSegments + additionalFaceSegments))+faceOffset;

					face = CreateFace(vertexOffset + actualX,
					                vertexOffset + nextX,
					                nextSegmentVertexOffset + actualX);
					face.DiffuseA = face.A;
					face.DiffuseB = face.B;
					face.DiffuseC = face.C;

					if( spokes && x == 0 )
					{
						face.SmoothingGroup = SpokesSmoothingGroup;
					} else
					{
						face.SmoothingGroup = GeneralSmoothingGroup;
					}

					FullDetailLevel.SetFace(faceIndex, face);
					faceIndex++;

					face = CreateFace(nextSegmentVertexOffset + nextX,
					                nextSegmentVertexOffset + actualX,
					                vertexOffset + nextX);
					face.DiffuseA = face.A;
					face.DiffuseB = face.B;
					face.DiffuseC = face.C;

					if (spokes && x == 0)
					{
						face.SmoothingGroup = SpokesSmoothingGroup;
					}
					else
					{
						face.SmoothingGroup = GeneralSmoothingGroup;
					}

					FullDetailLevel.SetFace(faceIndex, face);

					faceIndex++;
				}
			}
			return faceIndex;
		}

		private int BuildEnds(int actualSegments, int actualStacks, int nextSegmentOffset, int faceSegments, int faceOffset, int additionalFaceSegments, int faceIndex)
		{
			var vertexOffset = 0;
			Face face;
			for (var x = 0; x < faceSegments; x++)
			{
				var actualX = x + faceOffset;
				var nextX = ((x + 1) % (actualSegments + additionalFaceSegments)) + faceOffset;
				face = CreateFace(vertexOffset,
				                vertexOffset + nextX,
				                vertexOffset + actualX);
				face.DiffuseA = face.A;
				face.DiffuseB = face.B;
				face.DiffuseC = face.C;
				face.SmoothingGroup = EndsSmoothingGroup;
				FullDetailLevel.SetFace(faceIndex, face);
				faceIndex++;
			}

			vertexOffset = (actualStacks - 1) * nextSegmentOffset;
			for (var x = 0; x < faceSegments; x++)
			{
				var actualX = x + faceOffset;
				var nextX = ((x + 1) % (actualSegments + additionalFaceSegments)) + faceOffset;
				face = CreateFace(vertexOffset + actualX,
				                vertexOffset + nextX,
				                vertexOffset);
				face.DiffuseA = face.A;
				face.DiffuseB = face.B;
				face.DiffuseC = face.C;
				face.SmoothingGroup = EndsSmoothingGroup;
				FullDetailLevel.SetFace(faceIndex, face);
				faceIndex++;
			}

			return faceIndex;
		}
	}
}
