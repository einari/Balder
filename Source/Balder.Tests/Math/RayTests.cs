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
using Balder.Math;
using NUnit.Framework;

namespace Balder.Tests.Math
{
	[TestFixture]
	public class RayTests
	{
		[Test]
		public void RayPointingThroughBoundingSphereFromFrontShouldBeIntersected()
		{
			var boundingSphere = new BoundingSphere(new Vector(0, 0, 0), 1);
			var ray = new Ray(new Vector(0, 0, -10), new Vector(0, 0, 1));
			var result = ray.Intersects(boundingSphere);
			Assert.That(result,Is.Not.Null);
		}

		[Test]
		public void RayPointingThroughBoundingSphereFromLeftShouldBeIntersected()
		{
			var boundingSphere = new BoundingSphere(new Vector(0, 0, 0), 1);
			var ray = new Ray(new Vector(-10, 0, 0), new Vector(1, 0, 0));
			var result = ray.Intersects(boundingSphere);
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void RayPointingThroughBoundingSphereFromRightShouldBeIntersected()
		{
			var boundingSphere = new BoundingSphere(new Vector(0, 0, 0), 1);
			var ray = new Ray(new Vector(10, 0, 0), new Vector(-1, 0, 0));
			var result = ray.Intersects(boundingSphere);
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void RayPointingThroughBoundingSphereFromBackShouldBeIntersected()
		{
			var boundingSphere = new BoundingSphere(new Vector(0, 0, 0), 1);
			var ray = new Ray(new Vector(0, 0, 10), new Vector(0, 0, -1));
			var result = ray.Intersects(boundingSphere);
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void RayPointingThroughBoundingSphereFromTopShouldBeIntersected()
		{
			var boundingSphere = new BoundingSphere(new Vector(0, 0, 0), 1);
			var ray = new Ray(new Vector(0, 10, 0), new Vector(0, -1, 0));
			var result = ray.Intersects(boundingSphere);
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void RayPointingThroughBoundingSphereFromBottomShouldBeIntersected()
		{
			var boundingSphere = new BoundingSphere(new Vector(0, 0, 0), 1);
			var ray = new Ray(new Vector(0, -10, 0), new Vector(0, 1, 0));
			var result = ray.Intersects(boundingSphere);
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void RayPointingAwayFromBoundingSphereShouldBeIntersected()
		{
			var boundingSphere = new BoundingSphere(new Vector(0, 0, 0), 1);
			var ray = new Ray(new Vector(0, -10, 0), new Vector(0, -1, 0));
			var result = ray.Intersects(boundingSphere);
			Assert.That(result, Is.Null);
		}

		[Test]
		public void RayPointingIntoBoundSphereFromMultipleLocationsShouldBeIntersected()
		{
			var rays = new Ray[]
			           	{
							new Ray(new Vector(-12,12,-14), Vector.Zero), 
							new Ray(new Vector(12,12,-14), Vector.Zero), 
							new Ray(new Vector(-12,-12,-14), Vector.Zero), 
							new Ray(new Vector(12,-12,-14), Vector.Zero), 
			           	};

			var boundingSphere = new BoundingSphere(new Vector(0, 0, 0), 10);
			var destination = new Vector(0, 0, -1);

			foreach( var ray in rays )
			{
				var direction = destination - ray.Position;
				direction.Normalize();
				var newRay = new Ray(ray.Position, direction);

				var result = newRay.Intersects(boundingSphere);
				Assert.That(result, Is.Not.Null, string.Format("Ray : {0}",ray));
			}
		}

		[Test]
		public void RayPointingFromFrontThroughTriangleFacingFrontShouldBeIntersected()
		{
			var vector1 = new Vector(0, -10, 0);
			var vector3 = new Vector(-10, 10, 0);
			var vector2 = new Vector(10, 10, 0);
			
			var ray = new Ray(new Vector(0, 0, -10), new Vector(0, 0, 1));

			var distance = ray.IntersectsTriangle(vector1, vector2, vector3);
			Assert.That(distance, Is.Not.Null);
		}

		[Test]
		public void RayPointingFromFrontThroughTriangleFacingBackShouldNotBeIntersected()
		{
			var vector1 = new Vector(-10, 10, 0);
			var vector2 = new Vector(10, 10, 0);
			var vector3 = new Vector(0, -10, 0);

			var ray = new Ray(new Vector(0, 0, -10), new Vector(0, 0, 1));

			var distance = ray.IntersectsTriangle(vector1, vector2, vector3);
			Assert.That(distance, Is.Null);
		}

	}
}
