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
		string text = "Mission: " + Mission.Description + ", Reward: " + Mission.Reward.Description + 
			"\n\n Your actual best distance: " + HighScores.TopScore(HighScoreType.Adventure);
		GuiHelper.DrawBeneathLine (text);

		
		Texture leaderBoard = SpriteManager.GetLeaderboard();
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.76), GuiHelper.PercentH(0.34), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), leaderBoard, GuiHelper.CustomButton)){
			CarSmasherSocial.ShowLeaderBoard(GoogleLeaderboard.LEADERB_BEST_DISTANCES_ADV);
		}

		GuiHelper.ButtonWithText(0.5, 0.85, 0.3, 0.3, "Start", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			Minigame mi = gameObject.AddComponent<Minigame>();
			Destroy(this);
			mi.PrepareRace(Game.Me.Player, ScreenAfterMinigameAdv.PrepareScreen, Mission, Game.Me.Player.CarConfig);
		});

	}
}
