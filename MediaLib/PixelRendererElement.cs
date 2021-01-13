using ManagedSDL2;

namespace MediaLib
{
	public class PixelRendererElement : Element
	{
		SDL.Texture? sdlTexture = null;

		int contentWidth;
		int contentHeight;

		public int ContentWidth
		{
			get => contentWidth;
			set
			{
				contentWidth = value;
				if (sdlTexture != null)
					CreateTexture();
			}
		}

		public int ContentHeight
		{
			get => contentHeight;
			set
			{
				contentHeight = value;
				if (sdlTexture != null)
					CreateTexture();
			}
		}

		public PixelRendererElement(int contentWidth, int contentHeight)
		{
			this.contentWidth = contentWidth;
			this.contentHeight = contentHeight;
		}

		private void CreateTexture()
		{
			if (sdlTexture != null)
				sdlTexture.Dispose();

			sdlTexture = new SDL.Texture(Window.SdlRenderer, SDL.PixelFormat.RGBA8888, SDL.TextureAccess.Streaming, contentWidth, contentHeight);
		}

		protected override void OnShow()
		{
			CreateTexture();
		}

		protected override void OnHide()
		{
			sdlTexture?.Dispose();
			sdlTexture = null;
		}

		protected override void OnDraw()
		{
			if (sdlTexture != null && sdlTexture.Locked)
				sdlTexture.Unlock();
		}

		public override void Dispose()
		{
			base.Dispose();

			sdlTexture?.Dispose();
		}
	}
}
