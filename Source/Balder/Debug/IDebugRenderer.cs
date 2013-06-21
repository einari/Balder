﻿#region License
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
using Balder.Math;
using Balder.Rendering;

namespace Balder.Debug
{
	public interface IDebugRenderer
	{
        void RenderBoundingObject(IBoundingObject boundingObject, Viewport viewport, DetailLevel detailLevel, Matrix world);
        void RenderBoundingSphere(BoundingSphere sphere, Viewport viewport, DetailLevel detailLevel, Matrix world, bool topLevel);
        void RenderBoundingBox(BoundingBox sphere, Viewport viewport, DetailLevel detailLevel, Matrix world, bool topLevel);
		void RenderRectangle(Vector upperLeft, Vector upperRight, Vector lowerLeft, Vector lowerRight, Viewport viewport, Matrix world);
		void RenderRay(Vector position, Vector direction, Viewport viewport);
	}
}
