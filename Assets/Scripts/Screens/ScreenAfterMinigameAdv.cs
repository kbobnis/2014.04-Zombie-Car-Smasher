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

	public static void PrepareScreen(Dictionary<int, Result[]> inGameResults, Result[] afterGameResults, string reason, int distance, Mission mission){
		ScreenAfterMinigameAdv samc = Camera.main.gameObject.AddComponent<ScreenAfterMinigameAdv> ();
		samc.PrepareMe (inGameResults, afterGameResults, reason, distance, mission);
	}
	
	public void PrepareMe(Dictionary<int, Result[]> inGameResults, Result[] afterGameResults, string reason, int distance, Mission mission){
		Distance = distance;
		GameOverReason = reason;
		Mission = mission;
		InGameResults = inGameResults;
		AfterGameResults = afterGameResults;
		Passed = Mission.Passed(InGameResults, AfterGameResults);
		if (Passed) {
			mission.Reward.GiveItselfToPlayer(Game.Me.PlayerState);

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

		GuiHelper.DrawAtTop (Passed ? "Congratulations!" : "You have failed");
		string text = "";
		if (Passed) {
			text = "Congratulations, you have passed the mission. You are rewarded with " + Mission.Reward.Description;
		} else {
			text = "You can try again this mission and get the reward: " + Mission.Reward.Description;
		}
		GuiHelper.DrawBeneathLine(text);
	}
}





















