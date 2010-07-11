#region License
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
using Balder.Core.Debug;
using Balder.Core.Display;
using Balder.Core.Math;
using Balder.Core.Rendering;
using Balder.Core.View;

#if(SILVERLIGHT)
using System.Windows;
#endif

namespace Balder.Core.Execution
{
	public delegate void GameEventHandler(Game game);

	public partial class Game : Actor
	{
		public IRuntimeContext RuntimeContext { get; private set; }

		public event GameEventHandler Update = (s) => { };
		public event GameEventHandler Initialize = (s) => { };
		public event GameEventHandler LoadContent = (s) => { };

#if(SILVERLIGHT)
		public Game()
			: this(
				Runtime.Instance.Kernel.Get<IRuntimeContext>(),
				Runtime.Instance.Kernel.Get<INodeRenderingService>()
			)
		{
		}
#endif

		public Game(IRuntimeContext runtimeContext, INodeRenderingService renderingService)
		{
			RuntimeContext = runtimeContext;
			Viewport = new Viewport(runtimeContext) { Width = 800, Height = 600 };
			Scene = new Scene(renderingService);
			Camera = new Camera() { Target = Vector.Forward, Position = Vector.Zero };
			Constructed();
			PassiveRenderingMode = PassiveRenderingMode.FullDetail;

			Messenger.DefaultContext.SubscriptionsFor<UpdateMessage>().AddListener(this, UpdateAction);
		}

		public void Uninitialize()
		{
			Messenger.DefaultContext.SubscriptionsFor<UpdateMessage>().RemoveListener(this, UpdateAction);
		}

		partial void Constructed();

		public static readonly Property<Game, Camera> CameraProp = Property<Game, Camera>.Register(g => g.Camera);
		public Camera Camera
		{
			get { return CameraProp.GetValue(this); }
			set
			{
				var previousCamera = Camera;
#if(SILVERLIGHT)
				if (null != previousCamera)
				{
					if (Children.Contains(previousCamera))
					{
						Children.Remove(previousCamera);
					}
				}
				Children.Add(value);
				value.Width = 0;
				value.Height = 0;
				value.Visibility = Visibility.Collapsed;
#endif
				CameraProp.SetValue(this, value);
				Viewport.View = value;

			}
		}

		private Scene _scene;
		public Scene Scene
		{
			get { return _scene; }
			set
			{
				_scene = value;
				Viewport.Scene = value;
			}
		}

		private Viewport _viewport;
		public Viewport Viewport
		{
			get { return _viewport; }
			private set
			{
				_viewport = value;
				// Todo: This should be injected - need to figure out how to do this properly!
				Viewport.Display = Display;
			}
		}

		public DebugInfo DebugInfo
		{
			get { return Viewport.DebugInfo; }
			set { Viewport.DebugInfo = value; }
		}

		private bool _passiveRendering;
		public bool PassiveRendering
		{
			get { return _passiveRendering; }
			set
			{
				_passiveRendering = value;
				RuntimeContext.PassiveRendering = true;
			}
		}

		private PassiveRenderingMode _passiveRenderingMode;
		public PassiveRenderingMode PassiveRenderingMode
		{
			get { return _passiveRenderingMode; }
			set
			{
				_passiveRenderingMode = value;
				RuntimeContext.PassiveRenderingMode = value;
			}
		}

		public static readonly Property<Game, bool> IsPausedProperty =
			Property<Game, bool>.Register(g => g.IsPaused);
		public bool IsPaused
		{
			get { return IsPausedProperty.GetValue(this); }
			set
			{
				IsPausedProperty.SetValue(this, value);

				// Todo: Figure out a better way to pause/halt everything - hate to have this value floating around everywhere
				Scene.IsPaused = value;
				RuntimeContext.Paused = value;
				if( null != Display )
				{
					Display.Halted = value;
				}
			}
		}


		public override void OnLoadContent()
		{
			LoadContent(this);
		}

		public override void OnInitialize()
		{
			Initialize(this);
			base.OnInitialize();
		}

		private void UpdateAction(UpdateMessage updateMessage)
		{
			OnUpdateOccured();
			Update(this);
		}
	}
}