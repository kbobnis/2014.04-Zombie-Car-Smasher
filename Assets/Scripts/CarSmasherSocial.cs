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

	static CarSmasherSocial(){
		Achievements.Add (new GoogleAchievement ("Around The World", GoogleAchievement.ACHIEV_AROUND_THE_WORLD, 40000, AchievementType.INCREMENTAL));
		Achievements.Add (new GoogleAchievement ("First Steps", GoogleAchievement.ACHIEV_FIRST_STEPS, 100, AchievementType.UNLOCKABLE));
		Achievements.Add (new GoogleAchievement ("Apprentice", GoogleAchievement.ACHIEV_APPRENTICE, 250, AchievementType.UNLOCKABLE));
		Achievements.Add (new GoogleAchievement ("Expert Driver", GoogleAchievement.ACHIEV_EXPERT_DRIVER, 400, AchievementType.UNLOCKABLE));
		Achievements.Add (new GoogleAchievement ("Master of Zombie Car Smasher", GoogleAchievement.ACHIEV_MASTER_OF_ZOMBIE_CAR_SMASHER, 1000, AchievementType.UNLOCKABLE));
		Achievements.Add (new GoogleAchievement ("To The Moon", GoogleAchievement.ACHIEV_TO_THE_MOON, 384400, AchievementType.INCREMENTAL));

		LeaderBoards.Add (new GoogleLeaderboard (GoogleLeaderboard.LEADERB_BEST_DISTANCES));
	}


	public static void InitializeSocial(){
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();
		Social.localUser.Authenticate ((bool success) => {
			Debug.Log("zalogowany " + (success?"tak":"nie"));
			Authenticated = success;
			if (Authenticated){
				AfterAuthenticate();
				AfterAuthenticate = null;
			}
		});
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

	public static void ShowLeaderBoard(){
		AfterAuthenticate = ShowLeaderBoardInner;

		if (Authenticated) {
			ShowLeaderBoardInner();
		}else {
			InitializeSocial();
		}
	}

	private static void ShowLeaderBoardInner(){
		((PlayGamesPlatform)Social.Active).ShowLeaderboardUI (GoogleLeaderboard.LEADERB_BEST_DISTANCES);
	}

	public static void ShowAchievements(){
		AfterAuthenticate = Social.ShowAchievementsUI;

		if (Authenticated) {
			Social.ShowAchievementsUI();
		}else {
			InitializeSocial();
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


enum AchievementType{
	INCREMENTAL, UNLOCKABLE
}

abstract class CommonAchievement{
	protected string Id;
	protected int Value;
	protected string Name;
	protected AchievementType Type;

	public CommonAchievement(string name, string id, int value, AchievementType type){
		Id = id;
		Value = value;
		Name = name;
		Type = type;
	}

	public abstract void Update (int distance);
}

class GoogleAchievement : CommonAchievement{

	public const string ACHIEV_AROUND_THE_WORLD = "CgkI0eO__P0OEAIQAg";
	public const string ACHIEV_FIRST_STEPS = "CgkI0eO__P0OEAIQAw";
	public const string ACHIEV_APPRENTICE = "CgkI0eO__P0OEAIQBA";
	public const string ACHIEV_EXPERT_DRIVER = "CgkI0eO__P0OEAIQBQ";
	public const string ACHIEV_MASTER_OF_ZOMBIE_CAR_SMASHER = "CgkI0eO__P0OEAIQBg";
	public const string ACHIEV_TO_THE_MOON = "CgkI0eO__P0OEAIQBw";

	public GoogleAchievement(string name, string id, int value, AchievementType type):base(name, id, value, type){
	}

	override public void Update(int distance){

		switch (Type) {
			case AchievementType.UNLOCKABLE:
				if (distance >= Value) {
					Social.ReportProgress (Id, 100.0f, (bool success) => {
					});
				}
			break;
			case AchievementType.INCREMENTAL:
				//google play has limit to 10 000 steps. We will divide here distance by 100 and in google+ write amounts 100 times smaller
				((PlayGamesPlatform)Social.Active).IncrementAchievement(Id, Mathf.RoundToInt(distance/100f), (bool success) => {
				});
			break;
			default:
				throw new UnityException("There is no achievement type " + Type);
		}
	}
}
