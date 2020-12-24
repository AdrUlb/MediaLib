﻿using ManagedSDL2;
using System.Collections.Generic;

namespace MediaLib
{
	public class App
	{
		List<Window> existingWindows = new List<Window>();
		List<Window> openWindows = new List<Window>();

		bool running = false;

		public delegate void ActivateEventHandler();
		public delegate void DeactivateEventHandler();
		public delegate void AllWindowsClosedEventHandler();

		public event ActivateEventHandler? Activate;
		public event ActivateEventHandler? Deactivate;
		public event AllWindowsClosedEventHandler? AllWindowsClosed;

		protected virtual void OnActivate() { }
		protected virtual void OnDeactivate() { }
		protected virtual void OnAllWindowsClosed() { }

		internal void HandleActivate()
		{
			Activate?.Invoke();
			OnActivate();
		}

		internal void HandleDeactivate()
		{
			Deactivate?.Invoke();
			OnDeactivate();
		}

		internal void HandleAllWindowsClosed()
		{
			AllWindowsClosed?.Invoke();
			OnAllWindowsClosed();
		}

		public void Run()
		{
			running = true;

			HandleActivate();

			while (running)
			{
				SDL.ProcessEvents();
			}

			HandleDeactivate();
		}

		public void Quit()
		{
			running = false;
		}

		internal void RegisterWindow(Window window)
		{
			existingWindows.Add(window);
		}

		internal void UnregisterWindow(Window window)
		{
			existingWindows.Remove(window);
		}

		internal void WindowNowOpen(Window window)
		{
			openWindows.Add(window);
		}

		internal void WindowNowClosed(Window window)
		{
			openWindows.Remove(window);

			if (openWindows.Count == 0)
				HandleAllWindowsClosed();
		}
	}
}