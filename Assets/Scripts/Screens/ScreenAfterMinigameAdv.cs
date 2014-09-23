using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenAfterMinigameAdv : MonoBehaviour {

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

	void OnGUI(){

		GuiHelper.DrawBackground(delegate() {
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});
		GuiHelper.ButtonWithText(0.5, 0.8, 0.3, 0.3, "Continue", SpriteManager.GetRoundButton(), GuiHelper.SmallFont, delegate() {
			gameObject.AddComponent<ScreenAdvModeStart>();
			Destroy(this);
		});


		GuiHelper.DrawAtTop ("Mission " + (Passed ? "Completed" : "Failed"));
		string text = "Done " + TaskDoneAmount + "/" + TaskToBeDoneAmount + " of ("+ Mission.Description + ")\n";

		if (Passed) {
			text += "Reward:  " + Mission.Reward.Description+"\n\n";
		} else {
			text += "Try again and get the reward: " + Mission.Reward.Description+"\n\n";
		}
		text += "Coins collected: " + CoinsCollected + "\n";
		text += "Distance driven: " + Distance + "\n\n";
		GuiHelper.DrawBeneathLine(text);
	}
}





















