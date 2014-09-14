using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public bool showingSplash;
	public bool GooglePlayFinished;
	// Use this for initialization
	void Start () {
		FB.Init(delegate {});
		CarSmasherSocial.InitializeSocial(false, AfterGooglePlay, AfterGooglePlay);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ShowSplash(){
		showingSplash = true;
	}

	void OnGUI(){
		if (showingSplash){
			Texture texture = Resources.Load("Images/intro", typeof(Texture)) as Texture;
			float scale = (float)Screen.height / (float)texture.height;
			int height =  Mathf.RoundToInt( scale * texture.height);
			int width = Mathf.RoundToInt( scale * texture.width);
			int x = (Screen.width - width) / 2;

			GUI.DrawTexture(new Rect(x, 0 , width, height), texture);



			if (GooglePlayFinished){
				GUIStyle gs = new GUIStyle();//("button");
				gs.alignment = TextAnchor.MiddleCenter;
				if(GUI.Button(new Rect(PercentW(0.31), PercentH(0.7), PercentW(0.44), PercentH(0.25)), SpriteManager.GetStartButton(), gs)){
					showingSplash = false;
					GetComponent<MainLogic>().WantToStartGame();
				}
			} else {
				GuiHelper.DrawText("Authenticating\n google play. \nJust a second ", GuiHelper.SmallFont, 0, 0.7, 1, 0.4);
			}

			GoogleAnalyticsKProjekt.LogScreenOnce (Minigame.SCREEN_MAIN);

			GuiHelper.DrawText("K Bobnis: Design, Programming\nM Bartynski: Design, Concept", GuiHelper.MicroFont, 0.2, 0.05, 0.8, 0.2);

			Texture soundButton = Sounds.IsMuted()?SpriteManager.GetSoundButtonMuted():SpriteManager.GetSoundButton();
			if (GUI.Button(new Rect(GuiHelper.PercentW(0.75), GuiHelper.PercentH(0.66), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), soundButton, GuiHelper.CustomButton)){
				Sounds.Mute(!Sounds.IsMuted());
			}
		}
	}

	private void AfterGooglePlay(){
		GooglePlayFinished = true;
	}


	private void DrawElement(string slotName, double x, double y, double w, double h, double actualW=-1, double actualH=-1, bool downUp=false){
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
		
		GUI.BeginGroup (new Rect (groupX, groupY, tmpActualW, tmpActualH));
		GUI.DrawTexture(new Rect(textX, textY , tmpW, tmpH), Resources.Load(slotName, typeof(Texture))as Texture);
        GUI.EndGroup ();
	}

	private int PercentW(double x){
		return (int)(x * Screen.width);
	}
	
	private int PercentH(double y){
		return (int)(y * Screen.height);
    }
}
