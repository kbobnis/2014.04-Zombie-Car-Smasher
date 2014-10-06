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
		HighScores.AddScore (Distance, HighScoreType.Adventure);

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

		GuiHelper.YesButton(delegate() {
			ScreenStartingMission ssm = gameObject.AddComponent<ScreenStartingMission>();
			Destroy(this);
		}, "Race");


		GuiHelper.DrawAtTop ("Mission "+ (Passed ? "Completed" : "Failed") + " (" + Mission.Description +")");
		string text = "";

		int sumOfCoins = CoinsCollected + (Passed?Mission.Reward.Coins:0);
		text += "Coins: +" +  sumOfCoins + " (reward: " + (Passed?Mission.Reward.Coins:0) + ", collected: " + CoinsCollected + ") \n\n";


		text += "Distance: "+ Distance;
		int best = HighScores.GetTopScore (HighScoreType.Adventure);
		if (Distance == best){
			text += " New Record!!!";
		} else {
			text += ", best: " + best +"";
		}
		text += "\n";


		GuiHelper.DrawBeneathLine(text);


		Texture leaderBoard = SpriteManager.GetLeaderboard();
		GuiHelper.ButtonWithText(0.5, 0.53, 0.2, 0.2, "Scores", SpriteManager.GetRoundButton(), GuiHelper.MicroFont, delegate(){
			CarSmasherSocial.ShowLeaderBoard(GoogleLeaderboard.LEADERB_BEST_DISTANCES_ADV);
		});
	}
}





















