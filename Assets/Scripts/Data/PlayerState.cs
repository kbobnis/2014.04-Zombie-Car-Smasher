using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState  {

	private int _Coins;
	private int Exp;
	private Dictionary<string, bool> MissionsDone = new Dictionary<string, bool>();
	public CarConfig CarConfig;

	public PlayerState(CarConfig carConfig, int coins){
		CarConfig = carConfig;
		_Coins = coins;
	}

	public void BuyAndUpgrade(CarStatistic cs){
		_Coins -= cs.UpgradeCost ();
		cs.Upgrade ();
	}

	public int Level{
		get { return Mathf.FloorToInt( Exp / 10) + 1; }
	}

	public int Coins{
		get { return _Coins; }
	}

	public void MissionDone(Mission m){
		if (!MissionsDone.ContainsKey (m.Id)) {
			MissionsDone.Add (m.Id, true);	
		}
	}

	public bool IsMissionDone(Mission m){
		return MissionsDone.ContainsKey(m.Id);
	}

	public void RewardHim(Mission mission, Dictionary<int, Result[]> inGameAchievements, Result[] afterGameAchievements){
		//just what he collected
		foreach (Result result in afterGameAchievements) {
			switch(result.ScoreType){
			case SCORE_TYPE.COINS:
				_Coins += result.Value;
				break;
			}
		}

		if (mission.Passed (inGameAchievements, afterGameAchievements)) {
			GetReward(mission.Reward);
		}
	}

	private void GetReward(Reward r){
		_Coins += r.Coins;
		Exp += r.Exp;
	}

}
