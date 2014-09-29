using UnityEngine;
using System.Collections;

public class ScreenText : BaseScreen {

	private string Top, Middle;

	override protected void StartInner (){
	}

	public void Prepare (AfterButton afterButton, string top, string middle) {
		base.Prepare (afterButton, true);
		Top = top;
		Middle = middle;
	}

	override protected void OnGUIInner(){
		GuiHelper.DrawAtTop (Top);
		GuiHelper.DrawBeneathLine (Middle);
	}
}

