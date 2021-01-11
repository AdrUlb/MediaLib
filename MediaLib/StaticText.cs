using ManagedSDL2;
using System;
using System.Drawing;

namespace MediaLib
{
	public class StaticText : IText, IDisposable
	{
		SDL.Texture sdlTexture;
		SDL.Renderer sdlRenderer;

		public int Width => sdlTexture.Width;
		public int Height => sdlTexture.Height;

		internal StaticText(SDL.Texture sdlTexture, SDL.Renderer sdlRenderer)
		{
			this.sdlTexture = sdlTexture;
			this.sdlRenderer = sdlRenderer;
		}

		public void Draw(Rectangle rect)
		{
			sdlRenderer.Copy(sdlTexture, null, rect);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			sdlTexture.Dispose();
		}
	}
}
