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
using System;
using Balder.Core.Display;
using Balder.Core.Math;
using Balder.Core.Objects.Geometries;
using Balder.Core.Rendering;
using Balder.Core.View;
using CThru.Silverlight;
using NUnit.Framework;

namespace Balder.Core.Tests
{
	[TestFixture]
	public class SceneTests
	{
		public class MyRenderableNode : RenderableNode
		{

			public bool PrepareCalled = false;
			public override void Prepare(Viewport viewport)
			{
				PrepareCalled = true;
			}

			public bool RenderCalled = false;
			public override void Render(Viewport viewport, DetailLevel detailLevel)
			{
				RenderCalled = true;
			}
		}

		public class RenderableNodeMock : RenderableNode
		{
			private readonly Action _actionToCall;
			public RenderableNodeMock()
			{
				
			}

			public RenderableNodeMock(Action actionToCall)
			{
				_actionToCall = actionToCall;
			}

			public Matrix WorldResult;
			public override void Render(Viewport viewport, DetailLevel detailLevel)
			{
				
				if (null != _actionToCall)
				{
					_actionToCall();
				}
			}
		}

	
		[Test, SilverlightUnitTest]
		public void GettingObjectAtCenterOfScreenWithSingleObjectAtCenterOfSceneShouldReturnTheObject()
		{
			var viewport = new Viewport {Width = 640, Height = 480};
			var scene = new Scene();
			var camera = new Camera() {Position = {Z = -20}};
			viewport.View = camera;
			viewport.Scene = scene;
			camera.Update(viewport);

			var node = new Geometry
			           	{
			           		BoundingSphere = new BoundingSphere(Vector.Zero, 10000f)
			           	};
			scene.AddNode(node);

			var nodeAtCoordinate = viewport.GetNodeAtScreenCoordinate(viewport.Width / 2, viewport.Height / 2);
			Assert.That(nodeAtCoordinate, Is.Not.Null);
			Assert.That(nodeAtCoordinate, Is.EqualTo(node));
		}

		[Test, SilverlightUnitTest]
		public void GettingObjectAtTopLeftOfScreenWithSingleObjectAtCenterOfSceneShouldReturnTheObject()
		{
			var viewport = new Viewport { Width = 640, Height = 480 };
			var scene = new Scene();
			var camera = new Camera();
			viewport.View = camera;
			camera.Position.Z = -100;

			camera.Update(viewport);

			var node = new Geometry
			           	{
			           		BoundingSphere = new BoundingSphere(Vector.Zero, 10f)
			           	};
			scene.AddNode(node);

			var nodeAtCoordinate = viewport.GetNodeAtScreenCoordinate(0, 0);
			Assert.That(nodeAtCoordinate, Is.Null);
		}


		[Test, SilverlightUnitTest]
		public void AddedNodeShouldGetRendered()
		{
			var viewport = new Viewport { Width = 640, Height = 480 };
			var camera = new Camera();
			viewport.View = camera;
			camera.Position.Z = -100;
			camera.Update(viewport);
			var scene = new Scene();
			viewport.Scene = scene;

			var renderableNode = new MyRenderableNode();

			scene.AddNode(renderableNode);

			viewport.Render(null);

			Assert.That(renderableNode.RenderCalled,Is.True);
		}

		[Test, SilverlightUnitTest]
		public void AddingNodeWithChildShouldCallRenderOnChild()
		{
			var viewport = new Viewport { Width = 640, Height = 480 };
			var camera = new Camera();
			viewport.View = camera;
			camera.Position.Z = -100;
			camera.Update(viewport);
			var scene = new Scene();
			viewport.Scene = scene;

			var topLevelNode = new MyRenderableNode();
			var childNode = new MyRenderableNode();
			topLevelNode.Children.Add(childNode);

			scene.AddNode(topLevelNode);
			viewport.Render(null);
			Assert.That(childNode.RenderCalled,Is.True);
		}

		[Test, SilverlightUnitTest]
		public void ChildNodeShouldHaveParentNodesWorldAppliedToWorldMatrix()
		{
			var viewport = new Viewport { Width = 640, Height = 480 };
			var camera = new Camera();
			viewport.View = camera;
			camera.Position.Z = -100;
			camera.Update(viewport);
			var scene = new Scene();
			viewport.Scene = scene;

			var topLevelNode = new MyRenderableNode();
			topLevelNode.Position.X = 50;
			var childNode = new RenderableNodeMock();
			topLevelNode.Children.Add(childNode);

			scene.AddNode(topLevelNode);
			viewport.Render(null);

			var actualPosition = new Coordinate();
			actualPosition.X = childNode.WorldResult[3, 0];
			actualPosition.Y = childNode.WorldResult[3, 1];
			actualPosition.Z = childNode.WorldResult[3, 2];
			Assert.That(actualPosition.X, Is.EqualTo(topLevelNode.Position.X));
		}


		[Test, SilverlightUnitTest]
		public void AddingChildProgramaticallyShouldCallPrepareOnNodeBeforeRendering()
		{
			var viewport = new Viewport { Width = 640, Height = 480 };
			var camera = new Camera();
			viewport.View = camera;
			camera.Position.Z = -100;
			camera.Update(viewport);
			var scene = new Scene();
			viewport.Scene = scene;

			var node = new MyRenderableNode();
			scene.AddNode(node);

			viewport.Render(null);

			Assert.That(node.PrepareCalled,Is.True);
		}

		[Test, SilverlightUnitTest]
		public void AddingChildProgramaticallyShouldCallPrepareOnNodeBeforeRenderingOnlyFirstTime()
		{
			var viewport = new Viewport { Width = 640, Height = 480 };
			var camera = new Camera();
			viewport.View = camera;
			camera.Position.Z = -100;
			camera.Update(viewport);
			var scene = new Scene();
			viewport.Scene = scene;

			var node = new MyRenderableNode();
			scene.AddNode(node);

			viewport.Render(null);

			node.PrepareCalled = false;

			viewport.Render(null);

			Assert.That(node.PrepareCalled, Is.False);
		}

	}
}
