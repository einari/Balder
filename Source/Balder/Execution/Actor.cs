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
#if(SILVERLIGHT)
using System.Windows.Controls;
#endif
using Balder.Collections;
using Balder.Content;
using Balder.Display;
using Balder.Input;
using Ninject;

namespace Balder.Execution
{
	/// <summary>
	/// Base class for all actors.
	/// </summary>
#if(SILVERLIGHT)
	public class Actor : Grid, IActor
#else
	public class Actor : IActor
#endif
	{
		protected Actor()
		{
			Actors = new ActorCollection();
		}

		/// <summary>
		/// Gets a collection of all actors contained in the actor - Sub Actors
		/// </summary>
		public ActorCollection Actors { get; private set; }

		/// <summary>
		/// Gets a boolean indicating wether or not the Actor has initialized
		/// </summary>
		public bool HasInitialized { get; private set; }


		/// <summary>
		/// Gets a boolean indicating wether or not the Actor has loaded
		/// </summary>
		public bool HasLoaded { get; private set; }

		/// <summary>
		/// Gets a boolean indicating wether or not the Actor has been updated
		/// </summary>
		public bool HasUpdated { get; private set; }

		protected void AddActor(Actor actor)
		{
			Actors.Add(actor);
		}

		/// <summary>
		/// Gets the current state of the actor
		/// </summary>
		public ActorState State { get; private set; }
	
		public virtual void OnBeforeInitialize() { }
		public virtual void OnInitialize() { }

		public virtual void OnLoadContent() { }
		public virtual void OnLoaded() { }
		public virtual void OnStopped() { }

		public virtual void OnBeforeUpdate() { }
		public virtual void OnUpdate() { }
		public virtual void OnAfterUpdate() { }

		public void ChangeState(ActorState state)
		{
			switch (state)
			{
				case ActorState.Initialize:
					{
						OnInitializeOccured();
					}
					break;
				case ActorState.Load:
					{
						OnLoadContentOccured();
					}
					break;
			}
			State = state;
		}

		public void Stop()
		{
			foreach (var actor in Actors)
			{
				actor.OnStopped();
			}
		}


		private void ExecuteActionOnActors(Action<Actor> action)
		{
			foreach (var actor in Actors)
			{
				action(actor);
			}
		}



		private void OnInitializeOccured()
		{
			OnBeforeInitialize();
			OnInitialize();
			ExecuteActionOnActors(a => a.OnInitialize());
			HasInitialized = true;
		}

		private void OnLoadContentOccured()
		{
			OnLoadContent();
			ExecuteActionOnActors(a => a.OnLoadContent());
			HasLoaded = true;
		}

		internal void OnUpdateOccured()
		{
			ExecuteActionOnActors(a => a.OnBeforeUpdate());
			ExecuteActionOnActors(a => a.OnUpdate());
			OnBeforeUpdate();
			OnUpdate();
			OnAfterUpdate();
			ExecuteActionOnActors(a => a.OnAfterUpdate());
			HasUpdated = true;
		}


		#region Services
		[Inject]
		public IContentManager ContentManager { get; set; }

		[Inject]
		public IDisplay Display { get; set; }

		[Inject]
		public IMouseManager MouseManager { get; set; }

		[Inject]
		public Mouse Mouse { get; set; }

		[Inject]
		public IPlatform Platform { get; set; }
		#endregion
	}
}