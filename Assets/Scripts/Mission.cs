using System.Collections.Generic;
using UnityEngine;

public class Mission{

	private AchievQuery[] InGameReqs;
	private AchievQuery[] AfterGameReqs;
	public Reward Reward;
	public string Title, Description;
	/**
	 * Id is used to save data if this mission was done
	 **/
	public string Id;

	public Mission(string id, AchievQuery[] inGameReqs, AchievQuery[] afterGameReqs, Reward reward, string title){
		Id = id;
		InGameReqs = inGameReqs;
		AfterGameReqs = afterGameReqs;
		Reward = reward;
		Title = title;
		foreach (AchievQuery gameReq in afterGameReqs) {
			switch (gameReq.ScoreType) {
			case SCORE_TYPE.DISTANCE:
				Description = "Drive distance " + gameReq.Value;
				break;
			default:
				throw new UnityException ("ther is no description for score type : " + gameReq.ScoreType);
			}
		}
	}

	public static Mission Classic{
		get { return new Mission ("classic", new AchievQuery[]{}, new AchievQuery[]{}, new Reward (0, 0), ""); } 
	}

	public void RewardPlayer(PlayerState player){
		Reward.GiveItselfToPlayer(player);
		player.MissionDone (this);
	}

	public bool Passed(Dictionary<int, Result[]> InGameResults, Result[] AfterGameResults){

		bool passed = true;
		foreach (KeyValuePair<int, Result[]> kvp in InGameResults) {
			if (!Check(InGameReqs, kvp.Value)){
				passed = false;
				break;
			}
		}
		if (InGameResults.Count == 0 && InGameReqs.Length > 0) {
			passed = false;
		}

		return passed && Check (AfterGameReqs, AfterGameResults);
	}

	public static bool Check(AchievQuery[] reqs, Result[] results){
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
}


