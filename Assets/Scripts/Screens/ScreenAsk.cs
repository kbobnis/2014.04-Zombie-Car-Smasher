using UnityEngine;
using System.Collections;

public delegate void AfterYesM();

public class ScreenAsk : MonoBehaviour {

	private AfterYesM AfterYes;
	private AfterYesM AfterNo;

	public void Prepare(AfterYesM afterYes, AfterYesM afterNo){
		AfterYes = afterYes;
		AfterNo = afterNo;
	}

	void OnGUI(){

		GuiHelper.DrawBackground (delegate {
			AfterNo();
		});

		GuiHelper.DrawAtTop ("Google games");
		GuiHelper.DrawBeneathLine ("There are leaderboards and achievements from google games in this game. The game would be a lot more fun if connected. Would you like to connect?");
		GuiHelper.YesButton (AfterYes);
	}

}
