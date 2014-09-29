using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenAfterMinigameAdv : BaseScreen {

	private int Distance;
	private string GameOverReason;
	private Mission Mission;
	private Dictionary<int, Result[]> InGameResults;
	private Result[] AfterGameResults;
	private bool Passed;
	private int CoinsCollected;
	private int TaskDoneAmount;
	private int TaskToBeDoneAmount;
	private SCORE_TYPE TaskToBeDone;

	override protected void StartInner (){
		Prepare (delegate() {
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});
	}

	public static void PrepareScreen(Dictionary<int, Result[]> inGameResults, Result[] afterGameResults, string reason, int distance, Mission mission, PlayerState player){
		ScreenAfterMinigameAdv samc = Camera.main.gameObject.AddComponent<ScreenAfterMinigameAdv> ();

		samc.PrepareMe (inGameResults, afterGameResults, reason, distance, mission, player);
	}
	
	public void PrepareMe(Dictionary<int, Result[]> inGameResults, Result[] afterGameResults, string reason, int distance, Mission mission, PlayerState player){
		Distance = distance;
		GameOverReason = reason;
		Mission = mission;
		InGameResults = inGameResults;
		AfterGameResults = afterGameResults;

		if (Passed = Mission.Passed (InGameResults, AfterGameResults)) {
			player.MissionDone (Mission);
		}
		CarSmasherSocial.UpdateLeaderboard (GoogleLeaderboard.LEADERB_BEST_DISTANCES_ADV, afterGameResults);

		foreach(AchievQuery aq in Mission.AfterGameReqs){
			TaskToBeDone = aq.ScoreType;
			TaskToBeDoneAmount = aq.Value;
		}

		foreach(Result r in afterGameResults){
			if (r.ScoreType == SCORE_TYPE.COINS){
				CoinsCollected = r.Value;
			}
			if (r.ScoreType == TaskToBeDone){
				TaskDoneAmount = r.Value;
			}
		}

	}

	override protected void OnGUIInner(){

		GuiHelper.DrawBackground(delegate() {
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});
		GuiHelper.ButtonWithText(0.5, 0.8, 0.3, 0.3, "Continue", SpriteManager.GetRoundButton(), GuiHelper.SmallFont, delegate() {
			ScreenAdvModeStart sams = gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});


		GuiHelper.DrawAtTop ("Mission " + (Passed ? "Completed" : "Failed"));
		string text = "";

		if (Passed) {
			text += "Mission completed, reward:  " + Mission.Reward.Description+"\n";
		} else {
			text += "Mission failed\n";
		}
		text += "" + TaskDoneAmount + " / " + TaskToBeDoneAmount + " ( "+ Mission.Description + " )\n\n";

		text += "Coins collected: " + CoinsCollected + "\n\n";


		text += "Distance driven: " + Distance + "\n";
		text += "Best distance: " + HighScores.AddScore (Distance, HighScores.ADVENTURE);

		GuiHelper.DrawBeneathLine(text);


		Texture leaderBoard = SpriteManager.GetLeaderboard();
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.1), GuiHelper.PercentH(0.8), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), leaderBoard, GuiHelper.CustomButton)){
			CarSmasherSocial.ShowLeaderBoard(GoogleLeaderboard.LEADERB_BEST_DISTANCES_ADV);
		}
	}
}





















