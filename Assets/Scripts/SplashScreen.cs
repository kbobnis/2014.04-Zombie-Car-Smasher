using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public bool showingSplash;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ShowSplash(){
		showingSplash = true;
	}

	void OnGUI(){
		if (showingSplash){
			DrawElement("Images/buffCar", 0.1, 0.1, 0.8, 0.8);

			if(GUI.Button(new Rect(PercentW(0.2), PercentH(0.7), PercentW(0.6), PercentH(0.2)), "Start game")){
				showingSplash = false;
				GetComponent<MainLogic>().WantToStartGame();
			}
		}
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
