using UnityEngine;
using System.Collections;

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

		Mission mission1 = new Mission("dist1", new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 30)}, new Reward(10, 1), "Distance 1", Environment.ClassicMission);
		Mission mission2 = new Mission("dist2", new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 50)}, new Reward(20, 2), "Distance 2", Environment.ClassicMission);
		Mission mission3 = new Mission("dist3", new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 70)}, new Reward(30, 2), "Distance 3", Environment.ClassicMission);
		Mission mission4 = new Mission("dist4", new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 90)}, new Reward(40, 2), "Distance 4", Environment.ClassicMission);
		Mission mission5 = new Mission("dist5", new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 110)}, new Reward(50, 2), "Distance 5", Environment.ClassicMission);
		Mission mission6 = new Mission("dist6", new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 130)}, new Reward(60, 2), "Distance 6", Environment.ClassicMission);
		Mission mission7 = new Mission("dist7", new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 150)}, new Reward(70, 2), "Distance 7", Environment.ClassicMission);

		Mission[] missions = new Mission[]{ mission1, mission2, mission3, mission4, mission5, mission6, mission7 };

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
