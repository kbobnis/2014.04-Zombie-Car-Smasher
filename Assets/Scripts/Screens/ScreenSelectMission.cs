using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenSelectMission : BaseScreen {

	override protected void StartInner (){
		Prepare(delegate() {
			gameObject.AddComponent<ScreenAdvModeStart> ();
			Destroy (this);
		});
	}

	override protected void OnGUIInner(){
		
		PlayerState player = Game.Me.Player;

		GuiHelper.DrawAtTop ("Select mission");


	}
}
