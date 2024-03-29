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
using System.Threading;

namespace Balder.Extensions
{
	/// <summary>
	/// Extension methods for WaitHandles
	/// </summary>
	public static class WaitHandleExentions
	{
		/// <summary>
		/// Reset all <see cref="ManualResetEvent"/> in an array
		/// </summary>
		/// <param name="waitHandles">Array of <see cref="ManualResetEvent"/></param>
		public static void ResetAll(this ManualResetEvent[] waitHandles)
		{
			foreach (var waitHandle in waitHandles)
			{
				waitHandle.Reset();
			}
		}

		/// <summary>
		/// Set all <see cref="ManualResetEvent"/> in an array
		/// </summary>
		/// <param name="waitHandles">Array of <see cref="ManualResetEvent"/></param>
		public static void SetAll(this ManualResetEvent[] waitHandles)
		{
			foreach (var waitHandle in waitHandles)
			{
				waitHandle.Set();
			}
		}
	}
}
