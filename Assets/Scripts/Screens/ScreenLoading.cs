using UnityEngine;
using System.Collections;

public class ScreenLoading : BaseScreen {

	private float EndTime;
	private float Dots ;
	private float DotsTime;
	public string Text = "Loading";
	public bool ShowButton ;

	override protected void StartInner (){
	}

	override protected void UpdateInner () {
		DotsTime += Time.deltaTime;
		if (DotsTime > 0.1f) {
			Dots = Dots - 0.1f;
			if (Dots < 0){
				Dots = 0;
			}
			DotsTime -= 0.1f;
		}
		float Now = System.DateTime.Now.Second;
		if (Now > EndTime) {
			Destroy (this);
		}
	}

	override protected void OnGUIInner(){
		GuiHelper.DrawElement ("Images/LoadingScreen", 0, 0, 1, 1);
		GuiHelper.DrawText (Text+"\n"+string.Format("{0:0.0}", Dots), GuiHelper.MicroFontBrown, 0, 0, 1, 1);
		GuiHelper.YesButton (delegate() {
			Destroy(this);
		}, "Continue");
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

	public void EndMeWithButton(){
		EndTime = System.DateTime.Now.Second + 999f;
		ShowButton = true;
	}

}
