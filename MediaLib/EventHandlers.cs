using System;

namespace MediaLib
{
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
}
