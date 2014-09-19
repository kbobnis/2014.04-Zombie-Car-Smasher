using UnityEngine;
using System.Collections;

public class TopInfoBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		PlayerState playerState = Game.Me.Player;

		GuiHelper.DrawElement ("Images/topBarBackground", 0, 0, 1, 0.1);

		GuiHelper.DrawText ("Level: "+playerState.Level+" Coins: "+playerState.Coins, GuiHelper.SmallFontLeft, 0.2, 0.02, 0.3, 0.1);
	}
}
