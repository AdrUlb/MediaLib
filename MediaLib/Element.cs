using System;

namespace MediaLib
{
	public class Element : IDisposable
	{
		internal Screen? Screen = null;
		protected internal Window Window => Screen?.Window!;
		public int X { get; set; } = 0;
		public int Y { get; set; } = 0;
		public int Width { get; set; } = 50;
		public int Height { get; set; } = 50;

		private int depth = 0;

		public class UpdateEventArgs : EventArgs
		{
			public readonly TimeSpan Delta;

			public UpdateEventArgs(TimeSpan delta)
			{
				Delta = delta;
			}
		}

		public delegate void UpdateEventHandler(UpdateEventArgs e);
		public delegate void DrawEventHandler();

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

		protected virtual void OnUpdate(UpdateEventArgs e) { }
		protected virtual void OnDraw() { }

		internal void HandleUpdate(TimeSpan delta)
		{
			var e = new UpdateEventArgs(delta);

			Update?.Invoke(e);
			OnUpdate(e);
		}

		internal void HandleDraw()
		{
			Draw?.Invoke();
			OnDraw();
		}

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
