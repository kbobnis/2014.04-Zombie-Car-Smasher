using UnityEngine;
using System.Collections;

public delegate void AfterYesM();

public class ScreenAsk : BaseScreen {

	private AfterYesM AfterYes;

	override protected void StartInner (){
	}

	public void PrepareMe(AfterYesM afterYes, AfterButton afterNo){
		AfterYes = afterYes;
		Prepare (afterNo);
	}

	override protected void OnGUIInner(){

		GuiHelper.DrawAtTop ("Google games");
		GuiHelper.DrawBeneathLine ("There are leaderboards and achievements from google games in this game. The game would be a lot more fun if connected. Would you like to connect?");
		GuiHelper.YesButton (AfterYes);
	}

}
