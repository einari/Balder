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
using Balder.Content;
using Balder.Diagnostics;
using Balder.Display;
using Ninject;

namespace Balder.Execution
{
	public interface IRuntime
	{
		T CreateGame<T>() where T : Game;
		Game CreateGame(Type type);
		void RegisterGame(IDisplay display, Game game);
		void UnregisterGame(Game game);

		void EnableStatistics();
		void DisableStatistics();

		IStopwatch Stopwatch { get; }
		IPlatform Platform { get; }
		IKernel Kernel { get; }
		IContentManager ContentManager { get; }
	}
}