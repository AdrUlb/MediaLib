using ManagedSDL2;
using System;
using System.ComponentModel;

namespace MediaLib
{
	public partial class Window : IDisposable
	{
		public readonly App App;

		readonly SDL.Window sdlWindow;
		internal readonly SDL.Renderer SdlRenderer;

		bool open = false;

		public string Title
		{
			get => sdlWindow.Title;
			set => sdlWindow.Title = value;
		}

		public int X { get => sdlWindow.Position.X; set => sdlWindow.Position = (value, Y); }
		public int Y { get => sdlWindow.Position.Y; set => sdlWindow.Position = (X, value); }
		public int Width { get => sdlWindow.Size.Width; set => sdlWindow.Size = (value, Height); }
		public int Height { get => sdlWindow.Size.Height; set => sdlWindow.Size = (Width, value); }

		public Screen? screen = null;
		public Screen? Screen
		{
			get => screen;

			set
			{
				if (screen != null)
					screen.Window = null;

				screen = value;

				if (screen != null)
					screen.Window = this;
			}
		}

		public class ClosingEventArgs : CancelEventArgs
		{
			public ClosingEventArgs() : base(false) { }
		}

		public delegate void ClosingEventHandler(ClosingEventArgs e);

		public event ClosingEventHandler? Closing;

		protected virtual void OnClosing(ClosingEventArgs e) { }

		internal void HandleClosing()
		{
			var e = new ClosingEventArgs();

			Closing?.Invoke(e);

			if (e.Cancel)
				return;

			OnClosing(e);

			if (e.Cancel)
				return;

			ForceClose();
		}

		internal void HandleUpdate(TimeSpan delta)
		{
			Screen?.HandleUpdate(delta);
		}

		internal void HandleDraw()
		{
			Screen?.HandleDraw();
		}

		public Window(App app, string title, int width, int height)
		{
			App = app;

			try
			{
				SDL.Init(SDL.InitFlags.Video);
				sdlWindow = new SDL.Window(title, SDL.WindowPos.Undefined, SDL.WindowPos.Undefined, width, height, false);
				SdlRenderer = new SDL.Renderer(sdlWindow);
				SdlRenderer.SetDrawBlendMode(SDL.BlendMode.Blend);
			}
			catch (SDL.ErrorException ex)
			{
				Console.WriteLine($"SDL error: {ex.Message}");
				Environment.Exit(-1);
			}

			App.RegisterWindow(this);

			sdlWindow.CloseRequested += SdlWindow_CloseRequested;
		}

		private void SdlWindow_CloseRequested()
		{
			Close();
		}

		public void Open()
		{
			if (open)
				return;

			sdlWindow.Show();
			open = true;

			App.WindowNowOpen(this);
		}

		public void Close()
		{
			if (!open)
				return;

			HandleClosing();
		}

		public void ForceClose()
		{
			if (!open)
				return;

			open = false;
			App.WindowNowClosed(this);
			sdlWindow.Hide();

		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			sdlWindow.CloseRequested -= SdlWindow_CloseRequested;
			App.UnregisterWindow(this);
			sdlWindow.Dispose();
		}
	}
}
