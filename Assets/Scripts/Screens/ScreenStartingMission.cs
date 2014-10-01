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
		string text = "Mission: " + Mission.Description + ", " +
			"Reward: " + Mission.Reward.Description + 
			"\n\n Your best distance: " + HighScores.GetTopScore(HighScoreType.Adventure);

		GuiHelper.DrawBeneathLine (text);

		Texture leaderBoard = SpriteManager.GetLeaderboard();
		GuiHelper.ButtonWithText(0.5, 0.55, 0.2, 0.2, "Scores", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			CarSmasherSocial.ShowLeaderBoard(GoogleLeaderboard.LEADERB_BEST_DISTANCES_ADV);
		});

		GuiHelper.YesButton(delegate(){
			Minigame mi = gameObject.AddComponent<Minigame>();
			Destroy(this);
			mi.PrepareRace(Game.Me.Player, ScreenAfterMinigameAdv.PrepareScreen, Mission, Game.Me.Player.CarConfig);
		}, "Start");

	}
}
