﻿using System.Collections.Generic;
using Balder.Core.Display;
using Balder.Core.Input;
using Balder.Core.Math;
using Balder.Core.Objects.Geometries;
using Balder.Core.Silverlight.Helpers;

namespace Balder.Silverlight.SampleBrowser.Samples.Creative.RubicsCube
{
	public partial class Cube
	{
		public delegate void ManipulateGroupEventHandler(CubeBoxGroup group, ManipulationDeltaEventArgs args);

		public const int Depth = 3;
		public const int Width = 3;
		public const int Height = 3;

		private Dictionary<BoxSide, CubeBoxGroup> _groups;
		private CubeBoxGroup _manipulatingGroup;
		private ManipulateGroupEventHandler _manipulateGroupHandler;
		private CubeBox[, ,] _boxesGrid;



		public Cube()
		{
			InitializeComponent();
			SetupEvents();
		}

		private void SetupEvents()
		{
			ManipulationStarted += Cube_ManipulationStarted;
			ManipulationDelta += Cube_ManipulationDelta;
			ManipulationStopped += Cube_ManipulationStopped;
		}


		public DependencyProperty<Cube, MoveRecorder> MoveRecorderProperty =
			DependencyProperty<Cube, MoveRecorder>.Register(c => c.MoveRecorder);
		
		public MoveRecorder MoveRecorder
		{
			get { return MoveRecorderProperty.GetValue(this); }
			set 
			{ 
				MoveRecorderProperty.SetValue(this,value);
				InitializeMoveRecorder();
			}
		}

		public void Solve()
		{
			if( null != MoveRecorder )
			{
				MoveRecorder.Solve();
			}
		}


		private void InitializeMoveRecorder()
		{
			if( null != _groups && _groups.Count > 0 )
			{
				foreach( var group in _groups.Values )
				{
					MoveRecorder.AddGroup(group);
				}
			}
		}

		void Cube_ManipulationStarted(object sender, ManipulationDeltaEventArgs args)
		{
			var cubeBox = args.OriginalSource as CubeBox;
			if (null == cubeBox)
			{
				return;
			}

			if (null != args.Face)
			{
				var rotation = Matrix.CreateRotation(
					(float)cubeBox.Rotation.X,
					(float)cubeBox.Rotation.Y,
					(float)cubeBox.Rotation.Z);

				var normal = Vector.TransformNormal(args.Face.Normal, rotation);
				normal.Normalize();

				var upLength = (Vector.Up - normal).Length;
				var frontLength = (Vector.Backward - normal).Length;
				var rightLength = (Vector.Right - normal).Length;

				_manipulatingGroup = null;
				if (upLength < 0.1)
				{
					if (args.Direction == ManipulationDirection.Up ||
						args.Direction == ManipulationDirection.Down)
					{
						var BoxSides = new[] { BoxSide.Front, BoxSide.Back };
						_manipulatingGroup = FindBoxInGroup(BoxSides, cubeBox);
						_manipulateGroupHandler = (g, a) => g.Rotate(a.DeltaY);
					}
				}
				else if (frontLength < 0.1)
				{
					if (args.Direction == ManipulationDirection.Left ||
						args.Direction == ManipulationDirection.Right)
					{
						var BoxSides = new[] { BoxSide.Top, BoxSide.Bottom };
						_manipulatingGroup = FindBoxInGroup(BoxSides, cubeBox);
						_manipulateGroupHandler = (g, a) => g.Rotate(-a.DeltaX);
					}
					else if (args.Direction == ManipulationDirection.Up ||
							  args.Direction == ManipulationDirection.Down)
					{
						var BoxSides = new[] { BoxSide.Left, BoxSide.Right };
						_manipulatingGroup = FindBoxInGroup(BoxSides, cubeBox);
						_manipulateGroupHandler = (g, a) => g.Rotate(-a.DeltaY);
					}
				}
				else if (rightLength < 0.1)
				{
					if (args.Direction == ManipulationDirection.Left ||
						args.Direction == ManipulationDirection.Right)
					{
						var BoxSides = new[] { BoxSide.Top, BoxSide.Bottom };
						_manipulatingGroup = FindBoxInGroup(BoxSides, cubeBox);
						_manipulateGroupHandler = (g, a) => g.Rotate(-a.DeltaX);
					}
					else if (args.Direction == ManipulationDirection.Up ||
							  args.Direction == ManipulationDirection.Down)
					{
						var BoxSides = new[] { BoxSide.Front, BoxSide.Back };
						_manipulatingGroup = FindBoxInGroup(BoxSides, cubeBox);
						_manipulateGroupHandler = (g, a) => g.Rotate(-a.DeltaY);
					}
				}

			}
		}


		void Cube_ManipulationDelta(object sender, ManipulationDeltaEventArgs args)
		{
			var cubeBox = args.OriginalSource as CubeBox;
			if (null == cubeBox)
			{
				return;
			}

			if (null != _manipulatingGroup && null != _manipulateGroupHandler)
			{
				_manipulateGroupHandler(_manipulatingGroup, args);
			}
		}

		void Cube_ManipulationStopped(Core.INode sender, Core.Execution.BubbledEventArgs eventArgs)
		{
			if (null != _manipulatingGroup)
			{
				_manipulatingGroup.Snap();
			}

			_manipulatingGroup = null;
			_manipulateGroupHandler = null;
		}

		private CubeBoxGroup FindBoxInGroup(IEnumerable<BoxSide> BoxSides, CubeBox cubeBox)
		{
			foreach (var BoxSide in BoxSides)
			{
				var group = _groups[BoxSide];
				foreach (var box in group.Boxes)
				{
					if (box.Equals(cubeBox))
					{
						return group;
					}
				}
			}
			return null;
		}


		public override void Prepare(Viewport viewport)
		{
			PrepareCubeBoxGroups();
			GenerateCubeBoxes();
			OrganizeBoxes();
			base.Prepare(viewport);
		}


		private void PrepareCubeBoxGroups()
		{
			_groups = new Dictionary<BoxSide, CubeBoxGroup>();
			_groups[BoxSide.Front] = CreateCubeBoxGroup(BoxSide.Front);
			_groups[BoxSide.Back] = CreateCubeBoxGroup(BoxSide.Back);
			_groups[BoxSide.Left] = CreateCubeBoxGroup(BoxSide.Left);
			_groups[BoxSide.Right] = CreateCubeBoxGroup(BoxSide.Right);
			_groups[BoxSide.Top] = CreateCubeBoxGroup(BoxSide.Top);
			_groups[BoxSide.Bottom] = CreateCubeBoxGroup(BoxSide.Bottom);
		}

		private CubeBoxGroup CreateCubeBoxGroup(BoxSide side)
		{
			var group = new CubeBoxGroup(side);
			if (null != MoveRecorder)
			{
				MoveRecorder.AddGroup(group);
			}
			return group;
		}

		private void GenerateCubeBoxes()
		{
			_boxesGrid = new CubeBox[Width, Height, Depth];
			for (var z = 0; z < Depth; z++)
			{
				for (var y = 0; y < Height; y++)
				{
					for (var x = 0; x < Width; x++)
					{
						var box = new CubeBox(x, y, z);
						Children.Add(box);
						_boxesGrid[x, y, z] = box;
					}
				}
			}
		}

		private void OrganizeBoxes()
		{
			OrganizeFrontBoxes();
			OrganizeBackBoxes();
			OrganizeLeftBoxes();
			OrganizeRightBoxes();
			OrganizeTopBoxes();
			OrganizeBottomBoxes();
		}

		private void OrganizeFrontBoxes()
		{
			var group = _groups[BoxSide.Front];

			group.Add(_boxesGrid[0, 0, 0], BoxSide.Left, BoxSide.Top);
			group.Add(_boxesGrid[1, 0, 0], BoxSide.Top);
			group.Add(_boxesGrid[2, 0, 0], BoxSide.Top, BoxSide.Right);
			group.Add(_boxesGrid[2, 1, 0], BoxSide.Right);
			group.Add(_boxesGrid[2, 2, 0], BoxSide.Right, BoxSide.Bottom);
			group.Add(_boxesGrid[1, 2, 0], BoxSide.Bottom);
			group.Add(_boxesGrid[0, 2, 0], BoxSide.Bottom, BoxSide.Left);
			group.Add(_boxesGrid[0, 1, 0], BoxSide.Left);
			group.Add(_boxesGrid[1, 1, 0]);
		}

		private void OrganizeBackBoxes()
		{
			var group = _groups[BoxSide.Back];

			group.Add(_boxesGrid[0, 0, 2], BoxSide.Left, BoxSide.Top);
			group.Add(_boxesGrid[1, 0, 2], BoxSide.Top);
			group.Add(_boxesGrid[2, 0, 2], BoxSide.Top, BoxSide.Right);
			group.Add(_boxesGrid[2, 1, 2], BoxSide.Right);
			group.Add(_boxesGrid[2, 2, 2], BoxSide.Right, BoxSide.Bottom);
			group.Add(_boxesGrid[1, 2, 2], BoxSide.Bottom);
			group.Add(_boxesGrid[0, 2, 2], BoxSide.Bottom, BoxSide.Left);
			group.Add(_boxesGrid[0, 1, 2], BoxSide.Left);
			group.Add(_boxesGrid[1, 1, 2]);
		}

		private void OrganizeLeftBoxes()
		{
			var group = _groups[BoxSide.Left];

			group.Add(_boxesGrid[0, 0, 2], BoxSide.Back, BoxSide.Top);
			group.Add(_boxesGrid[0, 0, 1], BoxSide.Top);
			group.Add(_boxesGrid[0, 0, 0], BoxSide.Top, BoxSide.Front);
			group.Add(_boxesGrid[0, 1, 0], BoxSide.Front);
			group.Add(_boxesGrid[0, 2, 0], BoxSide.Front, BoxSide.Bottom);
			group.Add(_boxesGrid[0, 2, 1], BoxSide.Bottom);
			group.Add(_boxesGrid[0, 2, 2], BoxSide.Bottom, BoxSide.Back);
			group.Add(_boxesGrid[0, 1, 2], BoxSide.Back);
			group.Add(_boxesGrid[0, 1, 1]);
		}

		private void OrganizeRightBoxes()
		{
			var group = _groups[BoxSide.Right];

			group.Add(_boxesGrid[2, 0, 2], BoxSide.Back, BoxSide.Top);
			group.Add(_boxesGrid[2, 0, 1], BoxSide.Top);
			group.Add(_boxesGrid[2, 0, 0], BoxSide.Top, BoxSide.Front);
			group.Add(_boxesGrid[2, 1, 0], BoxSide.Front);
			group.Add(_boxesGrid[2, 2, 0], BoxSide.Front, BoxSide.Bottom);
			group.Add(_boxesGrid[2, 2, 1], BoxSide.Bottom);
			group.Add(_boxesGrid[2, 2, 2], BoxSide.Bottom, BoxSide.Back);
			group.Add(_boxesGrid[2, 1, 2], BoxSide.Back);
			group.Add(_boxesGrid[2, 1, 1]);
		}

		private void OrganizeTopBoxes()
		{
			var group = _groups[BoxSide.Top];

			group.Add(_boxesGrid[2, 0, 2], BoxSide.Right, BoxSide.Back);
			group.Add(_boxesGrid[1, 0, 2], BoxSide.Back);
			group.Add(_boxesGrid[0, 0, 2], BoxSide.Back, BoxSide.Left);
			group.Add(_boxesGrid[0, 0, 1], BoxSide.Left);
			group.Add(_boxesGrid[0, 0, 0], BoxSide.Left, BoxSide.Front);
			group.Add(_boxesGrid[1, 0, 0], BoxSide.Front);
			group.Add(_boxesGrid[2, 0, 0], BoxSide.Front, BoxSide.Right);
			group.Add(_boxesGrid[2, 0, 1], BoxSide.Right);
			group.Add(_boxesGrid[1, 0, 1]);
		}

		private void OrganizeBottomBoxes()
		{
			var group = _groups[BoxSide.Bottom];

			group.Add(_boxesGrid[2, 2, 2], BoxSide.Right, BoxSide.Back);
			group.Add(_boxesGrid[1, 2, 2], BoxSide.Back);
			group.Add(_boxesGrid[0, 2, 2], BoxSide.Back, BoxSide.Left);
			group.Add(_boxesGrid[0, 2, 1], BoxSide.Left);
			group.Add(_boxesGrid[0, 2, 0], BoxSide.Left, BoxSide.Front);
			group.Add(_boxesGrid[1, 2, 0], BoxSide.Front);
			group.Add(_boxesGrid[2, 2, 0], BoxSide.Front, BoxSide.Right);
			group.Add(_boxesGrid[2, 2, 1], BoxSide.Right);
			group.Add(_boxesGrid[1, 2, 1]);
		}
	}
}
