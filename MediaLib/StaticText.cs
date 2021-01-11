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

		public int Size { get; }
		public string Text { get; }
		public Color Color { get; }

		internal StaticText(SDL.Texture sdlTexture, SDL.Renderer sdlRenderer, string text, int size, Color color)
		{
			this.sdlTexture = sdlTexture;
			this.sdlRenderer = sdlRenderer;
			Text = text;
			Color = color;
		}

		public void Draw(Rectangle? rect = null)
		{
			if (rect == null)
				rect = new Rectangle(0, 0, Width, Height);

			sdlRenderer.Copy(sdlTexture, null, rect);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			sdlTexture.Dispose();
		}
	}
}
