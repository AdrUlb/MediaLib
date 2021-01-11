using ManagedSDL2;
using System.Drawing;

namespace MediaLib
{
	interface IText
	{
		public int Width { get; }
		public int Height { get; }

		public void Draw(Rectangle? rect = null);
	}
}
