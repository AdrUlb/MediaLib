using ManagedSDL2;

namespace MediaLib
{
	public class PixelRenderer : Element
	{
		SDL.Texture sdlTexture;
		public int ContentWidth => sdlTexture.Width;
		public int ContentHeight => sdlTexture.Height;

		public PixelRenderer(Window window, int width, int height)
		{
			sdlTexture = new SDL.Texture(window.SdlRenderer, SDL.PixelFormat.RGBA8888, SDL.TextureAccess.Streaming, width, height);
		}

		protected override void OnDraw()
		{
			sdlTexture.Unlock();
		}

		public override void Dispose()
		{
			base.Dispose();

			sdlTexture.Dispose();
		}
	}
}
