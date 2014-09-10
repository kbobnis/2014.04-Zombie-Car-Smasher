using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
using System;



public class CarSmasherSocial : MonoBehaviour {

	private static List<CommonAchievement> Achievements = new List<CommonAchievement> ();
	private static List<GoogleLeaderboard> LeaderBoards = new List<GoogleLeaderboard> ();

	private static bool Authenticated;
	private delegate void AfterAuthenticateD ();
	private static AfterAuthenticateD AfterAuthenticate;

	public static FBKprojekt FB;


	static CarSmasherSocial(){
		FB = new FBKprojekt ();

		Achievements.Add (new GoogleAchievement ("Around The World", GoogleAchievement.ACHIEV_AROUND_THE_WORLD, 40000, CommonAchievement.Incremental));
		Achievements.Add (new GoogleAchievement ("First Steps", GoogleAchievement.ACHIEV_FIRST_STEPS, 100, CommonAchievement.Unlockable));
		Achievements.Add (new GoogleAchievement ("Apprentice", GoogleAchievement.ACHIEV_APPRENTICE, 250, CommonAchievement.Unlockable));
		Achievements.Add (new GoogleAchievement ("Expert Driver", GoogleAchievement.ACHIEV_EXPERT_DRIVER, 400, CommonAchievement.Unlockable));
		Achievements.Add (new GoogleAchievement ("Master of Zombie Car Smasher", GoogleAchievement.ACHIEV_MASTER_OF_ZOMBIE_CAR_SMASHER, 1000, CommonAchievement.Unlockable));
		Achievements.Add (new GoogleAchievement ("To The Moon", GoogleAchievement.ACHIEV_TO_THE_MOON, 384400, CommonAchievement.Incremental));
		Achievements.Add (new GoogleAchievement ("Constant Driver", GoogleAchievement.ACHIEV_CONSTANT_DRIVER, 100, CommonAchievement.IncrementIf));

		LeaderBoards.Add (new GoogleLeaderboard (GoogleLeaderboard.LEADERB_BEST_DISTANCES));
	}


	public static void InitializeSocial(bool forceUI, int distanceToSave=-1){
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();

		if (CanIAsk() || forceUI) {
			Social.localUser.Authenticate ((bool success) => {
				Debug.Log ("zalogowany " + (success ? "tak" : "nie"));
				Authenticated = success;
				SaveAnswerForAuthentication(success);
				if (Authenticated && AfterAuthenticate != null) {
					if (distanceToSave != -1){
						GameOverWithScore(distanceToSave);
					}
					AfterAuthenticate ();
					AfterAuthenticate = null;
				}
			});
		}
	}

	/**
	 * We automatically ask only for the first time
	 **/
	private static bool CanIAsk(){
		return PlayerPrefs.GetString ("hasPreviouslyAccepted", "yes") == "yes";
	}

	private static void SaveAnswerForAuthentication(bool answer){
		PlayerPrefs.SetString ("hasPreviouslyAccepted", answer ? "yes" : "no");
	}



	public static void GameOverWithScore (int distance){

		if (Authenticated) {
			foreach (CommonAchievement ach in Achievements) {
				ach.Update (distance);
			}
			foreach (GoogleLeaderboard lead in LeaderBoards) {
				lead.Update (distance);
			}
		} 
	}

	public static void ShowLeaderBoard(int lastDistance){
		AfterAuthenticate = ShowLeaderBoardInner;

		if (Authenticated) {
			ShowLeaderBoardInner();
		}else {
			InitializeSocial(true, lastDistance);
		}
	}

	private static void ShowLeaderBoardInner(){
		((PlayGamesPlatform)Social.Active).ShowLeaderboardUI (GoogleLeaderboard.LEADERB_BEST_DISTANCES);
	}

	public static void ShowAchievements(int lastDistance){
		AfterAuthenticate = Social.ShowAchievementsUI;

		if (Authenticated) {
			Social.ShowAchievementsUI();
		}else {
			InitializeSocial(true, lastDistance);
		}
	}

}
class GoogleLeaderboard{
	public const string LEADERB_BEST_DISTANCES = "CgkI0eO__P0OEAIQAQ";

	private string Id;
		
	public GoogleLeaderboard(string id){
		Id = id;
	}

	public void Update(int distance){
		Social.ReportScore (distance, Id, (bool success) => {
		});
	}
}

delegate void CustomUpdateAchievement (int distance, string id, int value);

abstract class CommonAchievement{
	protected string Id;
	protected int Value;
	protected string Name;
	protected CustomUpdateAchievement CustomUpdate;

	public CommonAchievement(string name, string id, int value, CustomUpdateAchievement customUpdate){
		Id = id;
		Value = value;
		Name = name;
		CustomUpdate = customUpdate;
	}

	public void Update (int distance){
		CustomUpdate (distance, Id, Value);
	}

	public static void Unlockable(int distance, string id, int value) {
		if (distance >= value) {
			Social.ReportProgress (id, 100.0f, (bool success) => {});
		}
	}
			
	public static void Incremental(int distance, string id, int value) {
		//google play has limit to 10 000 steps. We will divide here distance by 100 and in google+ write amounts 100 times smaller
		((PlayGamesPlatform)Social.Active).IncrementAchievement (id, Mathf.RoundToInt (distance / 100f), (bool success) => {});
	}
			
	public static void IncrementIf(int distance, string id, int value) {
		if (distance > value){
			((PlayGamesPlatform)Social.Active).IncrementAchievement(id, 1, (bool success) => {});
		}
	}
}



class GoogleAchievement : CommonAchievement{

	public const string ACHIEV_AROUND_THE_WORLD = "CgkI0eO__P0OEAIQAg";
	public const string ACHIEV_FIRST_STEPS = "CgkI0eO__P0OEAIQAw";
	public const string ACHIEV_APPRENTICE = "CgkI0eO__P0OEAIQBA";
	public const string ACHIEV_EXPERT_DRIVER = "CgkI0eO__P0OEAIQBQ";
	public const string ACHIEV_MASTER_OF_ZOMBIE_CAR_SMASHER = "CgkI0eO__P0OEAIQBg";
	public const string ACHIEV_TO_THE_MOON = "CgkI0eO__P0OEAIQBw";
	public const string ACHIEV_CONSTANT_DRIVER = "CgkI0eO__P0OEAIQCA";


	public GoogleAchievement(string name, string id, int value, CustomUpdateAchievement customUpdate):base(name, id, value, customUpdate){}
}

public class FBKprojekt{

	private int Score;

	public void FeedHighScore(int score){
		Score = score;
		if (!FB.IsLoggedIn){
			FB.Login("", ShareBestScores);
		} else {
			ShareBestScores(null);
		}
	}
	private void ShareBestScores(FBResult result)
	{
		if (FB.IsLoggedIn){
			FB.Feed("", "https://play.google.com/store/apps/details?id=com.krzysiekprojekt.zombieCarSmasher","New Highscore","", "I drove distance "+Score+". Can you beat me?","http://philon.pl/zombieCarSmasher/icon.png");
		}
	} 
}
