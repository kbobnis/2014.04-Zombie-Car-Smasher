using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
using System;

enum SCORE_TYPE{
	DISTANCE, FUEL_PICKED, FUEL_PICKED_WHEN_LOW, TURNS, FUEL_PICKED_IN_ROW
}
enum SIGN{
	SMALLER, EQUAL, BIGGER, NO_MATTER
}
enum ACHIEVEMENT_TYPE{
	INCREMENTAL, UNLOCKABLE
}
class AchievQuery{
	private SCORE_TYPE ScoreType;
	private SIGN Sign;
	private int Value;

	public AchievQuery(SCORE_TYPE scoreType, SIGN sign, int value){
		ScoreType = scoreType;
		Sign = sign;
		Value = value;
	}
}

public class CarSmasherSocial : MonoBehaviour {

	private static List<CommonAchievement> Achievements = new List<CommonAchievement> ();
	private static List<GoogleLeaderboard> LeaderBoards = new List<GoogleLeaderboard> ();

	private static List<CommonAchievement> FuelInOneGame = new List<CommonAchievement> ();
	private static List<CommonAchievement> FuelInOneGameInARow = new List<CommonAchievement> ();
	private static List<CommonAchievement> TurnsMadeInOneGame = new List<CommonAchievement> ();
	private static List<CommonAchievement> FuelPickedWhenLow = new List<CommonAchievement>();

	internal static bool Authenticated;
	private delegate void AfterAuthenticateD ();
	private static AfterAuthenticateD AfterAuthenticate;

	public static FBKprojekt FB;


	static CarSmasherSocial(){
		FB = new FBKprojekt ();

		//Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_AROUND_THE_WORLD, ACHIEVEMENT_TYPE.INCREMENTAL, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.NO_MATTER, 40000) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_AROUND_THE_WORLD, 40000, CommonAchievement.IncrementalDiv100));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_FIRST_STEPS, 100, CommonAchievement.Unlockable));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_APPRENTICE, 250, CommonAchievement.Unlockable));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_EXPERT_DRIVER, 400, CommonAchievement.Unlockable));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_MASTER_OF_ZOMBIE_CAR_SMASHER, 1000, CommonAchievement.Unlockable));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_TO_THE_MOON, 384400, CommonAchievement.IncrementalDiv100));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_CONSTANT_DRIVER, 100, CommonAchievement.IncrementIf));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_ECO_FRIENDLY, 50, CommonAchievement.IncrementalFuel));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_A_YEAR_FOR_THE_WORLD, 250, CommonAchievement.IncrementalFuel));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_ALL_IS_MINE, 500, CommonAchievement.IncrementalFuel));

		FuelInOneGame.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_WHAT_IS_THAT, 5, CommonAchievement.UnlockableFuel));
		FuelInOneGame.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_I_CAN_GET_FURTHER_WITH_THIS, 25, CommonAchievement.UnlockableFuel));
		FuelInOneGame.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_DO_I_NEED_THAT_MUCH, 50, CommonAchievement.UnlockableFuel));

		FuelInOneGameInARow.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_STUNT_DRIVER, 10, CommonAchievement.UnlockableFuel));
		FuelInOneGameInARow.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_BULLSEYE, 25, CommonAchievement.UnlockableFuel));

		TurnsMadeInOneGame.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_I_LIKE_THE_WHEEL, 60, CommonAchievement.UnlockableTurnsDistanceOver100));
		TurnsMadeInOneGame.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_SCHUMACHER, 100, CommonAchievement.UnlockableTurnsDistanceOver100));

		FuelPickedWhenLow.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_A_LITTLE_BIT_MORE, 1, CommonAchievement.UnlockableFuel));
		FuelPickedWhenLow.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_I_DONT_NEED_FUEL_RIGHT_NOW, 3, CommonAchievement.UnlockableFuel));


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
						GameOverWithScore(distanceToSave, 0, 0);
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

	public static void GameOverWithScore (int distance, int fuelPickedUp, int fuelPickedUpWhenLow){
		foreach (CommonAchievement ach in Achievements) {
			ach.Update (distance, fuelPickedUp, 0, fuelPickedUpWhenLow);
		}
		foreach (GoogleLeaderboard lead in LeaderBoards) {
			lead.Update (distance);
		}
	}

	public static void FuelPickedUpInOneGame(int howMuch){
		foreach(CommonAchievement ach in FuelInOneGame){
			ach.Update(0, howMuch, 0, 0);
		}
	}

	public static void TurnsMade(int turnsMade, int distance){
		foreach(CommonAchievement ach in TurnsMadeInOneGame){
			ach.Update(distance, 0, turnsMade, 0);
		}
	}

	public static void FuelPickedUpInOneGameInARow(int inARow){
		foreach(CommonAchievement ach in FuelInOneGameInARow){
			ach.Update(0, inARow, 0, 0);
		}
	}

	public static void FuelPickedUpWhenLow(int howMany){
		foreach(CommonAchievement ach in FuelPickedWhenLow){
			ach.Update(0, howMany, 0, 0);
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
		if (CarSmasherSocial.Authenticated) {
			Social.ReportScore (distance, Id, (bool success) => {});
		}
	}
}

delegate void CustomUpdateAchievement (int distance, string id, int value, int fuelPickedUp, int turnsMade, int fuelPickedUpWhenLow);

abstract class CommonAchievement{
	protected string Id;
	protected int Value;
	protected CustomUpdateAchievement CustomUpdate;


	public CommonAchievement(string id, int value, CustomUpdateAchievement customUpdate){
		Id = id;
		Value = value;
		CustomUpdate = customUpdate;
	}

	public void Update (int distance, int fuelPickedUp, int turnsMade, int fuelPickedUpWhenLow){
		CustomUpdate (distance, Id, Value, fuelPickedUp, turnsMade, fuelPickedUpWhenLow);

		/*
		List<AchievQuery> rzeczy = new List<AchievQuery> ();
		List<AchievQuery> otrzymane = new List<AchievQuery> ();
		bool allIsOk = false;
		ACHIEVEMENT_TYPE ten = ACHIEVEMENT_TYPE.INCREMENTAL;
		foreach (AchievQuery rzecz in rzeczy) {
			foreach(AchievQuery otrzymana in otrzymane){
				if (rzecz.IsTheSameType(otrzymana) && !rzecz.CanAccept(otrzymana)){
					allIsOk = false;
				}
			}
		}

		if (allIsOk) {
			if (ten == ACHIEVEMENT_TYPE.INCREMENTAL){
				((PlayGamesPlatform)Social.Active).IncrementAchievement(id, fuelPickedUp, (bool success) => {});
			} else {
				Social.ReportProgress (id, 100.0f, (bool success) => {});
			}
		}
		*/
	}

	private static void UnlockAchievement(string id){
		Debug.Log ("[UnlockAchievement] " + GoogleAchievement.GetNameForId(id));
		if (CarSmasherSocial.Authenticated) {
			Social.ReportProgress (id, 100.0f, (bool success) => {});
		} 
	}
	private static void IncrementAchievement(string id, int value){
		Debug.Log ("[IncrementAchievement] " + GoogleAchievement.GetNameForId(id) + " with " + value);
		if (CarSmasherSocial.Authenticated) {
			((PlayGamesPlatform)Social.Active).IncrementAchievement (id, value, (bool success) => {});
		}
	}


	public static void Unlockable(int distance, string id, int value, int fuelPickedUp, int turnsMade, int fuelPickedUpWhenLow) {
		if (distance >= value) {
			UnlockAchievement(id);
		}
	}

	public static void UnlockableFuel(int distance, string id, int value, int fuelPickedUp, int turnsMade, int fuelPickedUpWhenLow) {
		if (fuelPickedUp >= value) {
			UnlockAchievement(id);
		}
	}

	public static void IncrementalFuel(int distance, string id, int value, int fuelPickedUp, int turnsMade, int fuelPickedUpWhenLow) {
		IncrementAchievement (id, fuelPickedUp);
	}
			
	public static void IncrementalDiv100(int distance, string id, int value, int fuelPickedUp, int turnsMade, int fuelPickedUpWhenLow) {
		//google play has limit to 10 000 steps. We will divide here distance by 100 and in google+ write amounts 100 times smaller
		IncrementAchievement(id, Mathf.RoundToInt (distance / 100f));
	}
			
	public static void IncrementIf(int distance, string id, int value, int fuelPickedUp, int turnsMade, int fuelPickedUpWhenLow) {
		if (distance > value){
			IncrementAchievement(id, 1);
		}
	}

	public static void UnlockableTurnsDistanceOver100(int distance, string id, int value, int fuelPickedUp, int turnsMade, int fuelPickedUpWhenLow){
		if (distance == 100 && turnsMade > value) {
			UnlockAchievement(id);
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

	public const string ACHIEV_WHAT_IS_THAT = "CgkI0eO__P0OEAIQCg";
	public const string ACHIEV_I_CAN_GET_FURTHER_WITH_THIS = "CgkI0eO__P0OEAIQCw";
	public const string ACHIEV_DO_I_NEED_THAT_MUCH = "CgkI0eO__P0OEAIQDA";

	public const string ACHIEV_ECO_FRIENDLY = "CgkI0eO__P0OEAIQDQ";
	public const string ACHIEV_A_YEAR_FOR_THE_WORLD = "CgkI0eO__P0OEAIQDg";
	public const string ACHIEV_ALL_IS_MINE = "CgkI0eO__P0OEAIQDw";

	public const string ACHIEV_STUNT_DRIVER = "CgkI0eO__P0OEAIQEA";
	public const string ACHIEV_BULLSEYE = "CgkI0eO__P0OEAIQEQ";

	public const string ACHIEV_I_LIKE_THE_WHEEL = "CgkI0eO__P0OEAIQEg";
	public const string ACHIEV_SCHUMACHER = "CgkI0eO__P0OEAIQEw";

	public const string ACHIEV_A_LITTLE_BIT_MORE = "CgkI0eO__P0OEAIQFA";
	public const string ACHIEV_I_DONT_NEED_FUEL_RIGHT_NOW = "CgkI0eO__P0OEAIQFQ";

	//public GoogleAchievement(string id, ACHIEVEMENT_TYPE type, List<AchievQuery> achievQuery):base(id, type, achievQuery){}
	public GoogleAchievement(string id, int value, CustomUpdateAchievement update):base(id, value, update){}

	public static string GetNameForId(string id){
		switch (id) {
		case ACHIEV_AROUND_THE_WORLD: return "Around The World";
		case ACHIEV_FIRST_STEPS: return "First steps";
		case ACHIEV_APPRENTICE: return "Apprentice";
		case ACHIEV_EXPERT_DRIVER : return "Expert driver";
		case ACHIEV_MASTER_OF_ZOMBIE_CAR_SMASHER: return "Master Of Zombie Car Smasher";
		case ACHIEV_TO_THE_MOON : return "To The Moon";
		case ACHIEV_CONSTANT_DRIVER : return "Constant Driver";
		case ACHIEV_WHAT_IS_THAT: return "What is that";
		case ACHIEV_I_CAN_GET_FURTHER_WITH_THIS : return "I can get further with this";
		case ACHIEV_DO_I_NEED_THAT_MUCH: return "Do I Need that much";
		case ACHIEV_ECO_FRIENDLY: return "Eco friendly";
		case ACHIEV_A_YEAR_FOR_THE_WORLD: return "A year for the world";
		case ACHIEV_ALL_IS_MINE : return "All is mine";
		case ACHIEV_STUNT_DRIVER : return "Stunt Driver";
		case ACHIEV_BULLSEYE: return "Bullseye";
		case ACHIEV_I_LIKE_THE_WHEEL: return "I like the Wheel";
		case ACHIEV_SCHUMACHER: return "Schumacher";
		case ACHIEV_A_LITTLE_BIT_MORE: return " A little bit more";
		case ACHIEV_I_DONT_NEED_FUEL_RIGHT_NOW: return "I dont need fel right now";
		default: throw new Exception("There is no name for id " + id );
				}
	}
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
			FB.Feed("","https://play.google.com/store/apps/details?id=com.krzysiekprojekt.zombieCarSmasher","New Highscore "+Score,"", "I drove distance "+Score+". Can you beat me?","http://philon.pl/zombieCarSmasher/icon.png");
		}
	} 
}
