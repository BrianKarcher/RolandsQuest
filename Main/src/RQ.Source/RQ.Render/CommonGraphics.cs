using UnityEngine;

namespace RQ
{
	public class CommonGraphics
	{
		public CommonGraphics ()
		{
		}

		public static Texture2D CreateSingleColorTexture(Color color)
		{
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, color);
			texture.Apply ();
			return texture;
		}
	}
}