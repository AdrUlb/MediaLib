using ManagedSDL2;
using System;
using System.Drawing;

namespace MediaLib
{
	public class PixelRendererElement : Element
	{
		SDL.Texture? sdlTexture = null;

		int contentWidth;
		int contentHeight;

		IntPtr pixelPtr;
		int pitch;

		Color[][] buffer;

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

			buffer = new Color[contentWidth][];

			for (var i = 0; i < buffer.Length; i++)
				buffer[i] = new Color[contentHeight];
		}

		private void CreateTexture()
		{
			if (sdlTexture != null)
				sdlTexture.Dispose();

			sdlTexture = new SDL.Texture(Window.SdlRenderer, SDL.PixelFormat.RGBA8888, SDL.TextureAccess.Streaming, contentWidth, contentHeight);

			for (var y = 0; y < contentHeight; y++)
				for (var x = 0; x < contentHeight; x++)
					this[x, y] = buffer[x][y];
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
			if (sdlTexture == null)
				return;

			if (sdlTexture.Locked)
				sdlTexture.Unlock();

			Window.SdlRenderer.Copy(sdlTexture, null, new Rectangle(0, 0, Width, Height));
		}

		public override void Dispose()
		{
			base.Dispose();

			sdlTexture?.Dispose();
		}

		public Color this[int x, int y]
		{
			get
			{
				if (x < 0 || y < 0 || x >= contentWidth || y >= contentHeight)
					throw new IndexOutOfRangeException("Specified coordinates outside of pixel buffer bounds");

				return buffer[x][y];
			}

			set
			{
				if (x < 0 || y < 0 || x >= contentWidth || y >= contentHeight)
					throw new IndexOutOfRangeException("Specified coordinates outside of pixel buffer bounds");

				buffer[x][y] = value;

				if (sdlTexture == null)
					return;

				if (!sdlTexture.Locked)
					(pixelPtr, pitch) = sdlTexture.Lock();

				var pixelIndex = x * 4 + y * pitch;

				unsafe
				{
					var __unsafe__pixelPtr = new Span<byte>(pixelPtr.ToPointer(), pitch * contentHeight);

					__unsafe__pixelPtr[pixelIndex] = value.A;
					__unsafe__pixelPtr[pixelIndex + 1] = value.B;
					__unsafe__pixelPtr[pixelIndex + 2] = value.G;
					__unsafe__pixelPtr[pixelIndex + 3] = value.R;
				}
			}
		}
	}
}
