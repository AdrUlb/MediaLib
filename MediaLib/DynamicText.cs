using ManagedSDL2;
using System;
using System.Drawing;

namespace MediaLib
{
	public class DynamicText : IText, IDisposable
	{
		Window window;
		Font font;
		string text;
		int size;
		Color color;
		uint wrapLength;

		public int Width => staticText.Width;
		public int Height => staticText.Width;

		public int Size
		{
			get => size;
			set
			{
				size = value;
				GenerateStaticText();
			}
		}

		public Color Color
		{
			get => color;
			set
			{
				color = value;
				GenerateStaticText();
			}
		}

		public uint WrapLength
		{
			get => wrapLength;
			set
			{
				wrapLength = value;
				GenerateStaticText();
			}
		}

		StaticText staticText;

		public DynamicText(Window window, Font font, string text, int size, Color color, uint wrapLength = 0)
		{
			this.window = window;
			this.font = font;
			this.text = text;
			this.size = size;
			this.color = color;
			this.wrapLength = wrapLength;
			staticText = null!;
			GenerateStaticText();
		}

		private void GenerateStaticText()
		{
			staticText?.Dispose();
			staticText = font.CreateStaticText(window, text, size, color, wrapLength);
		}

		public void Draw(Rectangle? rect = null)
		{
			staticText.Draw(rect);
		}

		public void Dispose()
		{
			staticText.Dispose();
		}
	}
}
