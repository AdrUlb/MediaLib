using System.Drawing;

namespace MediaLib
{
	public partial class Window
	{
		public void FillRect(Color color, Rectangle rect) => SdlRenderer.FillRect(color, rect);
	}
}
