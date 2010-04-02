#region License

//
// Author: Einar Ingebrigtsen <einar@dolittle.com>
// Copyright (c) 2007-2009, DoLittle Studios
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

using System.Collections.ObjectModel;
using Balder.Core;
using Balder.Core.Math;

namespace Balder.Silverlight.SampleBrowser.Samples.Data.HierarchicalNodesControl
{
	public class Row
	{
		public const int ColumnCount = 10;
		public const double ColumnSpace = 12;

		public Row(double yPosition)
		{
			Columns = new ObservableCollection<Column>();


			var position = -(ColumnSpace*(ColumnCount/2d));
			for( var columnIndex=0; columnIndex<ColumnCount; columnIndex++ )
			{
				var column = new Column
				             	{
				             		Position = new Coordinate(position, yPosition, 0),
									Color = Color.Random()
				             	};
				Columns.Add(column);
				position += ColumnSpace;
			}
		}

		public ObservableCollection<Column> Columns { get; private set; }
	}
}