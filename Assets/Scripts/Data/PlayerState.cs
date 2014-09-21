using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState  {

	private int _Coins;
	private int Exp;
	private Dictionary<MissionId, bool> MissionsDone = new Dictionary<MissionId, bool>();
	public CarConfig CarConfig;

	public PlayerState(CarConfig carConfig, int coins){
		Initialize (carConfig, coins);
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

	private string Serialize(){
		Dictionary<string, string> dict = new Dictionary<string, string> ();
		dict ["coins"] = ""+_Coins;
		dict ["exp"] = ""+Exp;
		dict ["missionsDone"] = MiniJSON.Json.Serialize (MissionsDone);
		dict ["carConfig"] = CarConfig.Serialize ();
		string state = MiniJSON.Json.Serialize (dict);
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
		Dictionary<string, object> dict =  (Dictionary<string, object>)MiniJSON.Json.Deserialize (serialized);
		_Coins = int.Parse( (string)dict ["coins"]);
		Exp = int.Parse( (string)dict ["exp"]);
		Dictionary<string, object> tmp = (Dictionary<string, object>)MiniJSON.Json.Deserialize ((string)dict ["missionsDone"]);
		foreach (KeyValuePair<string, object> kvp in tmp) {
			MissionId parsed = (MissionId)System.Enum.Parse(typeof(MissionId), kvp.Key);
			if (!MissionsDone.ContainsKey(parsed)){
				MissionsDone.Add(parsed, true);
			}
		}
		CarConfig.Deserialize((string)dict["carConfig"]);
	}
}
