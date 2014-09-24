using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class ScreenQuit : BaseScreen {

	override protected void OnGUIInner(){
		GuiHelper.DrawAtTop ("Quit");

		GuiHelper.DrawBeneathLine ("Are you sure you want to quit?");
		GuiHelper.YesButton (delegate() {
			Application.Quit();
		});
	}

}
