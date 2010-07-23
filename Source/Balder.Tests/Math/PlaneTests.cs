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
	public class PlaneTests
	{
		[Test]
		public void GettingDistanceFromPlaneShouldReturnCorrectDistance()
		{
			var plane = new Plane();
			var vector1 = new Vector(-100, -100, 100);
			var vector2 = new Vector(100, -100, 100);
			var vector3 = new Vector(0, -100, 0);
			plane.SetVectors(vector1,vector2,vector3);

			var vectorToTest = new Vector(0, -200, 0);

			var length = plane.GetDistanceFromVector(vectorToTest);
			Assert.That(length,Is.EqualTo(-100f));
		}
	}
}
