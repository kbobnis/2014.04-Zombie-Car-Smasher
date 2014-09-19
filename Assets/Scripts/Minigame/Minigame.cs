using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using Facebook;

public class Minigame : MonoBehaviour {

	private int LastCarWasAt = 0;
	private bool Pressed = false;
	private Dictionary<int, Result[]> InGameAchievements = new Dictionary<int, Result[]> ();
	private AfterMinigameF AfterMinigame;
	private Mission Mission;

	public float WallChance = 0.12f;
	public float HoleChance = 0.12f;
	public float BuffFuelChance = 0.037f;

	public int FirstClearStreets = 5;

	public GameObject Car;
	public Dictionary<int, GameObject> Streets = new Dictionary<int, GameObject>();
	public int Distance =0;
	

	public static float BuffOilValue = 10;
	public static float CactusChance = 0.05f;
	public static float LinesChance = 0.01f;

	public const string SCREEN_MAIN = "Screen main";
	public const string SCREEN_GAME = "Screen game";
	public const string SCREEN_FAIL = "Screen fail";

	public delegate void AfterMinigameF(Dictionary<int, Result[]> unlockResults, Result[] incremenentResult, string endGameReason, int distance, Mission mission);

	public void PrepareRace(CarConfig carConfig, AfterMinigameF afterMinigame, Mission mission){
		AfterMinigame = afterMinigame;
		Mission = mission;

		Tile.WallChance = WallChance;
		Tile.HoleChance = HoleChance;
		Tile.BuffFuelChance = BuffFuelChance;
		InGamePosition.tileH = SpriteManager.GetCar().height / 70f;
		InGamePosition.tileW = SpriteManager.GetCar().width / 70f;

		GameObject previousStreet = null;
		int carStartingStreet = 0;
		int clearStreets =0;
		int streetCount = 10;
		for(int i=-1; i < streetCount; i ++){
			bool noObstacles = clearStreets-streetCount-carStartingStreet<=FirstClearStreets;
			GameObject thisStreet = CreateStreet(i, previousStreet, noObstacles);
			Streets.Add(i, thisStreet);
			previousStreet = thisStreet;
			clearStreets ++;
		}
		Car = new GameObject();
		Car.name = "car";
		Car tmp = Car.AddComponent<Car>();

		tmp.Prepare(carConfig, Streets);
		Destroy(Camera.main.camera.gameObject.GetComponent<FollowGM>());
		FollowGM fgm = Camera.main.gameObject.AddComponent<FollowGM> ();
		fgm.FollowWhom = Car;
		fgm.Offset.y = -0.5f;
		GetComponent<GoogleMobileAdsKProjekt> ().HideBanner ();

		GoogleAnalyticsKProjekt.LogScreenOnce(Minigame.SCREEN_GAME);
		gameObject.GetComponent<TopInfoBar> ().enabled = false;

	}

	void OnApplicationFocus(bool pauseStatus){
		GoogleAnalyticsKProjekt.LogIsActive(pauseStatus);
	}

	private GameObject CreateStreet(int inGameY, GameObject previousStreet, bool noObstacles=false){
		GameObject g = new GameObject(); 
		g.name = "street";
		Street street = g.AddComponent<Street>();
		street.Prepare(inGameY, previousStreet, noObstacles);
		return g;
	}

	public void GameOver(string reason){
		GetComponent<TopInfoBar> ().enabled = true;

		GoogleAnalyticsKProjekt.LogScreenOnce (Minigame.SCREEN_FAIL);

		//results to unlock and increment achievements (we don't want to increment achievements several times for one ride)
		Result[] afterGameAchievements = new Result[]{
			new Result(SCORE_TYPE.DISTANCE, Distance), 
			new Result(SCORE_TYPE.FUEL_PICKED, Car.GetComponent<Car> ().FuelPickedUpThisGame), 
			new Result(SCORE_TYPE.FUEL_PICKED_WHEN_LOW, Car.GetComponent<Car> ().FuelPickedUpWhenLow),
			new Result(SCORE_TYPE.FUEL_PICKED_IN_ROW, Car.GetComponent<Car> ().FuelPickedUpInARow),
			new Result(SCORE_TYPE.TURNS, Car.GetComponent<Car> ().TurnsMade),
			new Result(SCORE_TYPE.COINS, Car.GetComponent<Car>().PickedUpCoins)
		};
	
		AfterMinigame (InGameAchievements, afterGameAchievements, reason, Distance, Mission);
		UnloadResources ();
		Destroy (this);
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
		if (Car.GetComponent<HurtTaker> ().HasLost) {
			GameOver(Car.GetComponent<HurtTaker>().LostReason);
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit(); 
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
				Streets.Add(maxStreet+1, CreateStreet(maxStreet+1, Streets[maxStreet]));
			}

		}

	}

	private void UnloadStreet(GameObject gm){
		gm.GetComponent<Street>().UnloadResources();
		Destroy(gm);
	}

	void OnGUI () {
		GUI.Label (new Rect (GuiHelper.oneTenthW/2, GuiHelper.oneTenthH/2, Screen.width, Screen.height), "Distance: " + Distance, GuiHelper.SmallFontLeft);
	}

}
