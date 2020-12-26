using ManagedSDL2;
using System;
using System.ComponentModel;

namespace MediaLib
{
	public class Window : Element, IDisposable
	{
		public readonly App App;

		readonly SDL.Window sdlWindow;

		bool open = false;

		public override int X { get => sdlWindow.Position.X; set => sdlWindow.Position = (value, Y); }
		public override int Y { get => sdlWindow.Position.Y; set => sdlWindow.Position = (X, value); }
		public override int Width { get => sdlWindow.Size.Width; set => sdlWindow.Size = (value, Height); }
		public override int Height { get => sdlWindow.Size.Height; set => sdlWindow.Size = (Width, value); }

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

		public Window(App app, string title, int width, int height)
		{
			App = app;

			try
			{
				SDL.Init(SDL.InitFlags.Video);
				sdlWindow = new SDL.Window(title, SDL.WindowPos.Undefined, SDL.WindowPos.Undefined, width, height, false);
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
