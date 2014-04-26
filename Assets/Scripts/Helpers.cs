using UnityEngine;
using System.Collections;

public class Helpers {
	public static void Render_Colored_Rectangle(int x, int y, int w, int h, float r, float g, float b, float a)
	{
		Texture2D rgb_texture = new Texture2D(w, h);
		Color rgb_color = new Color(r, g, b, a);
		int i, j;
		for(i = 0;i<w;i++)
		{
			for(j = 0;j<h;j++)
			{
				rgb_texture.SetPixel(i, j, rgb_color);
			}
		}
		rgb_texture.Apply();
		GUIStyle generic_style = new GUIStyle();
		GUI.skin.box = generic_style;
		GUI.Box (new Rect (x,y,w,h), rgb_texture);
	}

	public static void DrawRectangle (Rect position, Color color)
	{    
		
		// Optimization hint: 
		// Consider Graphics.DrawMeshNow
		GL.Color (color);
		GL.Begin (GL.QUADS);
		GL.Vertex3 (position.x, position.y, 0);
		GL.Vertex3 (position.x + position.width, position.y, 0);
		GL.Vertex3 (position.x + position.width, position.y + position.height, 0);
		GL.Vertex3 (position.x, position.y + position.height, 0);
		GL.End ();
	}

	public static void DebugDraw (Vector3 center, Vector3 size, Color color)
	{
		var hw = size.x / 2.0f;
		var hh = size.y / 2.0f;

		var p1 = new Vector3 (center.x - hw, center.y - hh, 0.0f);
		var p2 = new Vector3 (center.x + hw, center.y - hh, 0.0f);
		var p3 = new Vector3 (center.x - hw, center.y + hh, 0.0f);
		var p4 = new Vector3 (center.x + hw, center.y + hh, 0.0f);

//		Debug.DrawLine (p1, p2, color);
//		Debug.DrawLine (rectangle.p2, rectangle.p3, color);
//		Debug.DrawLine (rectangle.p3, rectangle.p4, color);
//		Debug.DrawLine (rectangle.p4, rectangle.p1, color);
//		Debug.DrawLine (rectangle.p5, rectangle.p6, color);
//		Debug.DrawLine (rectangle.p6, rectangle.p7, color);
//		Debug.DrawLine (rectangle.p7, rectangle.p8, color);
//		Debug.DrawLine (rectangle.p8, rectangle.p5, color);         
//		Debug.DrawLine (rectangle.p1, rectangle.p5, color);
//		Debug.DrawLine (rectangle.p2, rectangle.p6, color);
//		Debug.DrawLine (rectangle.p3, rectangle.p7, color);
//		Debug.DrawLine (rectangle.p4, rectangle.p8, color);
	}
}
