using UnityEngine;
using System.Collections.Generic;

public class GuiHelper : MonoBehaviour {

	private static List<MyKeyValue> blinking = new List<MyKeyValue>();

	public static int oneThirdW = Screen.width/3;
	public static int oneThirdH = Screen.height/3;
	public static int oneTenthW = Screen.width/10;
	public static int oneTenthH = Screen.height/10;
	public static int twentyPercent = Screen.height / 5;

	public static GUIStyle SmallFont ;
	public static GUIStyle CustomButton ;
	public static GUIStyle SmallFontBrown = new GUIStyle ();
	public static GUIStyle MicroFont = new GUIStyle();

	private static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();


	void Update () {
		foreach(MyKeyValue one in blinking) {
			one.Value += Time.deltaTime;
		}
	}
	

	// Use this for initialization
	void OnGUI () {
		if (SmallFont == null) {
			SmallFont = new GUIStyle();
			SmallFont.fontSize = 37 * Screen.width / 480;
			SmallFont.font = (Font)Resources.Load ("Fluf");
			SmallFont.normal.textColor = new Color (255 / 255f, 255 / 255f, 255 / 255f);

			MicroFont.fontSize = 20 * Screen.width / 480;
			MicroFont.font = (Font)Resources.Load ("Fluf");
			MicroFont.normal.textColor = new Color (255 / 255f, 255 / 255f, 255 / 255f);


			CustomButton = new GUIStyle ("button");
			CustomButton.fontSize = 37 * Screen.width / 480;
			CustomButton.font = (Font)Resources.Load ("Fluf");

			SmallFontBrown.fontSize = 37 * Screen.width / 480;
			SmallFontBrown.font = (Font)Resources.Load ("Fluf");
			SmallFontBrown.normal.textColor = new Color (227 / 255f, 6 / 255f, 19 / 255f);
		}
	}

	public static void DrawElement(string slotName, double x, double y, double w, double h, double actualW=-1, double actualH=-1, bool downUp=false){
		int tmpX = PercentW(x);
		int tmpY = PercentH (y);
		int tmpW = PercentW (w);
		int tmpH = PercentH (h);
		
		int tmpActualW = actualW==-1?tmpW : PercentW (actualW);
		int tmpActualH = actualH==-1?tmpH : PercentH (actualH);
		
		int groupX = tmpX;
		int groupY = tmpY;
		int textX = 0;
		int textY = 0;
		if (downUp) {
			groupY = Mathf.RoundToInt( (float)(tmpY + tmpH - tmpActualH) );
			textY = Mathf.RoundToInt( -tmpH + tmpActualH);
		}

		Texture texture = null;
		if (Textures.ContainsKey (slotName)) {
			texture = Textures[slotName];
		} else {
			texture = Resources.Load<Texture>(slotName);
			Textures[slotName] = texture;
		}

		GUI.BeginGroup (new Rect (groupX, groupY, tmpActualW, tmpActualH));
		GUI.DrawTexture(new Rect(textX, textY , tmpW, tmpH), texture);
		GUI.EndGroup ();
	}

	public static void DrawElementBlink(string slotName, double x, double y, double w, double h, double actualW=-1, double actualH=-1, bool downUp=false){
		
		double blinkValue = 0;
		MyKeyValue one = null;
		foreach(MyKeyValue tmp in blinking) {
			if (slotName == tmp.Key){
				one = tmp;
				blinkValue = tmp.Value;
			}
		}
		
		if (blinkValue > 0) {
			DrawElement (slotName, x, y, w, h, actualW, actualH, downUp);
		}
		
		if (blinkValue > 0.25) {
			blinkValue = -0.25;
		}
		
		if (one == null) {
			one = new MyKeyValue();
			one.Key = slotName;
			blinking.Add (one);
		} 
		
		one.Value = blinkValue;
		
	}

	public static int PercentW(double x){
		return (int)(x * Screen.width);
	}

	public static int PercentH(double y){
		return (int)(y * Screen.height);
	}

	public static void DrawText(string text, GUIStyle font, double x, double y, double w=-1, double h=-1){
		int tmpX = PercentW(x);
		int tmpY = PercentH (y);
		int tmpW = w==-1?Screen.width:PercentW(w);
		int tmpH = w == -1 ? Screen.height : PercentH (h);
		GUI.Label(new Rect(tmpX, tmpY, tmpW, tmpH), text, font);
	}
}

public class MyKeyValue
{
	public string Key;
	public double Value;
	
}
