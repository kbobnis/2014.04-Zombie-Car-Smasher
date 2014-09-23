using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using Facebook;

public class Minigame : MonoBehaviour {

	private int LastCarWasAt = 0;
	private bool Pressed = false;
	public int Distance =0;
	private string MissionInfo = "";
	private bool FanfarePlayed;
	private bool IsGameOver;
	public static Minigame Me;

	private Dictionary<int, Result[]> InGameAchievements = new Dictionary<int, Result[]> ();
	private AfterMinigameF AfterMinigame;
	private Mission Mission;
	public GameObject Car;
	public Dictionary<int, GameObject> Streets = new Dictionary<int, GameObject>();
	public PlayerState Player;
	private CarConfig CarConfig;

	public delegate void AfterMinigameF(Dictionary<int, Result[]> unlockResults, Result[] incremenentResult, string endGameReason, int distance, Mission mission, PlayerState player);

	public void PrepareRace(PlayerState player, AfterMinigameF afterMinigame, Mission mission, CarConfig chosenCar){
		if (Me != null) {
			Me.UnloadResources();
		}
		Me = this;
		AfterMinigame = afterMinigame;
		Mission = mission;
		Player = player;
		CarConfig = chosenCar;

		InGamePosition.tileH = SpriteManager.GetCar().height / 70f;
		InGamePosition.tileW = SpriteManager.GetCar().width / 70f;

		GameObject previousStreet = null;
		int carStartingStreet = 0;
		int clearStreets =0;
		int streetCount = 10;
		for(int i=-1; i < streetCount; i ++){
			bool noObstacles = clearStreets-streetCount-carStartingStreet<=Mission.Env.FirstClearStreets;
			GameObject thisStreet = CreateStreet(i, previousStreet, noObstacles, Mission.Env);
			Streets.Add(i, thisStreet);
			previousStreet = thisStreet;
			clearStreets ++;
		}
		Car = new GameObject();
		Car.name = "car";
		Car tmp = Car.AddComponent<Car>();

		tmp.Prepare(chosenCar, Streets);
		Destroy(Camera.main.camera.gameObject.GetComponent<FollowGM>());
		FollowGM fgm = Camera.main.gameObject.AddComponent<FollowGM> ();
		fgm.FollowWhom = Car;
		fgm.Offset.y = -0.5f;
		GetComponent<GoogleMobileAdsKProjekt> ().HideBanner ();

		GoogleAnalyticsKProjekt.LogScreenOnce(ANALYTICS_SCREENS.GAME);
	}

	void OnApplicationFocus(bool pauseStatus){
		GoogleAnalyticsKProjekt.LogIsActive(pauseStatus);
	}

	private GameObject CreateStreet(int inGameY, GameObject previousStreet, bool noObstacles, Environment env){
		GameObject g = new GameObject(); 
		g.name = "street";
		Street street = g.AddComponent<Street>();
		street.Prepare(inGameY, previousStreet, noObstacles, env);
		return g;
	}

	public void GameOver(string reason){
		IsGameOver = true;
		GoogleAnalyticsKProjekt.LogScreenOnce (ANALYTICS_SCREENS.FAIL);

		//results to unlock and increment achievements (we don't want to increment achievements several times for one ride)
		Result[] afterGameAchievements = new Result[]{
			new Result(SCORE_TYPE.DISTANCE, Distance), 
			new Result(SCORE_TYPE.FUEL_PICKED, Car.GetComponent<Car> ().FuelPickedUpThisGame), 
			new Result(SCORE_TYPE.FUEL_PICKED_WHEN_LOW, Car.GetComponent<Car> ().FuelPickedUpWhenLow),
			new Result(SCORE_TYPE.FUEL_PICKED_IN_ROW, Car.GetComponent<Car> ().FuelPickedUpInARow),
			new Result(SCORE_TYPE.TURNS, Car.GetComponent<Car> ().TurnsMade),
			new Result(SCORE_TYPE.COINS, Car.GetComponent<Car>().PickedUpCoins),
			new Result(SCORE_TYPE.SHIELDS_USED, Car.GetComponent<Car>().ShieldsUsed)
		};

		Player.RewardHim (Mission, InGameAchievements, afterGameAchievements);
		CarConfig.UpdateCar (afterGameAchievements);
		Player.Save ();
		Destroy (Car.GetComponent<Fuel> ());

		AfterMinigame (InGameAchievements, afterGameAchievements, reason, Distance, Mission, Player);
	}

	public void UnloadResources(){
		Speeder s = Car.gameObject.GetComponent<Speeder> ();
		Destroy (s);
		//remove all previous objects 
		Dictionary<int, GameObject> str = Streets;
		Streets = null;
		foreach(KeyValuePair<int, GameObject> tmp2 in str){
			UnloadStreet(tmp2.Value);
		}

		//remove car
		Destroy(Car);

		Car = null;
	}



	// Update is called once per frame
	void Update () {
		if (Car != null && Car.GetComponent<HurtTaker>() != null && Car.GetComponent<HurtTaker> ().HasLost) {
			Destroy(Car.GetComponent<HurtTaker>());
			GameOver(Car.GetComponent<HurtTaker>().LostReason);
		}

		if (Car == null) {
			return ;
		}

		if(Input.GetMouseButtonUp (0)){
			Pressed = false;
		}

		if ((Distance == 100 || Distance == 200) && !InGameAchievements.ContainsKey(Distance)) { //because there are several achievements which have to be made in the distance 100 or 200
			InGameAchievements.Add(Distance, new Result[]{
				new Result(SCORE_TYPE.TURNS, Car.GetComponent<Car>().TurnsMade),
				new Result(SCORE_TYPE.DISTANCE, Distance),
				new Result(SCORE_TYPE.FUEL_PICKED, Car.GetComponent<Car>().FuelPickedUpThisGame),
				new Result(SCORE_TYPE.FUEL_PICKED_IN_ROW, Car.GetComponent<Car>().FuelPickedUpInARow),
				new Result(SCORE_TYPE.FUEL_PICKED_WHEN_LOW, Car.GetComponent<Car>().FuelPickedUpWhenLow),
			});

		}

		//car should be always in the middle of the road
		int carIsAt = Mathf.RoundToInt( Car.GetComponent<InGamePosition>().y);
		if (carIsAt != LastCarWasAt){

			Distance  = LastCarWasAt = carIsAt;
			int minStreet = int.MaxValue;
			int maxStreet = int.MinValue;
			foreach(KeyValuePair<int, GameObject> street in Streets){

				if (street.Key < minStreet){
					minStreet = street.Key;
				}
				if (street.Key > maxStreet){
					maxStreet = street.Key;
				}
			}
	
			float streetsMiddle = (maxStreet - minStreet)/2 + minStreet;
			if (Mathf.Abs(streetsMiddle - carIsAt) > 1){

				UnloadStreet(Streets[minStreet]);
				Streets.Remove(minStreet);
				Streets.Add(maxStreet+1, CreateStreet(maxStreet+1, Streets[maxStreet], false, Mission.Env));
			}

			MissionInfo = null;
			//update mission query
			if (Mission.AfterGameReqs.Count > 0){
				
				int amountDone = Mission.GetAmountDone(new Result[]{
					new Result(SCORE_TYPE.TURNS, Car.GetComponent<Car>().TurnsMade),
					new Result(SCORE_TYPE.DISTANCE, Distance),
					new Result(SCORE_TYPE.FUEL_PICKED, Car.GetComponent<Car>().FuelPickedUpThisGame),
					new Result(SCORE_TYPE.FUEL_PICKED_IN_ROW, Car.GetComponent<Car>().FuelPickedUpInARow),
					new Result(SCORE_TYPE.FUEL_PICKED_WHEN_LOW, Car.GetComponent<Car>().FuelPickedUpWhenLow)
				});
				int amountFull = Mission.GetAmountFull();
				string amountType = Mission.GetAmountType().HumanName();

				if (amountFull != 0){
					MissionInfo = "" + amountDone + " / " + amountFull + " " + amountType;
					if (amountDone >= amountFull && !FanfarePlayed){
						PlaySingleSound.SpawnSound(Sounds.Fanfare, Camera.main.transform.position);
						FanfarePlayed = true;
					}
				}
			}
		}

	}

	private void UnloadStreet(GameObject gm){
		gm.GetComponent<Street>().UnloadResources();
		Destroy(gm);
	}

	void OnGUI () {

		if (!IsGameOver){
			string m = MissionInfo != null ? MissionInfo : "Distance: " + Distance;
			GUI.Label (new Rect (GuiHelper.oneTenthW/2, GuiHelper.oneTenthH/2, Screen.width, Screen.height), m, GuiHelper.SmallFontLeft);
		}

	}

}
