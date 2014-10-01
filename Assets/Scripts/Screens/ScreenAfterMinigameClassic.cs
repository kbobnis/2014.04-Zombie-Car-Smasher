using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenAfterMinigameClassic : BaseScreen {

	private int Distance;
	private int HowManyInTopScores = 4;
	private string GameOverReason;
	private bool ShowNewHighScoreScreen = false;
	private bool ShowRideInfoScreen = false;
	private int FuelPickedUp, FuelPickedUpInARow, FuelPickedUpWhenLow, Turns, CoinsPickedUp;
	private Mission Mission;
	private PlayerState Player;

	override protected void StartInner (){
		Prepare (delegate() {
			gameObject.AddComponent<ScreenSplash> ();
			Destroy (this);
		}, false);
	}

	public static void PrepareScreen(Dictionary<int, Result[]> inGameResults, Result[] afterGameResults, string reason, int distance, Mission mission, PlayerState player){
		ScreenAfterMinigameClassic samc = Camera.main.gameObject.AddComponent<ScreenAfterMinigameClassic> ();
		samc.PrepareMe (inGameResults, afterGameResults, reason, distance, mission, player);
	}

	public void PrepareMe(Dictionary<int, Result[]> inGameResults, Result[] afterGameResults, string reason, int distance, Mission mission, PlayerState player){
		Distance = distance;
		GameOverReason = reason;
		Mission = mission;
		Player = player;
		HighScores.AddScore (distance, HighScoreType.Classic);
		int place = HighScores.GetPlaceFor (distance, HighScoreType.Classic);
		if (place == 1) {
			PlaySingleSound.SpawnSound(Sounds.Fanfare, Camera.main.gameObject.transform.position, 0.2f);
			if (distance > 50){
				ShowNewHighScoreScreen = true;
			}
		}

		foreach (Result result in afterGameResults) {
			switch(result.ScoreType){
				case SCORE_TYPE.FUEL_PICKED:
					FuelPickedUp = result.Value;
					break;
				case SCORE_TYPE.FUEL_PICKED_IN_ROW:
					FuelPickedUpInARow = result.Value;
					break;
				case SCORE_TYPE.FUEL_PICKED_WHEN_LOW:
					FuelPickedUpWhenLow = result.Value;
					break;
				case SCORE_TYPE.TURNS:
					Turns = result.Value;
					break;
				case SCORE_TYPE.COINS:
				CoinsPickedUp = result.Value;
					break;
			}
		}
		//if someone had internet issues before, at least we can update high score with his best distance and come unlockable achievements
		//we don't want this. this is sending best score for every day player is playing
		//List<int> topScores = HighScores.GetTopScores (1);
		//int bestDistance = topScores.Count > 0 ? topScores [0] : 0;
		//InGameAchievements.Add(bestDistance, new Result[]{new Result (SCORE_TYPE.DISTANCE, bestDistance)});
	}

	override protected void OnGUIInner(){
			
		if (ShowNewHighScoreScreen){
			
			GuiHelper.DrawBackground(delegate() {
				ShowNewHighScoreScreen = false;
			});
			
			GuiHelper.DrawAtTop("New High Score!");
			GuiHelper.DrawBeneathLine("You just beat your high score with distance "+Distance+". \n\n Like to tell your friends about it?");

			if (GUI.Button(new Rect(GuiHelper.PercentW(0.2), GuiHelper.PercentH(0.70), GuiHelper.PercentW(0.6), GuiHelper.PercentH(0.11)), SpriteManager.GetFbShareButton(), GuiHelper.CustomButton)){
				CarSmasherSocial.FB.FeedHighScore(Distance);
				ShowNewHighScoreScreen = false;
			}
			
		} else if (ShowRideInfoScreen){
			GuiHelper.DrawBackground(delegate() {
				ShowRideInfoScreen = false;
			});

			GuiHelper.DrawAtTop(GameOverReason);
			GuiHelper.DrawBeneathLine(
				"Collect oil drops to replenish fuel tank. Avoid obstacles.\n"+
            	"\nDistance made: "+Distance+
	            "\nTurns made: "+Turns+
	            "\nFuel picked up: "+FuelPickedUp+" "+
	            "(in a row: "+FuelPickedUpInARow+") "+
	            "(when low: "+FuelPickedUpWhenLow+") ");
		} else {

			GuiHelper.DrawBackground(delegate() {
				gameObject.AddComponent<ScreenSplash>();
				Destroy(this);
			});
			GuiHelper.DrawAtTop(GameOverReason);


			if (GUI.Button(new Rect(GuiHelper.PercentW(0.8), GuiHelper.PercentH(0.13), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.1)), "help", GuiHelper.SmallFont)){
				ShowRideInfoScreen = true;
			}
			DrawTopScores(0.3f);
			
			Texture achievements = SpriteManager.GetAchievements();
			if (GUI.Button(new Rect(GuiHelper.PercentW(0.07), GuiHelper.PercentH(0.65), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), achievements, GuiHelper.CustomButton)){
				CarSmasherSocial.ShowAchievements();
			}
			
			Texture leaderBoard = SpriteManager.GetLeaderboard();
			if (GUI.Button(new Rect(GuiHelper.PercentW(0.28), GuiHelper.PercentH(0.61), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), leaderBoard, GuiHelper.CustomButton)){
				CarSmasherSocial.ShowLeaderBoard(GoogleLeaderboard.LEADERB_BEST_DISTANCES);
			}

			GuiHelper.YesButton(delegate(){
				Minigame m = gameObject.AddComponent<Minigame>();
				m.PrepareRace(Game.Me.Player, ScreenAfterMinigameClassic.PrepareScreen, Mission.Classic, Game.Me.ClassicCarConfig);
				Destroy(this);
			}, "Start");
			
			GuiHelper.ButtonWithText(0.9, 0.92, 0.4, 0.2, "", SpriteManager.GetBackButton(), GuiHelper.CustomButton, delegate() {
				BackToSplash();
			});
		}

	}

	private void BackToSplash(){
		ScreenSplash ss = gameObject.AddComponent<ScreenSplash> ();
		Destroy (this);
	}

	private void DrawTopScores(float y){
		int place = HighScores.GetPlaceFor (Distance, HighScoreType.Classic);
		bool isInTop = place <= HowManyInTopScores;
		List<int> top = HighScores.GetTopScores (HowManyInTopScores, HighScoreType.Classic);
		bool yourWasSet = false;
		string topScores = "Best Distances:";
		for(int i=0; i < HowManyInTopScores && top.Count > i; i++){
			
			if (i < HowManyInTopScores){
				topScores += "\n";
			}
			if (!yourWasSet && i == HowManyInTopScores-1 && !isInTop){
				break;
			}
			
			int s = top[i];
			if (s == Distance && !yourWasSet){
				yourWasSet = true;
				topScores += "Top " + (i+1) + ". " + s + " (Now)";
			} else {
				topScores += "Top " + (i+1) + ". " + s + "";
			}
		}
		
		if (!yourWasSet){
			topScores += "... Now: " + Distance ;
		}
		
		GuiHelper.DrawText (topScores, GuiHelper.SmallFontLeft, 0.1, y, 0.8, 0.41);
		
		Texture fbButton = SpriteManager.GetFbShareButton();
		int bestScore = top.Count>0?top[0]:0;
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.545), GuiHelper.PercentH(y), GuiHelper.PercentW(0.35), GuiHelper.PercentH(0.18)), fbButton, GuiHelper.CustomButton)){
			CarSmasherSocial.FB.FeedHighScore(bestScore);
		}
	}
}
