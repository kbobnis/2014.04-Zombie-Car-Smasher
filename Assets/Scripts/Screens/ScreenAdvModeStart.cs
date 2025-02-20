using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenAdvModeStart : BaseScreen {

	override protected void StartInner (){
		 Prepare(delegate() {
			gameObject.AddComponent<ScreenSplash>();
			Destroy(this);
		});
	}


	override protected void OnGUIInner(){

		PlayerState state = Game.Me.Player;

		GuiHelper.DrawAtTop ("Adventure mode");
		GuiHelper.DrawBeneathLine ("Available coins: " + state.Coins);//, GuiHelper.SmallFontTop, 0.1, 0.2, 0.8, 0.1);

		float y = 0.38f;
		foreach(KeyValuePair<CarStatisticType, CarStatistic> kvp in state.CarConfig.CarStatistics){
			CarStatistic cs = kvp.Value;
			if (cs.IsUnlockedFor(state)){

				AfterButton upgrade = delegate() {
					ScreenUpgrade su = gameObject.AddComponent<ScreenUpgrade>();
					su.PrepareWith(cs);
					Destroy(this);
				};

				string inBrackets = "";
				if (!cs.Type.AboveMinimum(cs.Type.ValueForLevel( cs.Level+1))){
					inBrackets = "(Best)";
				} else if (cs.CanUpgrade(state.Coins)){
					inBrackets = "(Upg for "+cs.UpgradeCost()+")";
				}
				string text = cs.Type.Name()+": "+ cs.ValueFormatted + " " + inBrackets;
				GuiHelper.ButtonWithText(0.5, y, 1, 0.15, text, SpriteManager.GetRectangleButton(), GuiHelper.MicroFont, upgrade);


				//this has to be drawn after button with text because of overlay
				if (cs.CanUpgrade(state.Coins)){
					GuiHelper.ButtonWithText(0.9, y, 0.15, 0.15, "", SpriteManager.GetUpArrow(), GuiHelper.MicroFont, upgrade);
				}
				y += 0.093f;
			}
		}


		GuiHelper.YesButton(delegate() {
			ScreenStartingMission ssm = gameObject.AddComponent<ScreenStartingMission>();
			Destroy(this);
		}, "Race");

	}
}
