using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
using System;

delegate int ProcessScore (int score);

public class CarSmasherSocial : MonoBehaviour {

	private static List<GoogleAchievement> Achievements = new List<GoogleAchievement> ();
	private static List<GoogleLeaderboard> LeaderBoards = new List<GoogleLeaderboard> ();

	internal delegate int ProcessScore (int score);
	public delegate void AfterAuthenticateD ();

	internal static bool Authenticated;
	public static FBKprojekt FB;

	static CarSmasherSocial(){
		FB = new FBKprojekt ();

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_FIRST_STEPS, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 100) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_APPRENTICE, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 250) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_EXPERT_DRIVER, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 400) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_MASTER_OF_ZOMBIE_CAR_SMASHER, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 1000) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_BEAT_THE_MASTER, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 1254) })));

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_AROUND_THE_WORLD, ACHIEVEMENT_TYPE.INCREMENTAL, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.NO_MATTER, 0) }), GoogleAchievement.DivideBy100));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_TO_THE_MOON, ACHIEVEMENT_TYPE.INCREMENTAL, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.NO_MATTER, 0) }), GoogleAchievement.DivideBy100));

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_CONSTANT_DRIVER, ACHIEVEMENT_TYPE.INCREMENTAL, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 100) }), GoogleAchievement.MakeOne));

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_ECO_FRIENDLY, ACHIEVEMENT_TYPE.INCREMENTAL, SCORE_TYPE.FUEL_PICKED,  new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.NO_MATTER, 0) }))); 
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_A_YEAR_FOR_THE_WORLD, ACHIEVEMENT_TYPE.INCREMENTAL, SCORE_TYPE.FUEL_PICKED,  new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.NO_MATTER, 0) }))); 
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_ALL_IS_MINE, ACHIEVEMENT_TYPE.INCREMENTAL, SCORE_TYPE.FUEL_PICKED,  new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.NO_MATTER, 0) }))); 

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_WHAT_IS_THAT, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.FUEL_PICKED, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.BIGGER_EQUAL, 5) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_I_CAN_GET_FURTHER_WITH_THIS, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.FUEL_PICKED, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.BIGGER_EQUAL, 25) }))); 
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_DO_I_NEED_THAT_MUCH, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.FUEL_PICKED, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.BIGGER_EQUAL, 50) })));

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_STUNT_DRIVER, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.FUEL_PICKED_IN_ROW, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED_IN_ROW, SIGN.BIGGER_EQUAL, 10) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_BULLSEYE, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.FUEL_PICKED_IN_ROW, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED_IN_ROW, SIGN.BIGGER_EQUAL, 17) })));

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_I_LIKE_THE_WHEEL, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.TURNS, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.TURNS, SIGN.BIGGER_EQUAL, 50), new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.SMALLER_EQUAL, 100) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_SCHUMACHER, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.TURNS, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.TURNS, SIGN.BIGGER_EQUAL, 75), new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.SMALLER_EQUAL, 100) })));

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_A_LITTLE_BIT_MORE, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.FUEL_PICKED_WHEN_LOW, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED_WHEN_LOW, SIGN.BIGGER_EQUAL, 1)})));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_I_DONT_NEED_FUEL_RIGHT_NOW, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.FUEL_PICKED_WHEN_LOW, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED_WHEN_LOW, SIGN.BIGGER_EQUAL, 3)})));

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_TEMPORARY_EMPTINESS, ACHIEVEMENT_TYPE.INCREMENTAL, SCORE_TYPE.FUEL_PICKED_WHEN_LOW, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED_WHEN_LOW, SIGN.NO_MATTER, 0)})));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_EVERLASTING_POVERTY, ACHIEVEMENT_TYPE.INCREMENTAL, SCORE_TYPE.FUEL_PICKED_WHEN_LOW, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED_WHEN_LOW, SIGN.NO_MATTER, 0)})));

		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_COMBO, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.TURNS, SIGN.BIGGER_EQUAL, 50), new AchievQuery(SCORE_TYPE.FUEL_PICKED_IN_ROW, SIGN.BIGGER_EQUAL, 5), new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.SMALLER_EQUAL, 100) })));
		Achievements.Add (new GoogleAchievement (GoogleAchievement.ACHIEV_COMBO_MARATHON, ACHIEVEMENT_TYPE.UNLOCKABLE, SCORE_TYPE.DISTANCE, new List<AchievQuery> (new AchievQuery[] { new AchievQuery (SCORE_TYPE.TURNS, SIGN.BIGGER_EQUAL, 100), new AchievQuery(SCORE_TYPE.FUEL_PICKED_IN_ROW, SIGN.BIGGER_EQUAL, 9), new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.SMALLER_EQUAL, 200) })));

		LeaderBoards.Add (new GoogleLeaderboard (GoogleLeaderboard.LEADERB_BEST_DISTANCES));
	}


	public static void InitializeSocial(bool forceUI, AfterAuthenticateD afterSuccess=null, AfterAuthenticateD afterFailure=null){
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();

		if (!Authenticated && (CanIAsk () || forceUI)) {
			Social.localUser.Authenticate ((bool success) => {
				Authenticated = success;
				SaveAnswerForAuthentication (success);
				if (Authenticated){
					if (afterSuccess != null) {
						afterSuccess ();
					}
				} else {
					afterFailure();
				}
			});
		} else {
			afterFailure();
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

	public static void UpdateAchievements (Result[] results){
		foreach (GoogleAchievement ach in Achievements) {
			ach.Update(results);
		}

		UpdateLeaderBoards (results);

	}

	private static void UpdateLeaderBoards(Result[] r){
		foreach (GoogleLeaderboard l in LeaderBoards) {
			l.Update(r);
		}
	}

	/**
	 * On some occasions we don't want to increment achievements, because in the end they would be incremented twice for one race.
	 **/
	public static void UnlockAchievements(Result[] results){
		foreach (GoogleAchievement ach in Achievements) {
			if (ach.Type == ACHIEVEMENT_TYPE.UNLOCKABLE){
				ach.Update(results);
			}
		}
		UpdateLeaderBoards (results);
	}

	public static void ShowLeaderBoard(){

		if (Authenticated) {
			ShowLeaderBoardInner();
		}else {
			InitializeSocial(true, ShowLeaderBoardInner, delegate(){});
		}
	}

	private static void ShowLeaderBoardInner(){
		((PlayGamesPlatform)Social.Active).ShowLeaderboardUI (GoogleLeaderboard.LEADERB_BEST_DISTANCES);
	}

	public static void ShowAchievements(){

		if (Authenticated) {
			Social.ShowAchievementsUI();
		}else {
			InitializeSocial(true, Social.ShowAchievementsUI, delegate(){});
		}
	}

}
public class GoogleLeaderboard{
	public const string LEADERB_BEST_DISTANCES = "CgkI0eO__P0OEAIQAQ";

	private string Id;
		
	public GoogleLeaderboard(string id){
		Id = id;
	}

	public void Update(Result[] results){
		foreach(Result r in results){
			if (r.ScoreType == SCORE_TYPE.DISTANCE){
				Debug.Log("Reporting score "+ r.Value +" to leaderboard");
				if (CarSmasherSocial.Authenticated) {
					Social.ReportScore (r.Value, Id, (bool success) => {});
				}
			}
		}
	}
}
public enum SCORE_TYPE{
	DISTANCE, FUEL_PICKED, FUEL_PICKED_WHEN_LOW, TURNS, FUEL_PICKED_IN_ROW, COINS, SHIELDS_USED
}
public enum SIGN{
	SMALLER_EQUAL, EQUAL, BIGGER_EQUAL, NO_MATTER
}
enum ACHIEVEMENT_TYPE{
	INCREMENTAL, UNLOCKABLE
}

public class Result{
	protected SCORE_TYPE _ScoreType;
	protected int _Value;
	
	public SCORE_TYPE ScoreType{
		get { return _ScoreType; }
	}
	public int Value {
		get { return _Value; }
	}
	
	public Result(SCORE_TYPE scoreType, int value){
		_ScoreType = scoreType;
		_Value = value;
	}
}

public class AchievQuery : Result {
	private SIGN _Sign;
	
	public SIGN Sign {
		get { return _Sign; }
	}
	
	public AchievQuery(SCORE_TYPE scoreType, SIGN sign, int value):base(scoreType, value){
		_Sign = sign;
	}
	
	public bool IsTheSameType(Result r){
		return ScoreType == r.ScoreType;
	}
	public bool CanAccept(Result r){
		if (!IsTheSameType (r)) {
			return true;
		}
		
		switch (Sign) {
		case SIGN.BIGGER_EQUAL: return r.Value >= Value;
		case SIGN.EQUAL : return Value == r.Value;
		case SIGN.SMALLER_EQUAL : return r.Value <= Value;
		case SIGN.NO_MATTER: return true;
		default:
			throw new Exception("There is no operation for sign: " + Sign);
		}
	}
	
}

class GoogleAchievement {

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

	public const string ACHIEV_TEMPORARY_EMPTINESS = "CgkI0eO__P0OEAIQFg";
	public const string ACHIEV_EVERLASTING_POVERTY ="CgkI0eO__P0OEAIQFw";

	public const string ACHIEV_COMBO = "CgkI0eO__P0OEAIQGA";
	public const string ACHIEV_COMBO_MARATHON = "CgkI0eO__P0OEAIQGQ";

	public const string ACHIEV_BEAT_THE_MASTER = "CgkI0eO__P0OEAIQGg";

	private string Id;
	private ACHIEVEMENT_TYPE _Type;
	private SCORE_TYPE WhatIncrement;
	private List<AchievQuery> Queries;
	private ProcessScore ProcessScore;
	
	public ACHIEVEMENT_TYPE Type {
		get { return _Type; }
	}

	public GoogleAchievement(string id, ACHIEVEMENT_TYPE type, SCORE_TYPE whatIncrement, List<AchievQuery> achievQuery, ProcessScore processScore=null){
		Id = id;
		_Type = type;
		WhatIncrement = whatIncrement;
		Queries = achievQuery;
		ProcessScore = processScore!=null?processScore:DefaultProcessScore;
	}

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
		case ACHIEV_TEMPORARY_EMPTINESS: return "Temporary emptiness";
		case ACHIEV_EVERLASTING_POVERTY: return "Everlasting poverty";
		case ACHIEV_COMBO : return "Combo";
		case ACHIEV_COMBO_MARATHON: return "Combo marathon";
		case ACHIEV_BEAT_THE_MASTER: return "Beat the master";
		default: return "There is no name for id " + id;
				}
	}

	public static void UnlockAchievement(string id){
		Debug.Log ("[UnlockAchievement] " + GoogleAchievement.GetNameForId(id));
		if (CarSmasherSocial.Authenticated) {
			Social.ReportProgress (id, 100.0f, (bool success) => {});
		} 
	}
	public static void IncrementAchievement(string id, int value){
		Debug.Log ("[IncrementAchievement] " + GoogleAchievement.GetNameForId(id) + " with " + value);
		if (CarSmasherSocial.Authenticated) {
			((PlayGamesPlatform)Social.Active).IncrementAchievement (id, value, (bool success) => {});
		}
	}

	public void Update(Result[] results){
		bool allIsOk = true;
		int score = -1;

		foreach (AchievQuery query in Queries) {
			//is there any query that wasn't set in the results?
			bool wasChecked = false;
			foreach(Result result in results){
				if (result.ScoreType == WhatIncrement){
					score = result.Value;
				}
				if (query.IsTheSameType(result)){
					wasChecked = true;
				}

				if (!query.CanAccept(result)){
					allIsOk = false;
				}
			}
			//if there was no result for specific query, then this is no good
			if (!wasChecked){
				allIsOk = false;
			}
		}


		if (allIsOk && score != -1) { //score is -1 when achievement and results have different scoreTypes. no worries, this is normal, when updating just some of achievements while game is running

			if (_Type == ACHIEVEMENT_TYPE.INCREMENTAL && ProcessScore(score) > 0) { //there is no need to send request to inrement by zero
				GoogleAchievement.IncrementAchievement (Id, ProcessScore(score));
			}
			if (_Type == ACHIEVEMENT_TYPE.UNLOCKABLE){
				GoogleAchievement.UnlockAchievement(Id);
			}
		}
	}

	public static int DivideBy100(int score){
		return Mathf.RoundToInt (score / 100f);
	}

	public static int MakeOne(int score){
		return 1;
	}
	public static int DefaultProcessScore(int score){
		return score;
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
			//FB.Feed("","https://www.facebook.com/ZombieCarSmasher","New Highscore "+Score,"", "I drove distance "+Score+" in Zombie Car Smasher. Can you beat me?","http://philon.pl/zombieCarSmasher/icon.png");
		}
	}

	public void Like(){
		Application.OpenURL("https://www.facebook.com/ZombieCarSmasher");
	}
}
