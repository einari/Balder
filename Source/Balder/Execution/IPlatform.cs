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
using System;
using Balder.Display;
using Balder.Input;

namespace Balder.Execution
{
	public enum PlatformState
	{
		Idle=0,
		Initialize,
		Load,
		Run,
		Exit
	}

	public delegate void PlatformStateChange(IPlatform platform, PlatformState state);

	public interface IPlatform
	{
		event PlatformStateChange BeforeStateChange;
		event PlatformStateChange StateChanged;

		string PlatformName { get; }

		string EntryAssemblyName { get; }

		bool IsInDesignMode { get; }
		IDisplayDevice DisplayDevice { get; }
		IMouseDevice MouseDevice { get; }


		Type DefaultFileLoaderType { get; }
		Type GeometryContextType { get; }
		Type SpriteContextType { get; }
		Type ImageContextType { get; }
		Type ShapeContextType { get; }
		Type MaterialCalculatorType { get; }
		Type SkyboxContextType { get; }

		PlatformState CurrentState { get; }
	}
}