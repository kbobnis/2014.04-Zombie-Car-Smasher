using UnityEngine;
using System.Collections;

public class ScreenLoading : MonoBehaviour {

	private float EndTime;
	private float Dots ;
	private float DotsTime;
	public string Text = "Loading";

	void Update () {
		DotsTime += Time.deltaTime;
		if (DotsTime > 0.1f) {
			Dots = Round ( Dots - 0.1f, 2);
			if (Dots < 0){
				Dots = 0;
			}
			DotsTime -= 0.1f;
		}
	}

	void OnGUI(){
		GUI.depth = -1;
		GuiHelper.DrawElement ("Images/LoadingScreen", 0, 0, 1, 1);
		GuiHelper.DrawText (Text+"\n"+string.Format("{0:0.0}", Dots), GuiHelper.SmallFontBrown, 0, 0, 1, 1);
		float Now = System.DateTime.Now.Second;
		if (Now > EndTime) {
			Destroy (this);
		}
		GUI.depth = 0;
	}

	public void EndMe(){
		System.DateTime time = System.DateTime.Now;
		EndTime = time.Second;
		Dots = 4f;
	}

	public void EndMeIn(int seconds){
		System.DateTime time = System.DateTime.Now;
		EndTime = time.Second + seconds;
		Dots = seconds;
	}

	public static float Round(float value, int digits)
	{
		float mult = Mathf.Pow(10.0f, (float)digits);
		return Mathf.Round(value * mult) / mult;
	}
}
