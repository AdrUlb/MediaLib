using ManagedSDL2;
using System;
using System.Drawing;

namespace MediaLib
{
	public class PixelRendererElement : Element
	{
		SDL.Texture? sdlTexture = null;

		int canvasWidth;
		int canvasHeight;

		IntPtr pixelPtr;
		int pitch;

		Color[][] buffer;

		public int CanvasWidth
		{
			get => canvasWidth;
			set
			{
				canvasWidth = value;
				if (sdlTexture != null)
					CreateTexture();
			}
		}

		public int CanvasHeight
		{
			get => canvasHeight;
			set
			{
				canvasHeight = value;
				if (sdlTexture != null)
					CreateTexture();
			}
		}

		public PixelRendererElement(int canvasWidth, int canvasHeight)
		{
			this.canvasWidth = canvasWidth;
			this.canvasHeight = canvasHeight;

			buffer = new Color[canvasWidth][];

			for (var i = 0; i < buffer.Length; i++)
				buffer[i] = new Color[canvasHeight];
		}

		private void CreateTexture()
		{
			if (sdlTexture != null)
				sdlTexture.Dispose();

			sdlTexture = new SDL.Texture(Window.SdlRenderer, SDL.PixelFormat.RGBA8888, SDL.TextureAccess.Streaming, canvasWidth, canvasHeight);

			for (var y = 0; y < canvasHeight; y++)
				for (var x = 0; x < canvasWidth; x++)
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

		public void Clear(Color color)
		{
			for (var y = 0; y < CanvasHeight; y++)
				for (var x = 0; x < CanvasWidth; x++)
					this[x, y] = color;
		}

		public void Clear() => Clear(BackgroundColor);

		public override void Dispose()
		{
			base.Dispose();

			sdlTexture?.Dispose();
		}

		public Color this[int x, int y]
		{
			get
			{
				if (x < 0 || y < 0 || x >= canvasWidth || y >= canvasHeight)
					throw new IndexOutOfRangeException("Specified coordinates outside of pixel buffer bounds");

				return buffer[x][y];
			}

			set
			{
				if (x < 0 || y < 0 || x >= canvasWidth || y >= canvasHeight)
					throw new IndexOutOfRangeException("Specified coordinates outside of pixel buffer bounds");

				buffer[x][y] = value;

				if (sdlTexture == null)
					return;

				if (!sdlTexture.Locked)
					(pixelPtr, pitch) = sdlTexture.Lock();

				var pixelIndex = x * 4 + y * pitch;

				unsafe
				{
					var __unsafe__pixelPtr = new Span<byte>(pixelPtr.ToPointer(), pitch * canvasHeight);

					__unsafe__pixelPtr[pixelIndex] = value.A;
					__unsafe__pixelPtr[pixelIndex + 1] = value.B;
					__unsafe__pixelPtr[pixelIndex + 2] = value.G;
					__unsafe__pixelPtr[pixelIndex + 3] = value.R;
				}
			}
		}
	}
}
