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
using Balder.Math;
using Balder.Rendering;
using Balder.Testing;
using Moq;
using NUnit.Framework;

namespace Balder.Tests.Extensions
{
	[TestFixture]
	public class ViewportExtensionsTests : TestFixture
	{
		[Test]
		public void UnprojectingCenterOfViewportWithIdentityViewShouldGenerateAVectorAtCenter()
		{
			var runtimeContextMock = new Mock<IRuntimeContext>();
			var viewport = new Viewport(runtimeContextMock.Object) { Width = 640, Height = 480 };

			var aspect = (float)viewport.Height / (float)viewport.Width;
			var projection = Matrix.CreatePerspectiveFieldOfView(40f, aspect, 1, 4000f);
			var view = Matrix.Identity;
			var world = Matrix.Identity;
			var position = new Vector((float)viewport.Width / 2, (float)viewport.Height / 2, 0);
			var result = viewport.Unproject(position, projection, view, world);
			Assert.That(result, Is.EqualTo(Vector.Forward));
		}

		[Test]
		public void UnprojectingCenterOfViewportWithViewRotated90DegreesAroundYAxisShouldGenerateAVectorRotated90DegreesInOpositeDirection()
		{
			var runtimeContextMock = new Mock<IRuntimeContext>();
			var viewport = new Viewport(runtimeContextMock.Object) { Width = 640, Height = 480 };
			var aspect = (float)viewport.Height / (float)viewport.Width;
			var projection = Matrix.CreatePerspectiveFieldOfView(40f, aspect, 1, 4000f);
			var view = Matrix.CreateRotationY(90);
			var world = Matrix.Identity;
			var position = new Vector((float)viewport.Width / 2, (float)viewport.Height / 2, 0);
			var result = viewport.Unproject(position, projection, view, world);

			var negativeView = Matrix.CreateRotationY(-90);
			var expected = Vector.Forward;
			var rotatedExpected = expected * negativeView;

			Assert.That(result, Is.EqualTo(rotatedExpected));
		}

	}
}
