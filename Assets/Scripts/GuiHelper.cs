using UnityEngine;
using System.Collections.Generic;

public delegate void AfterButton();

public class GuiHelper : MonoBehaviour {

	private static List<MyKeyValue> blinking = new List<MyKeyValue>();
	private static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

	public static GUIStyle SmallFont ;
	public static GUIStyle SmallFontLeft;
	public static GUIStyle SmallFontTop;
	public static GUIStyle SmallFontBlack;
	public static GUIStyle CustomButton ;
	public static GUIStyle SmallFontBrown = new GUIStyle ();
	public static GUIStyle MicroFontBrown;
	public static GUIStyle MicroFont;
	public static GUIStyle MicroFontLeft;
	public static GUIStyle MicroFontTop;

	void Update () {
		foreach(MyKeyValue one in blinking) {
			one.Value += Time.deltaTime;
		}
	}

	void OnGUI(){
		Load ();
	}

	// Use this for initialization
	private void Load () {
		if (SmallFont == null) {

			int normalSize = 35;
			SmallFont = new GUIStyle();
			SmallFont.fontSize = normalSize * Screen.width / 480;
			SmallFont.font = (Font)Resources.Load ("Fluf");
			SmallFont.normal.textColor = new Color (255 / 255f, 255 / 255f, 255 / 255f);
			SmallFont.alignment = TextAnchor.MiddleCenter;
			SmallFont.wordWrap = true;

			SmallFontBlack = new GUIStyle();
			SmallFontBlack.fontSize = normalSize * Screen.width / 480;
			SmallFontBlack.font = (Font)Resources.Load ("Fluf");
			SmallFontBlack.normal.textColor = new Color (0 / 255f, 0 / 255f, 0 / 255f);
			SmallFontBlack.alignment = TextAnchor.MiddleCenter;
			SmallFontBlack.wordWrap = true;

			SmallFontTop = new GUIStyle();
			SmallFontTop.fontSize = normalSize * Screen.width / 480;
			SmallFontTop.font = (Font)Resources.Load ("Fluf");
			SmallFontTop.normal.textColor = new Color (255 / 255f, 255 / 255f, 255 / 255f);
			SmallFontTop.alignment = TextAnchor.UpperCenter;
			SmallFontTop.wordWrap = true;

			SmallFontLeft = new GUIStyle();
			SmallFontLeft.fontSize = normalSize * Screen.width / 480;
			SmallFontLeft.font = (Font)Resources.Load ("Fluf");
			SmallFontLeft.normal.textColor = new Color (255 / 255f, 255 / 255f, 255 / 255f);

			MicroFont = new GUIStyle();//("button");
			MicroFont.fontSize = 25 * Screen.width / 480;
			MicroFont.font = (Font)Resources.Load ("Fluf");
			MicroFont.normal.textColor = new Color (255 / 255f, 255 / 255f, 255 / 255f);
			MicroFont.alignment = TextAnchor.MiddleCenter;
			MicroFont.wordWrap = true;

			MicroFontLeft = new GUIStyle();//("button");
			MicroFontLeft.fontSize = 25 * Screen.width / 480;
			MicroFontLeft.font = (Font)Resources.Load ("Fluf");
			MicroFontLeft.normal.textColor = new Color (255 / 255f, 255 / 255f, 255 / 255f);
			MicroFontLeft.alignment = TextAnchor.UpperLeft;

			MicroFontTop = new GUIStyle();//("button");
			MicroFontTop.fontSize = 25 * Screen.width / 480;
			MicroFontTop.font = (Font)Resources.Load ("Fluf");
			MicroFontTop.normal.textColor = new Color (255 / 255f, 255 / 255f, 255 / 255f);
			MicroFontTop.alignment = TextAnchor.UpperCenter;
			MicroFontTop.wordWrap = true;


			CustomButton = new GUIStyle ();//("button");
			CustomButton.fontSize = 30 * Screen.width / 480;
			CustomButton.font = (Font)Resources.Load ("Fluf");
			CustomButton.alignment = TextAnchor.MiddleCenter;

			SmallFontBrown.fontSize = normalSize * Screen.width / 480;
			SmallFontBrown.font = (Font)Resources.Load ("Fluf");
			SmallFontBrown.normal.textColor = new Color (41 / 255f, 41 / 255f, 41 / 255f);
			SmallFontBrown.alignment = TextAnchor.MiddleCenter;
			SmallFontBrown.wordWrap = true;

			MicroFontBrown = new GUIStyle();//("button");
			MicroFontBrown.fontSize = 20 * Screen.width / 480;
			MicroFontBrown.font = (Font)Resources.Load ("Fluf");
			MicroFontBrown.normal.textColor = new Color (41 / 255f, 41 / 255f, 41 / 255f);
			MicroFontBrown.alignment = TextAnchor.MiddleCenter;
			MicroFontBrown.wordWrap = true;
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

	public static void DrawButton(string text, GUIStyle font, double x, double y, double w, double h, AfterButton afterButton){
		int tmpX = PercentW(x);
		int tmpY = PercentH (y);
		int tmpW = w==-1?Screen.width:PercentW(w);
		int tmpH = w == -1 ? Screen.height : PercentH (h);
		if (GUI.Button (new Rect (tmpX, tmpY, tmpW, tmpH), text, font)) {
			afterButton();
		}
	}

	public static void ButtonWithText(double x, double y, double w, double h,string text, Texture background, GUIStyle font, AfterButton afterButton){
		int _x = PercentW (x);
		int _y = PercentH (y);
		int _w = PercentW (w);
		int _h = PercentW (h);

		if (background != null) {
			if (GUI.Button (new Rect (_x - _w / 2, _y - _h / 2, _w, _h), background, font)) {
				afterButton ();
			}
			GUI.Label (new Rect (_x - _w / 2, _y - _h / 2, _w, _h), text, font);
		} else {
			if (GUI.Button (new Rect (_x - _w / 2, _y - _h / 2, _w, _h), text, font)) {
				afterButton ();
			}
		}
	}

	public static void DrawBackground(AfterButton afterButton, bool showSettings=false){
		GuiHelper.DrawElement("Images/popupWindow", 0.02, 0.1, 1, 0.94);
		GuiHelper.ButtonWithText (0.9, 0.92, 0.4, 0.2, "", SpriteManager.GetBackButton (), GuiHelper.CustomButton, afterButton);

		if (showSettings && GUI.Button(new Rect(GuiHelper.PercentW(0.8), GuiHelper.PercentH(0.70), GuiHelper.PercentW(0.2), GuiHelper.PercentH(0.2)), SpriteManager.GetSettingsIcon(), GuiHelper.CustomButton)){
			ScreenOptions so = Camera.main.gameObject.AddComponent<ScreenOptions>();
			so.Prepare(delegate(){
				Destroy(so);
			});
		}
	}

	public static void DrawAtTop(string text){
		GuiHelper.DrawText (text, GuiHelper.SmallFont, 0.1, 0.15, 0.8, 0.07);
	}
	public static void DrawBeneathLine(string text){
		GuiHelper.DrawText (text, GuiHelper.MicroFontTop, 0.1, 0.28, 0.75, 0.07);
	}
	public static void DrawMissionLabel(int slot, string name, string detail, string reward, AfterButton afterButton){
		double y = 0.1 + slot * 0.15;
		GuiHelper.DrawText (name, MicroFontLeft, 0.1, y, 0.3, 0.15); 
		GuiHelper.DrawText (detail, MicroFontLeft, 0.37, y-0.02, 0.5, 0.15);
		GuiHelper.DrawText (reward, MicroFontLeft, 0.37, y+0.02, 0.5, 0.15);
		GuiHelper.ButtonWithText(0.82, y + 0.02, 0.15, 0.15, "Race", SpriteManager.GetRoundButton(), SmallFont, afterButton);
	}

	public static void YesButton(AfterYesM afterYes, string text="Yes"){
		GuiHelper.ButtonWithText(0.5, 0.89, 0.4, 0.3, text, SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate() {
			afterYes();
		});
	}

}

public class MyKeyValue
{
	public string Key;
	public double Value;
	
}
