using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState  {

	private int _Coins;
	private Dictionary<SCORE_TYPE, int> MissionsDone = new Dictionary<SCORE_TYPE, int>();
	public CarConfig CarConfig;

	public PlayerState(CarConfig carConfig, int coins){
		Initialize (carConfig, coins);
	}

	public int GetNextMissionLevel(SCORE_TYPE ScoreType){
		int next = 1;
		if (MissionsDone.ContainsKey (ScoreType)) {
			next = MissionsDone[ScoreType] + 1;
		}
		return next;
	}

	private void Initialize(CarConfig carConfig, int coins){
		CarConfig = carConfig;
		_Coins = coins;
	}

	public void Reset(){
		Initialize (new CarConfig (CarConfig.MODE_ADV), 20);
		MissionsDone.Clear ();
	}

	public void BuyAndUpgrade(CarStatistic cs){
		_Coins -= cs.UpgradeCost ();
		cs.Upgrade ();
	}

	public int Coins{
		get { return _Coins; }
	}

	public void MissionDone(Mission m){
		if (!MissionsDone.ContainsKey(m.ScoreType)){
			MissionsDone.Add (m.ScoreType, m.Level);	
		} else {
			MissionsDone[m.ScoreType] = m.Level;
		}
	}

	public bool IsMissionDone(Mission m){
		bool isDone = false;
		if (MissionsDone.ContainsKey (m.ScoreType)) {
			isDone = MissionsDone[m.ScoreType] >= m.Level;
		}
		return isDone;
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
			MissionDone (mission);
		}

	}

	private void GetReward(Reward r){
		_Coins += r.Coins;
	}

	private string Serialize(){
		Dictionary<string, string> dict = new Dictionary<string, string> ();
		dict ["coins"] = ""+_Coins;
		dict ["missionsDone"] = MiniJSON.Json.Serialize ( MissionsDone);
		dict ["carConfig"] = CarConfig.Serialize ();
		string state = MiniJSON.Json.Serialize (dict);
		Debug.Log ("serialize player state: " + state);
		return state;
	}

	public void Save(){
		PlayerPrefs.SetString ("player", Serialize ());
	}

	public void Load(){
		if (PlayerPrefs.HasKey ("player")) {
			Deserialize (PlayerPrefs.GetString ("player"));
		}
	}

	private void Deserialize(string serialized){
		Debug.Log ("Deserializing: " + serialized);
		Dictionary<string, object> dict =  (Dictionary<string, object>)MiniJSON.Json.Deserialize (serialized);
		_Coins = int.Parse( (string)dict ["coins"]);
		Dictionary<string, object> tmp = (Dictionary<string, object>)MiniJSON.Json.Deserialize ((string)dict ["missionsDone"]);
		foreach (KeyValuePair<string, object> kvp in tmp) {

			SCORE_TYPE parsed = (SCORE_TYPE)System.Enum.Parse(typeof(SCORE_TYPE), kvp.Key);
			object value = kvp.Value;
			if (!MissionsDone.ContainsKey(parsed)){
				MissionsDone.Add(parsed, int.Parse(string.Format("{0}", value)));
			}
		}
		CarConfig.Deserialize((string)dict["carConfig"]);
	}
}
