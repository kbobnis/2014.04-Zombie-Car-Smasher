using System.Collections.Generic;
using UnityEngine;

public class Mission{

	private List<AchievQuery> InGameReqs = new List<AchievQuery>();
	public List<AchievQuery> AfterGameReqs = new List<AchievQuery>();
	private Reward _Reward;
	public string Title, Description;
	private Environment _Env;
	public SCORE_TYPE ScoreType;
	public int Level;


	public Mission(SCORE_TYPE scoreType, int level){
		ScoreType = scoreType;
		Level = level;
		Initialize (new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (scoreType, SIGN.BIGGER_EQUAL, GetValueForScore (scoreType, level))}, GetRewardForLevel(level), Environment.ClassicMission);
	}

	private static int GetValueForScore(SCORE_TYPE scoreType, int level){
		switch (scoreType) {
			case SCORE_TYPE.DISTANCE: return CumulativePercent(20, 0.1f, level);
			case SCORE_TYPE.COINS: return CumulativePercent(2, 0.1f, level);
			case SCORE_TYPE.FUEL_PICKED: return CumulativePercent(2, 0.1f, level);
			case SCORE_TYPE.FUEL_PICKED_IN_ROW: return CumulativePercent(2, 0.1f, level);
			case SCORE_TYPE.FUEL_PICKED_WHEN_LOW: return CumulativePercent(1, 0.1f, level);
			case SCORE_TYPE.TURNS: return CumulativePercent(4, 0.1f, level);
		default: 
			throw new UnityException("There is no value for score type: " + scoreType);
		}
	}

	private static Reward GetRewardForLevel(int level){
		return new Reward (CumulativePercent(10, 0.1f, level));
	}

	private static int CumulativePercent(float baseValue, float percent, int cumulations){
		float sum = baseValue;
		for (int i=0; i < cumulations; i++) {
			sum *= 1.1f;
		}
		return (int)sum;
	}

	private void Initialize(AchievQuery[] inGameReqs, AchievQuery[] afterGameReqs, Reward reward, Environment env){
		_Env = env;

		if (inGameReqs != null) {
			foreach(AchievQuery aq in inGameReqs){
				InGameReqs.Add(aq);
			}
		}
		if (afterGameReqs != null){
			foreach(AchievQuery aq in afterGameReqs){
				AfterGameReqs.Add(aq);
			}
		}

		_Reward = reward;
		foreach (AchievQuery gameReq in afterGameReqs) {
			switch (gameReq.ScoreType) {
			case SCORE_TYPE.DISTANCE:
				Description = "Drive distance " + gameReq.Value;
				Title = "Distance " + gameReq.Value;
				break;
			case SCORE_TYPE.COINS:
				Description = "Collect " + gameReq.Value + " coins ";
				Title = "Coins " + gameReq.Value;
				break;
			case SCORE_TYPE.FUEL_PICKED:
				Description = "Collect " + gameReq.Value + " oil drops";
				Title = "Oil drops " + gameReq.Value;
				break;
			case SCORE_TYPE.FUEL_PICKED_IN_ROW:
				Description = "Collect " + gameReq.Value + " oil drops in a row";
				Title = "Oil drops in a row " + gameReq.Value;
				break;
			case SCORE_TYPE.FUEL_PICKED_WHEN_LOW:
				Description = "Collect " + gameReq.Value + " oil drops when low";
				Title = "Oil drops when low " + gameReq.Value;
				break;
			case SCORE_TYPE.TURNS:
				Description = "Make " + gameReq.Value + " turns";
				Title = "Make " + gameReq.Value + " turns";
				break;
			default:
				throw new UnityException ("ther is no description and title for score type : " + gameReq.ScoreType);
			}
		}
	}

	public int GetAmountDone(Result[] results){
		int amountDone = 0;

		SCORE_TYPE type = GetScoreType();
		foreach(Result r in results){
			if (r.ScoreType == type){
				amountDone = r.Value;
			}
		}
		return amountDone;
	}

	public SCORE_TYPE GetAmountType(){
		SCORE_TYPE s = SCORE_TYPE.COINS; //it can't be null
		foreach(AchievQuery aq in AfterGameReqs){
			s = aq.ScoreType;
		}
		return s;
	}

	public int GetAmountFull(){
		int amountFull = 0;
		foreach(AchievQuery aq in AfterGameReqs){
			amountFull = aq.Value;
		}
		return amountFull;
	}

	private SCORE_TYPE GetScoreType(){
		SCORE_TYPE type = SCORE_TYPE.COINS; //it can't be null
		foreach(AchievQuery aq in AfterGameReqs){
			type = aq.ScoreType;
		}
		return type;
	}

	public Reward Reward{
		get { return _Reward; }
	}

	public Environment Env{
		get { return _Env; }
	}



	public static Mission Classic{
		get { return new Mission (SCORE_TYPE.DISTANCE, 1); } 
	}

	public bool Passed(Dictionary<int, Result[]> InGameResults, Result[] AfterGameResults){

		bool passed = true;
		foreach (KeyValuePair<int, Result[]> kvp in InGameResults) {
			if (!Check(InGameReqs, kvp.Value)){
				passed = false;
				break;
			}
		}
		if (InGameResults.Count == 0 && InGameReqs.Count > 0) {
			passed = false;
		}

		return passed && Check (AfterGameReqs, AfterGameResults);
	}

	private static bool Check(List<AchievQuery> reqs, Result[] results){
		bool allIsOk = true;

		foreach (AchievQuery query in reqs) {
			//is there any query that wasn't set in the results?
			bool wasChecked = false;
			foreach(Result result in results){
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
		return allIsOk;
	}

	public static Mission SearchMissionForPlayer(PlayerState player){

		List<Mission> missions = new List<Mission> ();

		missions.Add (new Mission (SCORE_TYPE.DISTANCE, player.GetNextMissionLevel (SCORE_TYPE.DISTANCE)));
		missions.Add (new Mission (SCORE_TYPE.FUEL_PICKED, player.GetNextMissionLevel (SCORE_TYPE.FUEL_PICKED)));
		missions.Add (new Mission (SCORE_TYPE.FUEL_PICKED_IN_ROW, player.GetNextMissionLevel (SCORE_TYPE.FUEL_PICKED_IN_ROW)));
		missions.Add (new Mission (SCORE_TYPE.FUEL_PICKED_WHEN_LOW, player.GetNextMissionLevel (SCORE_TYPE.FUEL_PICKED_WHEN_LOW)));
		missions.Add (new Mission (SCORE_TYPE.TURNS, player.GetNextMissionLevel (SCORE_TYPE.FUEL_PICKED_WHEN_LOW)));

		return missions [Random.Range (0, missions.Count)];
	}
}


