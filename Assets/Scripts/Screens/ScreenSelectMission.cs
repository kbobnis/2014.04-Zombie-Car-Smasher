using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenSelectMission : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		
		PlayerState player = Game.Me.Player;

		GuiHelper.DrawBackground (delegate() {
			gameObject.AddComponent<ScreenAdvModeStart> ();
			Destroy (this);
		});

		List<Mission> missions = new List<Mission> ();
		missions.Add (new Mission (MissionId.D1, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 20)}, new Reward (10, 1), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.F1, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.BIGGER_EQUAL, 2)}, new Reward (20, 2), Environment.FuelMission));
		missions.Add (new Mission (MissionId.D2, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 30)}, new Reward (20, 2), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D3, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 40)}, new Reward (30, 3), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D4, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 50)}, new Reward (40, 4), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D5, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 60)}, new Reward (50, 5), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D6, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 70)}, new Reward (70, 6), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D7, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 80)}, new Reward (80, 7), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D8, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 90)}, new Reward (90, 7), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D9, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 100)}, new Reward (100, 7), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D10, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 110)}, new Reward (110, 7), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D11, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 120)}, new Reward (120, 7), Environment.ClassicMission));

		int i = 1; 
		foreach (Mission mission in missions) {
			if (!player.IsMissionDone(mission)){
				GuiHelper.DrawMissionLabel (i++, mission.Title, mission.Description, "Win "+mission.Reward.Description, delegate() {
					Minigame m = gameObject.AddComponent<Minigame>();
					m.PrepareRace(Game.Me.Player, ScreenAfterMinigameAdv.PrepareScreen, mission, Game.Me.Player.CarConfig);
					Destroy(this);
				});
			}
			//we show only four missions at a time
			if (i > 4){
				break;
			}

		}
	}
}
