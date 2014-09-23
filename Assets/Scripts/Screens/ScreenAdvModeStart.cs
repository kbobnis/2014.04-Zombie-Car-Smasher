using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenAdvModeStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		PlayerState state = Game.Me.Player;

		GuiHelper.DrawBackground (delegate() {
			gameObject.AddComponent<ScreenSplash>();
			Destroy(this);
		});

		GuiHelper.DrawAtTop ("Adventure mode");

		GuiHelper.DrawText ("Available coins: " + state.Coins, GuiHelper.SmallFontTop, 0.1, 0.2, 0.8, 0.1);
		GuiHelper.DrawText ("Earn coins, upgrade car, complete all missions.", GuiHelper.SmallFontTop, 0.1, 0.27, 0.8, 0.1);


		GUI.DrawTexture (new Rect (GuiHelper.PercentW(0.6), GuiHelper.PercentH (0.45), GuiHelper.PercentW (0.3), GuiHelper.PercentH (0.25)), state.CarConfig.CarTexture);

		float y = 0.44f;
		foreach(KeyValuePair<CarStatisticType, CarStatistic> kvp in state.CarConfig.CarStatistics){
			CarStatistic cs = kvp.Value;
			string inBrackets = "";
			if (!cs.Type.AboveMinimum(cs.Type.ValueForLevel( cs.Level+1))){
				inBrackets = "(Best)";
			} else if (cs.CanUpgrade(state.Coins)){
				inBrackets = "(Upgrade for "+cs.UpgradeCost()+")";
			}
			GuiHelper.DrawButton (cs.Type.Name()+": "+ string.Format("{0:0.00}", cs.Value) + " " + inBrackets, GuiHelper.MicroFontLeft, 0.1, y, 0.6, 0.04, delegate() {
				ScreenUpgrade su = gameObject.AddComponent<ScreenUpgrade>();
				su.PrepareWith(cs);
				Destroy(this);
			});
			y += 0.06f;
		}

		GuiHelper.ButtonWithText(0.45, 0.85, 0.3, 0.3, "Select mission", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate() {
			ScreenSelectMission ssm = gameObject.AddComponent<ScreenSelectMission>();
			Destroy(this);
		});
	}
}
