using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenSelectMission : BaseScreen {

	override protected void OnGUIInner(){
		
		PlayerState player = Game.Me.Player;

		GuiHelper.DrawAtTop ("Select mission");

		List<Mission> missions = new List<Mission> ();
		missions.Add (new Mission (MissionId.D1, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 20)}, new Reward (10), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.F1, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.BIGGER_EQUAL, 2)}, new Reward (20), Environment.FuelMission));
		missions.Add (new Mission (MissionId.D2, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 30)}, new Reward (20), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.F2, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.BIGGER_EQUAL, 3)}, new Reward (30), Environment.FuelMission));
		missions.Add (new Mission (MissionId.D3, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 40)}, new Reward (30), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.F3, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.BIGGER_EQUAL, 4)}, new Reward (40), Environment.FuelMission));
		missions.Add (new Mission (MissionId.D4, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 50)}, new Reward (40), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D5, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 60)}, new Reward (50), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D6, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 70)}, new Reward (70), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D7, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 80)}, new Reward (80), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D8, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 90)}, new Reward (90), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D9, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 100)}, new Reward (100), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D10, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 110)}, new Reward (110), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D11, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 120)}, new Reward (120), Environment.ClassicMission));

		missions.Add (new Mission (MissionId.D12, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 150)}, new Reward (120), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D13, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 200)}, new Reward (130), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D14, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 250)}, new Reward (140), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D15, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 300)}, new Reward (150), Environment.ClassicMission));
		missions.Add (new Mission (MissionId.D16, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 350)}, new Reward (160), Environment.ClassicMission));

		missions.Add (new Mission (MissionId.DREN, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 350)}, new Reward (160), Environment.ClassicMission, true));
		missions.Add (new Mission (MissionId.FREN, new AchievQuery[] { }, new AchievQuery[] { new AchievQuery (SCORE_TYPE.FUEL_PICKED, SIGN.BIGGER_EQUAL, 15)}, new Reward (160), Environment.ClassicMission, true));

		int i = 1; 
		foreach (Mission mission in missions) {
			if (mission.IsRenewable() ||  !player.IsMissionDone(mission)){
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
