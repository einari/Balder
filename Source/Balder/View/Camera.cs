﻿#region License
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
using Balder.Display;
using Balder.Execution;
using Balder.Math;
#if(SILVERLIGHT)
using System.Windows;
#endif


namespace Balder.View
{
#if(SILVERLIGHT)
	public class Camera : FrameworkElement, IView
#else
	public class Camera : IView
#endif
	{
		public const float DefaultFieldOfView = 45f;
		public const float DefaultFar = 4000f;
		public const float DefaultNear = 0.1f;

		private readonly Frustum _frustum;

		public Camera()
		{
			Position = Vector.Zero;
			Target = Vector.Forward;
			Up = Vector.Up;
			Near = DefaultNear;
			Far = DefaultFar;
			FieldOfView = DefaultFieldOfView;
			ProjectionMatrix = Matrix.Identity;
			UpdateDepthDivisor();
			ViewMatrix = Matrix.CreateLookAt(Position, Target, Up);

			_frustum = new Frustum();
		}

		#region Public Properties

		public virtual Matrix ViewMatrix { get; protected set; }
		public virtual Matrix ProjectionMatrix { get; protected set; }

		public static readonly Property<Camera, Coordinate> PositionProp = Property<Camera, Coordinate>.Register(c => c.Position);
		/// <summary>
		/// Get and set the position for the Camera
		/// </summary>
		public Coordinate Position
		{
			get { return PositionProp.GetValue(this); }
			set { PositionProp.SetValue(this, value); }
		}


		public static readonly Property<Camera, Coordinate> TargetProp = Property<Camera, Coordinate>.Register(c => c.Target);
		/// <summary>
		/// Get and set the target for the Camera - The location the camera is looking at
		/// </summary>
		public Coordinate Target
		{
			get { return TargetProp.GetValue(this); }
			set { TargetProp.SetValue(this, value); }
		}

		public Vector Up { get; set; }

		/// <summary>
		/// Get the forward vector for the camera. This is calculated from the target and position
		/// </summary>
		public Vector Forward
		{
			get { return Target - Position; }
		}

		/// <summary>
		/// Gets or sets the near distance clipping plane
		/// </summary>
		public float Near { get; set; }

		/// <summary>
		/// Gets or sets the far distance clipping plane
		/// </summary>
		public float Far { get; set; }

		/// <summary>
		/// Gets the divisor used for transforming Z values for purposes such as depth buffers
		/// </summary>
		public float DepthDivisor { get; private set; }

		/// <summary>
		/// Gets the value that indicates the actual zero/start of the depth, typically used by depth buffers
		/// </summary>
		public float DepthZero { get; private set; }


		/// <summary>
		/// Gets or sets the field of view for the camera
		/// </summary>
		public double FieldOfView { get; set; }

		#endregion

		#region Private Methods

		/// <summary>
		/// Calculates the projection matrix
		/// </summary>
		protected virtual void SetupProjection(Viewport viewport)
		{
			var projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.ToRadians((float)FieldOfView),
				viewport.AspectRatio,
				Near,
				Far);
			//var screenTranslationMatrix = Matrix.CreateScreenTranslation(viewport.Width, viewport.Height);
			ProjectionMatrix = projectionMatrix; // *screenTranslationMatrix;
		}

		private void UpdateDepthDivisor()
		{
			DepthDivisor = Far - Near;
			DepthZero = Near / DepthDivisor;
		}
		#endregion

		#region Public Methods

		public void Update(Viewport viewport)
				{	
			ViewMatrix = Matrix.CreateLookAt(Position, Target, Up);
			SetupProjection(viewport);
			UpdateDepthDivisor();
			_frustum.SetCameraDefinition(viewport, this);
		}


		public bool IsInView(Vector vector)
		{
			return _frustum.IsPointInFrustum(vector) == FrustumIntersection.Inside;
		}

		public bool IsInView(Coordinate coordinate)
		{
			return _frustum.IsPointInFrustum(coordinate) == FrustumIntersection.Inside;
		}

		public bool IsInView(BoundingSphere boundingSphere)
		{
			return _frustum.IsSphereInFrustum(boundingSphere.Center, boundingSphere.Radius) == FrustumIntersection.Inside;
		}

		#endregion
	}
}