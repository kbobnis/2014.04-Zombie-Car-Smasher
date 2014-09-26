using UnityEngine;
using System.Collections;

public class ScreenStartingMission : BaseScreen {

	public Mission Mission;

	override protected void StartInner (){
		Prepare(delegate() {
			gameObject.AddComponent<ScreenAdvModeStart> ();
			Destroy (this);
		});
		Mission = Mission.SearchMissionForPlayer (Game.Me.Player);
	}

	override protected void OnGUIInner(){

		GuiHelper.DrawAtTop ("Preparing race");
		GuiHelper.DrawBeneathLine ("Mission: " + Mission.Description + ", Reward: " + Mission.Reward.Description);

		GuiHelper.ButtonWithText(0.5, 0.85, 0.3, 0.3, "Start", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			Minigame mi = gameObject.AddComponent<Minigame>();
			Destroy(this);
			mi.PrepareRace(Game.Me.Player, ScreenAfterMinigameAdv.PrepareScreen, Mission, Game.Me.Player.CarConfig);
		});

	}
}
