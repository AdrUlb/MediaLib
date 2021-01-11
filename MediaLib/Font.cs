using ManagedSDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MediaLib
{
	public class Font : IDisposable
	{
		string path;
		Dictionary<int, TTF.Font> ttfFonts = new Dictionary<int, TTF.Font>();

		public Font(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException("Could not find the specified font file.");

			this.path = path;
		}

		public StaticText CreateStaticText(Window window, string text, int size, Color color, bool preferSpeedOverQuality = false, uint wrapLength = 0)
		{
			if (!ttfFonts.ContainsKey(size))
			{
				var font = new TTF.Font(path, size);
				ttfFonts.Add(size, font);
			}

			return new StaticText(ttfFonts[size].RenderText(window.SdlRenderer, text, color, preferSpeedOverQuality, wrapLength), window.SdlRenderer);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			foreach (var font in ttfFonts.Values)
				font.Dispose();
		}
	}
}
