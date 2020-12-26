using System.Drawing;

namespace MediaLib
{
	public partial class Window
	{
		public void FillRect(Color color, Rectangle rect) => SdlRenderer.FillRect(color, rect);

		public void DrawSprite(Sprite sprite, Rectangle pos)
		{
			var texture = sprite.GetTexture(SdlRenderer);

			SdlRenderer.Copy(texture, null, pos);
		}

		public void DrawSprite(Sprite sprite, int x, int y) => DrawSprite(sprite, new Rectangle(x, y, sprite.Width, sprite.Height));
	}
}
