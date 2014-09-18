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
		
		PlayerState state = Game.Me.PlayerState;

		GuiHelper.DrawBackground (delegate() {
			gameObject.AddComponent<ScreenAdvModeStart> ();
			Destroy (this);
		});

		Mission mission1 = new Mission(new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 30)}, new Reward(10, 1), "Distance 1");
		Mission mission2 = new Mission(new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 50)}, new Reward(20, 2), "Distance 2");
		Mission mission3 = new Mission(new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 70)}, new Reward(30, 2), "Distance 3");
		Mission mission4 = new Mission(new AchievQuery[] { }, new AchievQuery[] { new AchievQuery(SCORE_TYPE.DISTANCE, SIGN.BIGGER_EQUAL, 90)}, new Reward(40, 2), "Distance 4");

		Mission[] missions = new Mission[]{ mission1, mission2, mission3, mission4 };

		int i = 1; 
		foreach (Mission mission in missions) {
			GuiHelper.DrawMissionLabel (i++, mission.Title, mission.Description, "Win "+mission.Reward.Description, delegate() {
				Minigame m = gameObject.AddComponent<Minigame>();
				m.PrepareRace(Game.Me.PlayerState.CarConfig, ScreenAfterMinigameAdv.PrepareScreen, mission);
				Destroy(this);
			});
			//we show only four missions at a time
			if (i > 4){
				break;
			}
		}
	}
}
