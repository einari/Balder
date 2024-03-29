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
using System;
using Balder.Materials;
using Balder.Math;
using Balder.Objects.Geometries;
using Balder.Rendering.Silverlight5;
using Microsoft.Xna.Framework.Graphics;
using Viewport = Balder.Display.Viewport;

#if(WINDOWS_PHONE)
using D = Balder.Display.WP7.Display;
#else
#if(SILVERLIGHT)
using D = Balder.Display.Silverlight5.Display;
#else
using D = Balder.Display.Xna.Display;
#endif
#endif
using Matrix = Microsoft.Xna.Framework.Matrix;


namespace Balder.Rendering.Xna
{
	public class GeometryDetailLevel : IGeometryDetailLevel
	{
		private static readonly object VertexLock = new object();

		private readonly IRenderingManager _renderingManager;
		public int FaceCount { get; private set; }
		public int VertexCount { get; private set; }
		public int TextureCoordinateCount { get; private set; }
		public int LineCount { get; private set; }
		public int NormalCount { get; private set; }

		private VertexBuffer _vertexBuffer;
		private IndexBuffer _indexBuffer;

		private RenderVertex[] _vertices;
		private LineRenderVertex[] _lineVertices;
		private ushort[] _indices;

		private bool _verticesPrepared;

		private Vertex[] _originalVertices;
		private Face[] _originalFaces;
		private Line[] _originalLines;
		private Normal[] _originalNormals;
		private TextureCoordinate[] _originalTextureCoordinates;
		private FaceTextureCoordinate[] _textureCoordinateForFaces;

		private Material _colorMaterial;

		private GraphicsDevice _graphicsDevice;


		public GeometryDetailLevel(IRenderingManager renderingManager)
		{
			_renderingManager = renderingManager;
			_graphicsDevice = D.GraphicsDevice;
			_colorMaterial = Material.FromColor(Colors.Blue);
		}

		public void AllocateFaces(int count)
		{
			lock (VertexLock)
			{
				var actualCount = count * 3;
				_indexBuffer = new IndexBuffer(D.GraphicsDevice, IndexElementSize.SixteenBits, actualCount, BufferUsage.WriteOnly);

				_indices = new ushort[actualCount];
				_originalFaces = new Face[count];

				if (null != _vertexBuffer)
				{
#if(!SILVERLIGHT)
				_vertexBuffer.Dispose();
#endif
				}
				_vertices = null;

				_textureCoordinateForFaces = new FaceTextureCoordinate[count];
			}
		}

		public void SetFace(int index, Face face)
		{
			var actualIndex = index * 3;
			_indices[actualIndex + 2] = (ushort)face.A;
			_indices[actualIndex + 1] = (ushort)face.B;
			_indices[actualIndex] = (ushort)face.C;

			_originalFaces[index] = face;
		}

		public Face[] GetFaces()
		{
			return _originalFaces;
		}

		public void InvalidateFace(int index)
		{
			SetFace(index, _originalFaces[index]);
		}

		public void AllocateVertices(int count)
		{
			lock (VertexLock)
			{
				_originalVertices = new Vertex[count];
				_verticesPrepared = false;
			}
		}

		public void SetVertex(int index, Vertex vertex)
		{
			_originalVertices[index] = vertex;
		}

		public Vertex[] GetVertices()
		{
			return _originalVertices;
		}

		public void InvalidateVertex(int index)
		{
			SetVertex(index, _originalVertices[index]);
		}

		public void AllocateNormals(int count)
		{
			lock (VertexLock)
			{
				_originalNormals = new Normal[count];
			}
		}


		public void InvalidateNormal(int index)
		{

		}

		public void AllocateLines(int count)
		{
			lock (VertexLock)
			{
				_originalLines = new Line[count];
			}

		}

		public void SetLine(int index, Line line)
		{
			_originalLines[index] = line;
		}

		public Line[] GetLines()
		{
			return _originalLines;
		}

		public Face GetFace(int index)
		{
			return _originalFaces[index];
		}

		public Vector GetFaceNormal(int index)
		{
			throw new NotImplementedException();
		}

		public void AllocateTextureCoordinates(int count)
		{
			lock (VertexLock)
			{
				_originalTextureCoordinates = new TextureCoordinate[count];
			}
		}

		public void SetTextureCoordinate(int index, TextureCoordinate textureCoordinate)
		{
			_originalTextureCoordinates[index] = textureCoordinate;
		}

		public void SetFaceTextureCoordinateIndex(int index, int a, int b, int c)
		{
			_textureCoordinateForFaces[index] = new FaceTextureCoordinate { A = a, B = b, C = c };
		}

		public TextureCoordinate[] GetTextureCoordinates()
		{
			return _originalTextureCoordinates;
		}

		public void SetMaterial(Material material, INode node)
		{
		}

		public void CalculateVertices(Viewport viewport, INode node)
		{

		}


		private Material GetActualMaterialFromFace(Material material, Face face)
		{
			if (null != material &&
				material.SubMaterials.Count >= face.MaterialId &&
				material.SubMaterials.Count != 0)
			{
				if (material.SubMaterials.ContainsKey(face.MaterialId))
				{
					material = material.SubMaterials[face.MaterialId];
				}
			}
			return material;
		}

		private Material GetMaterialForFace(Face face, INode node, Material material)
		{
			var actualMaterial = GetActualMaterialFromFace(material, face);

			if (null == actualMaterial)
			{
				if (node is IHaveColor)
				{
					actualMaterial = _colorMaterial;
					actualMaterial.Diffuse = ((IHaveColor)node).Color;
				}
				else
				{
					actualMaterial = Material.Default;
				}
			}
			return actualMaterial;
		}

		private void PrepareVertexBufferForLines(Geometry geometry)
		{
			if (null == _lineVertices)
			{
				_lineVertices = new LineRenderVertex[_originalLines.Length * 2];
			}

			var vertexIndex = 0;
			foreach (var line in _originalLines)
			{
				_lineVertices[vertexIndex++] = new LineRenderVertex(_originalVertices[line.A], Colors.White);
				_lineVertices[vertexIndex++] = new LineRenderVertex(_originalVertices[line.B], Colors.White);
			}
			if (null == _vertexBuffer)
			{
				_vertexBuffer = new VertexBuffer(D.GraphicsDevice, LineRenderVertex.Declaration, vertexIndex, BufferUsage.WriteOnly);
			}

		}

		private void PrepareVertexBufferForFaces(Geometry geometry)
		{
			lock (VertexLock)
			{
				if (null == _vertices)
				{
					_vertices = new RenderVertex[_originalFaces.Length * 3];
				}
				var vertexIndex = 0;
				var faceIndex = 0;

				var nodeMaterial = geometry.Material;

				foreach (var face in _originalFaces)
				{
					var color = Colors.Blue;
					var material = GetMaterialForFace(face, geometry, nodeMaterial);
					if (null != material)
					{
						color = material.Diffuse;
					}

					_vertices[vertexIndex++] = new RenderVertex(_originalVertices[face.C], color, _originalNormals[face.NormalC],
						_originalTextureCoordinates == null || _originalTextureCoordinates.Length == 0 ? TextureCoordinate.Zero : _originalTextureCoordinates[face.DiffuseC]) { FaceNormal = face.Normal };
					_vertices[vertexIndex++] = new RenderVertex(_originalVertices[face.B], color, _originalNormals[face.NormalB],
						_originalTextureCoordinates == null || _originalTextureCoordinates.Length == 0 ? TextureCoordinate.Zero : _originalTextureCoordinates[face.DiffuseB]) { FaceNormal = face.Normal };
					_vertices[vertexIndex++] = new RenderVertex(_originalVertices[face.A], color, _originalNormals[face.NormalA],
						_originalTextureCoordinates == null || _originalTextureCoordinates.Length == 0 ? TextureCoordinate.Zero : _originalTextureCoordinates[face.DiffuseA]) { FaceNormal = face.Normal };
					faceIndex++;
				}

				if (_vertexBuffer == null || _vertexBuffer.VertexCount != _vertices.Length)
					_vertexBuffer = new VertexBuffer(_graphicsDevice, RenderVertex.VertexDeclaration, _vertices.Length,
													 BufferUsage.WriteOnly);
				_vertexBuffer.SetData(0, _vertices, 0, _vertices.Length, 0);
			}

		}

		public void Render(Viewport viewport, INode node)
		{
			_renderingManager.RegisterForRendering(
				this,
				viewport,
				node,
				viewport.View.ViewMatrix,
				viewport.View.ProjectionMatrix,
				node.RenderingWorld);
		}



		internal void ActualRender(GraphicsDevice graphicsDevice, Viewport viewport, INode node, Matrix view, Matrix projection, Matrix world)
		{
			var drawFaces = null != _originalFaces;
			var drawLines = null != _originalLines;
			if (!_verticesPrepared)
			{
				_verticesPrepared = true;
				if (null != _originalFaces)
				{
					PrepareVertexBufferForFaces(node as Geometry);
				}
				else if (null != _originalLines)
				{
					PrepareVertexBufferForLines(node as Geometry);
				}
			}

			if (_vertexBuffer == null)
				return;

			var worldView = world * view;
			var worldViewProjection = worldView * projection;
			graphicsDevice.SetVertexShaderConstantFloat4(0, ref world);
			graphicsDevice.SetVertexShaderConstantFloat4(4, ref worldView);
			graphicsDevice.SetVertexShaderConstantFloat4(8, ref worldViewProjection);

			graphicsDevice.SetVertexBuffer(_vertexBuffer);
			var shader = ShaderManager.Instance.Gouraud;
			

			var geometry = node as Geometry;
			if (geometry != null && geometry.Material != null)
			{
				shader = geometry.Material.Shader;
				var reflectionIndex = 0;

				if (geometry.Material.DiffuseMap != null &&
					geometry.Material.DiffuseMap is ImageMap)
				{
					var imageMap = geometry.Material.DiffuseMap as ImageMap;
					var image = imageMap.Image;
					var imageContext = image.ImageContext as ImageContext;
					graphicsDevice.Textures[0] = imageContext.Texture;
					reflectionIndex = 1;
				}
				if (geometry.Material.ReflectionMap != null &&
					geometry.Material.ReflectionMap is ImageMap)
				{
					var imageMap = geometry.Material.ReflectionMap as ImageMap;
					var image = imageMap.Image;
					var imageContext = image.ImageContext as ImageContext;

					graphicsDevice.Textures[reflectionIndex] = imageContext.Texture;
				}
				if (geometry.Material.BumpMap != null &&
					geometry.Material.BumpMap is ImageMap)
				{
					var imageMap = geometry.Material.ReflectionMap as ImageMap;
					var image = imageMap.Image;
					var imageContext = image.ImageContext as ImageContext;

					graphicsDevice.Textures[2] = imageContext.Texture;
				}
				shader.HandleMaterial(graphicsDevice, geometry.Material);
			}
			shader.HandleLighting(graphicsDevice, viewport);
			graphicsDevice.SetShader(shader);

			if (drawFaces)
			{
				graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertexBuffer.VertexCount / 3);
			}
			if (drawLines)
			{
				//graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, _vertexBuffer.VertexCount / 3);
			}
		}




		public void SetNormal(int index, Normal normal)
		{

			_originalNormals[index] = normal;
		}

		Normal[] IGeometryDetailLevel.GetNormals()
		{
			return _originalNormals;
		}
	}
}
#endif	