using ManagedSDL2;
using System;

namespace MediaLib
{
	public class Window
	{
		SDLWindow sdlWindow;

		public Window()
		{
			SDL.Init(SDLInitFlags.Video);
			sdlWindow = new SDLWindow("", SDL.WindowPosUndefined, SDL.WindowPosUndefined, 400, 400, true);
		}

		~Window() => Dispose();

		private void Dispose()
		{
			Console.WriteLine("hi");
			GC.SuppressFinalize(this);
			sdlWindow.Dispose();
		}
	}
}
