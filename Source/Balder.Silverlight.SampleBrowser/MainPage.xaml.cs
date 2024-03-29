﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Balder.Execution;

namespace Balder.Silverlight.SampleBrowser
{
	public partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();

			Application.Current.Host.Settings.EnableFrameRateCounter = true;
			Application.Current.Host.Settings.MaxFrameRate = 60;
		}

		private void HandleGameInVisualTree(UIElement element, bool reload)
		{
			if( null == element )
			{
				return;
			}
			if( element is ItemsControl )
			{
				foreach( var item in ((ItemsControl)element).Items )
				{
					if( item is Game )
					{
						HandleGame((Game)item, reload);
						break;
					} else if( item is UIElement)
					{
						HandleGameInVisualTree((UIElement)item, reload);
					}
				}
			} else if( element is Panel )
			{
				foreach( var child in ((Panel)element).Children )
				{
					if( child is Game )
					{
						HandleGame((Game)child, reload);
						break;
					} else
					{
						HandleGameInVisualTree(child, reload);
					}
				}
			} else if( element is ContentControl )
			{
				var contentControl = element as ContentControl;
				if( contentControl.Content is Game )
				{
					HandleGame((Game)contentControl.Content, reload);
				} else if( contentControl.Content is UIElement )
				{
					HandleGameInVisualTree((UIElement)contentControl.Content, reload);
				}
			} else
			{
				var count = VisualTreeHelper.GetChildrenCount(element);
				if (count > 0)
				{
					var child = VisualTreeHelper.GetChild(element, 0);
					if (null != child && child is UIElement)
					{
						HandleGameInVisualTree(child as UIElement, reload);
					}
				}
			}
		}


		private void HandleGame(Game game, bool reload)
		{
			if( reload )
			{
			} else
			{
				game.Unload();
				game.RuntimeContext.PassiveRendering = false;
			}
		}


		
		private bool _tabItemChanged = false;
		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (null == TabControl)
			{
				return;
			}

			
			_tabItemChanged = true;
		}

		private void ContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
		{			
			_resourceView.Source = ContentFrame.Source;
		}

		private void ContentFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
		{
			if (!_tabItemChanged)
			{
				HandleGameInVisualTree(ContentFrame, false);
			}
			
			_tabItemChanged = false;
		}
	}
}
