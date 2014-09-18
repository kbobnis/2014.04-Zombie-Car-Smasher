﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenAfterMinigameClassic : MonoBehaviour {

	private int Distance;
	private int HowManyInTopScores = 4;
	private string GameOverReason;
	private bool ShowNewHighScoreScreen = false;
	private bool ShowRideInfoScreen = false;
	private int FuelPickedUp, FuelPickedUpInARow, FuelPickedUpWhenLow, Turns;
	private Mission Mission;


	public static void PrepareScreen(Dictionary<int, Result[]> inGameResults, Result[] afterGameResults, string reason, int distance, Mission mission){
		ScreenAfterMinigameClassic samc = Camera.main.gameObject.AddComponent<ScreenAfterMinigameClassic> ();
		samc.PrepareMe (inGameResults, afterGameResults, reason, distance, mission);
	}

	public void PrepareMe(Dictionary<int, Result[]> inGameResults, Result[] afterGameResults, string reason, int distance, Mission mission){
		Distance = distance;
		GameOverReason = reason;
		Mission = mission;
		HighScores.AddScore (distance);
		int place = HighScores.GetPlaceFor (distance);
		if (place == 1) {
			PlaySingleSound.SpawnSound(Sounds.Fanfare, Camera.main.gameObject.transform.position, 0.2f);
			if (distance > 50){
				ShowNewHighScoreScreen = true;
			}
		}

		GetComponent<GoogleMobileAdsKProjekt> ().ShowBanner ();

		foreach (KeyValuePair<int, Result[]> result in inGameResults) {
			CarSmasherSocial.UnlockAchievements(result.Value);
		}
		CarSmasherSocial.UpdateAchievements (afterGameResults);

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
			}
		}
		//if someone had internet issues before, at least we can update high score with his best distance and come unlockable achievements
		//we don't want this. this is sending best score for every day player is playing
		//List<int> topScores = HighScores.GetTopScores (1);
		//int bestDistance = topScores.Count > 0 ? topScores [0] : 0;
		//InGameAchievements.Add(bestDistance, new Result[]{new Result (SCORE_TYPE.DISTANCE, bestDistance)});
	}

	void OnGUI(){
			
			if (ShowNewHighScoreScreen){
				
				GuiHelper.DrawBackground(delegate() {
					ShowNewHighScoreScreen = false;
				});
				
				GuiHelper.DrawText ("New High Score!", GuiHelper.SmallFont, 0.1, 0.08, 0.8, 0.07);
				GuiHelper.DrawText ("You just beat your high score with distance "+Distance+". \n\n Like to tell your friends about it?", GuiHelper.SmallFont, 0.1, 0.2, 0.75, 0.07);
				
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.2), GuiHelper.PercentH(0.70), GuiHelper.PercentW(0.6), GuiHelper.PercentH(0.11)), SpriteManager.GetFbShareButton(), GuiHelper.CustomButton)){
					CarSmasherSocial.FB.FeedHighScore(Distance);
					ShowNewHighScoreScreen = false;
				}
				
			} else if (ShowRideInfoScreen){
				GuiHelper.DrawText (GameOverReason, GuiHelper.SmallFont, 0.1, 0.08, 0.8, 0.07);
				
				GuiHelper.DrawText ("Collect oil drops to replenish fuel tank. Omit obstacles.\n"+
				                    "\nDistance made: "+Distance+
				                    "\nTurns made: "+Turns+
				                    "\nFuel picked up: "+FuelPickedUp+" "+
				                    "(in a row: "+FuelPickedUpInARow+") "+
				                    "(when low: "+FuelPickedUpWhenLow+") "
				                    , GuiHelper.SmallFont, 0.1, 0.5, 0.75, 0.07);
				GuiHelper.ButtonWithText(0.9, 0.92, 0.4, 0.2, "", SpriteManager.GetBackButton(), GuiHelper.CustomButton, delegate() {
					ShowRideInfoScreen = false;
				});
				
			} else {

				GuiHelper.DrawBackground(delegate() {
					gameObject.AddComponent<ScreenSplash>();
					Destroy(this);
				});

				GuiHelper.DrawText (GameOverReason, GuiHelper.SmallFontLeft, 0.1, 0.08, 0.8, 0.07);
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.75), GuiHelper.PercentH(0.08), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.1)), "help", GuiHelper.SmallFont)){
					ShowRideInfoScreen = true;
				}
				DrawTopScores(0.2f);
				
				Texture achievements = SpriteManager.GetAchievements();
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.07), GuiHelper.PercentH(0.6), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), achievements, GuiHelper.CustomButton)){
					CarSmasherSocial.ShowAchievements();
				}
				
				Texture leaderBoard = SpriteManager.GetLeaderboard();
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.28), GuiHelper.PercentH(0.56), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), leaderBoard, GuiHelper.CustomButton)){
					CarSmasherSocial.ShowLeaderBoard();
				}
				
				Texture soundButton = Sounds.IsMuted()?SpriteManager.GetSoundButtonMuted():SpriteManager.GetSoundButton();
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.52), GuiHelper.PercentH(0.56), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), soundButton, GuiHelper.CustomButton)){
					Sounds.Mute(!Sounds.IsMuted());
				}
				
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.27), GuiHelper.PercentH(0.67), GuiHelper.PercentW(0.45), GuiHelper.PercentH(0.3)), SpriteManager.GetStartButton(), GuiHelper.CustomButton)){
					Minigame m = gameObject.AddComponent<Minigame>();
					m.PrepareRace(Game.Me.ClassicCarConfig, ScreenAfterMinigameClassic.PrepareScreen, new Mission(new AchievQuery[]{}, new AchievQuery[]{}, new Reward(0, 0), ""));
					Destroy(this);
				}
				
				GuiHelper.ButtonWithText(0.9, 0.92, 0.4, 0.2, "", SpriteManager.GetBackButton(), GuiHelper.CustomButton, delegate() {
					BackToSplash();
					
				});
			}

	}


		private void BackToSplash(){
			ScreenSplash ss = gameObject.AddComponent<ScreenSplash> ();
			Destroy (this);
		}

	private bool IsBetterDrawDrivingTips(){
		List<int> scores = HighScores.GetTopScores (1);
		return scores.Count > 0 && scores[0] < 200;
	}
	
	private void DrawTopScores(float y){
		int place = HighScores.GetPlaceFor (Distance);
		bool isInTop = place <= HowManyInTopScores;
		List<int> top = HighScores.GetTopScores (HowManyInTopScores);
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
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.545), GuiHelper.PercentH(0.215), GuiHelper.PercentW(0.35), GuiHelper.PercentH(0.18)), fbButton, GuiHelper.CustomButton)){
			CarSmasherSocial.FB.FeedHighScore(bestScore);
		}
	}
}
