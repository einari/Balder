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
using Balder.Materials;
using Balder.Math;
namespace Balder
{
    public class DataItemInfo
    {
        public Vector Position;
        public Vector Scale;
        public Vector Rotation;
        public Matrix Matrix;
        public object DataItem;
        public BoundingSphere BoundingSphere;
        public RenderableNode Node;
        public Material Material;

        public DataItemInfo()
        {
            Position = Vector.Zero;
            Rotation = Vector.Zero;
            Scale = new Vector(1, 1, 1);
            Matrix = Matrix.Identity;
        }

        public void Prepare()
        {
            var translation = Matrix.CreateTranslation(Position);
            var scale = Matrix.CreateScale(Scale);
            var rotation = Matrix.CreateRotation(Rotation.X, Rotation.Y, Rotation.Z);
            Matrix = scale * rotation * translation;
        }
    }
}
