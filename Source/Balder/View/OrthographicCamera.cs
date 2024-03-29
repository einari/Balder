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
using Balder.Display;
using Balder.Execution;
using Balder.Math;

namespace Balder.View
{
	public class OrthographicCamera : Camera
	{
		public OrthographicCamera()
		{
			Frustum = new OrthographicFrustum();
		}


		protected override void SetupProjection(Viewport viewport)
		{
			if( XSize == 0 )
			{
				XSize = viewport.Width;
			}
			if( YSize == 0 )
			{
				YSize = viewport.Height;
			}
			
			ProjectionMatrix = Matrix.CreateOrthographic((float)XSize, (float)YSize, Near, Far);
		}


		public static readonly Property<OrthographicCamera, double> XSizeProperty =
			Property<OrthographicCamera, double>.Register(c => c.XSize);
		public double XSize
		{
			get { return XSizeProperty.GetValue(this); }
			set
			{
				XSizeProperty.SetValue(this, value);
				((OrthographicFrustum)Frustum).XSize = (float)value;
			}
		}

		public static readonly Property<OrthographicCamera, double> YSizeProperty =
			Property<OrthographicCamera, double>.Register(c => c.YSize);
		public double YSize
		{
			get { return YSizeProperty.GetValue(this); }
			set
			{
				YSizeProperty.SetValue(this, value);
				((OrthographicFrustum) Frustum).YSize = (float)value;
			}
		}

		public override Ray GetPickRay(int x, int y)
		{
			var view = ViewMatrix;
			var world = Matrix.Identity;
			var projection = ProjectionMatrix;
			var nearPoint = Unproject(new Vector(x, y, Near), projection, view, world);
			var farPoint = Unproject(new Vector(x, y, Far), projection, view, world);
			var position = nearPoint;

			var direction = farPoint - nearPoint;
			direction.Normalize();

			var ray = new Ray(position, direction);
			return ray;
		}
	}
}