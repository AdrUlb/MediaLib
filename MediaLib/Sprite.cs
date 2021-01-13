using ImageLib;
using System;
using ManagedSDL2;
using System.Collections.Generic;

namespace MediaLib
{
	public class Sprite : IDisposable
	{
		readonly Dictionary<SDL.Renderer, SDL.Texture> textures = new Dictionary<SDL.Renderer, SDL.Texture>();

		readonly Image image;

		public int Width => image.Width;
		public int Height => image.Height;

		public Sprite(string path)
		{
			image = new Image(path);
		}

		private SDL.Texture CreateTexture(SDL.Renderer renderer)
		{
			var texture = new SDL.Texture(renderer, SDL.PixelFormat.RGBA8888, SDL.TextureAccess.Streaming, image.Width, image.Height);

			(var pixelPtr, var pitch) = texture.Lock();

			unsafe
			{
				var __unsafe__pixelPtr = new Span<byte>(pixelPtr.ToPointer(), pitch * Height);

				for (var y = 0; y < Height; y++)
				{
					for (var x = 0; x < Width; x++)
					{
						var pixelIndex = x * 4 + y * pitch;
						var pixel = image[x, y];

						__unsafe__pixelPtr[pixelIndex] = pixel.A;
						__unsafe__pixelPtr[pixelIndex + 1] = pixel.B;
						__unsafe__pixelPtr[pixelIndex + 2] = pixel.G;
						__unsafe__pixelPtr[pixelIndex + 3] = pixel.R;
					}
				}
			}

			texture.Unlock();

			return texture;
		}

		internal SDL.Texture GetTexture(SDL.Renderer renderer)
		{
			if (!textures.ContainsKey(renderer))
				textures.Add(renderer, CreateTexture(renderer));

			return textures[renderer];
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			foreach (var texture in textures.Values)
				texture.Dispose();
		}
	}
}
