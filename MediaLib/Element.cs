using System;
using System.Drawing;

namespace MediaLib
{
	public class Element : IDisposable
	{
		internal Screen? Screen = null;
		protected internal Window Window => Screen?.Window!;

		public Rectangle Rect = new Rectangle(0, 0, 50, 50);
		public int X { get => Rect.X; set => Rect.X = value; }
		public int Y { get => Rect.Y; set => Rect.Y = value; }
		public int Width { get => Rect.Width; set => Rect.Width = value; }
		public int Height { get => Rect.Height; set => Rect.Height = value; }

		private int depth = 0;

		public Color BackgroundColor = Color.White;

		public class UpdateEventArgs : EventArgs
		{
			public readonly TimeSpan Delta;

			public UpdateEventArgs(TimeSpan delta)
			{
				Delta = delta;
			}
		}

		public delegate void ShowEventHandler();
		public delegate void HideEventHandler();
		public delegate void UpdateEventHandler(UpdateEventArgs e);
		public delegate void DrawEventHandler();

		public event ShowEventHandler? Show;
		public event HideEventHandler? Hide;
		public event UpdateEventHandler? Update;
		public event DrawEventHandler? Draw;

		public int Depth
		{
			get => depth;
			set
			{
				depth = value;
				Screen?.OrderElements();
			}
		}

		protected virtual void OnShow() { }
		protected virtual void OnHide() { }
		protected virtual void OnUpdate(UpdateEventArgs e) { }
		protected virtual void OnDraw() { }

		internal void HandleUpdate(TimeSpan delta)
		{
			var e = new UpdateEventArgs(delta);

			Update?.Invoke(e);
			OnUpdate(e);
		}

		internal void HandleShow()
		{
			Show?.Invoke();
			OnShow();
		}

		internal void HandleHide()
		{
			Hide?.Invoke();
			OnHide();
		}

		internal void HandleDraw()
		{
			Window.FillRect(BackgroundColor, Rect);

			Draw?.Invoke();
			OnDraw();
		}

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
