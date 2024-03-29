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
#if(XNA)
using Balder.Objects.Geometries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Balder.Rendering.Xna
{
	public struct LineRenderVertex 
#if(!SILVERLIGHT)
		: IVertexType
#endif
	{
		private Vector3 _position;
		private Microsoft.Xna.Framework.Color _color;

		public LineRenderVertex(Vertex vertex, Color color)
		{
			_position = new Vector3(vertex.X, vertex.Y, vertex.Z);
			_color = color;
		}

		public static VertexElement[] VertexElements = {
		                                               	new VertexElement(0, VertexElementFormat.Vector3,VertexElementUsage.Position, 0), 
		                                               	new VertexElement(sizeof(float)*6,VertexElementFormat.Color,VertexElementUsage.Color,0), 
		                                               };

		public static VertexDeclaration Declaration = new VertexDeclaration(VertexElements);

		public VertexDeclaration VertexDeclaration
		{
			get { return Declaration; }
		}
	}
}
#endif