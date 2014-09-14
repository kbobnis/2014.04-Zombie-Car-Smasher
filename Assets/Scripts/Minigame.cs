using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using Facebook;

public class Minigame : MonoBehaviour {

	public float StartingCarSpeed;
	public float EndingCarSpeed;
	public float ShooterProbability;
	public float JumperProbability;
	public float WallChance = 0.05f;
	public float HoleChance = 0.08f;
	public float BuffFuelChance = 0.01f;
	public float BuffOilValue = 10;
	public int FirstClearStreets = 6;
	public float RideCost = 5f;
	public float TurnSpeed = 7f;
	public static float CactusChance = 0.05f;
	public static float LinesChance = 0.01f;

	public GameObject Car;

	private int lastCarWasAt = 0;
	private int _Distance =0;
	private bool IsGameOver = false;
	private string GameOverReason = "No reason";
	public Dictionary<int, GameObject> Streets = new Dictionary<int, GameObject>();

	public static Minigame Me;

	private GUIStyle bigFontLeft = new GUIStyle();

	private bool Pressed = false;

	public static string FELL_INTO_HOLE = "Fell into hole";
	public static string CRASHED_INTO_WALL = "Crashed into brick";
	public static string OUT_OF_OIL = "Out of fuel";

	public const string SCREEN_MAIN = "Screen main";
	public const string SCREEN_GAME = "Screen game";
	public const string SCREEN_FAIL = "Screen fail";

	private int HowManyInTopScores = 4;
	private bool ShowNewHighScoreScreen = false;
	private bool ShowRideInfoScreen = false;

	private Dictionary<int, Result[]> InGameAchievements = new Dictionary<int, Result[]> ();

	public int Distance{
		get { return _Distance;}
	}
	// Use this for initialization
	void Start () {
		Sounds.LoadSounds ();
		Me = this;
		bigFontLeft.fontSize = 25 * Screen.width / 1900;
		bigFontLeft.normal.textColor = new Color (255/255f, 255/255f, 255/255f);

		InGamePosition.tileH = SpriteManager.GetCar().bounds.size.y*2;
		InGamePosition.tileW = SpriteManager.GetCar().bounds.size.x*2.5f;
		CarTurner.TurnSpeed = TurnSpeed;
	}

	public void UnloadResources(){
		//remove all previous objects 
		foreach(KeyValuePair<int, GameObject> tmp2 in Streets){
			UnloadStreet(tmp2.Value);
		}
		Streets.Clear();
		//remove car
		Destroy(Car);
		Car = null;
		IsGameOver = false;
		lastCarWasAt = 0;
		_Distance = 0;
	}

	public void PrepareRace(){
		UnloadResources();

		Tile.WallChance = WallChance;
		Tile.HoleChance = HoleChance;
		Tile.BuffFuelChance = BuffFuelChance;
		Tile.BuffOilValue = BuffOilValue;

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
		List<int> topScores = HighScores.GetTopScores (1);
		float bestScore =  topScores.Count==0?0:topScores[0];
		float carStartingSpeed = StartingCarSpeed;
		if (bestScore < 200){
			carStartingSpeed = 3 + bestScore/200 * 2;
		}
		tmp.Prepare(0, carStartingStreet, carStartingSpeed, EndingCarSpeed, ShooterProbability, JumperProbability, RideCost);

		Destroy (Camera.main.gameObject.GetComponent<FollowGM> ());
		FollowGM fgm = Camera.main.gameObject.AddComponent<FollowGM> ();
		fgm.FollowWhom = Car;
		fgm.Offset.y = -0.5f;
		GetComponent<GoogleMobileAdsKProjekt> ().HideBanner ();

		GoogleAnalyticsKProjekt.LogScreenOnce(Minigame.SCREEN_GAME);

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

	private bool IsShowBanner(){
		return HighScores.GetTopScoresCount() > HowManyInTopScores;
	}

	public void GameOver(string reason){
		if (reason == Minigame.CRASHED_INTO_WALL || reason == Minigame.FELL_INTO_HOLE) {
			PlaySingleSound.SpawnSound (Sounds.CartonImpact, Car.transform.position);
		} else if (reason == Minigame.OUT_OF_OIL){
			PlaySingleSound.SpawnSound(Sounds.NoMoreFuel, Car.transform.position, 0.4f);
		}

		IsGameOver = true;

		Destroy (Car.GetComponent<Speeder> ());
		Destroy (Car.GetComponent<CarTurner> ());
		Destroy (Car.GetComponent<Fuel> ());
		HighScores.AddScore (_Distance);
		int place = HighScores.GetPlaceFor (_Distance);
		if (place == 1) {
			PlaySingleSound.SpawnSound(Sounds.Fanfare, Camera.main.gameObject.transform.position, 0.2f);
			if (_Distance > 50){
				ShowNewHighScoreScreen = true;
			}
		}

		if (IsShowBanner()) {
			GetComponent<GoogleMobileAdsKProjekt> ().ShowBanner ();
		}

		GameOverReason = reason;
		GoogleAnalyticsKProjekt.LogScreenOnce (Minigame.SCREEN_FAIL);

		//if someone had internet issues before, at least we can update high score with his best distance and come unlockable achievements
		List<int> topScores = HighScores.GetTopScores (1);
		int bestDistance = topScores.Count > 0 ? topScores [0] : 0;
		InGameAchievements.Add(bestDistance, new Result[]{new Result (SCORE_TYPE.DISTANCE, bestDistance)});

		//result to unlock achievement
		foreach (KeyValuePair<int, Result[]> result in InGameAchievements) {
			CarSmasherSocial.UnlockAchievements(result.Value);
		}
		InGameAchievements.Clear ();

		//results to unlock and increment achievements (we don't want to increment achievements several times for one ride)
		CarSmasherSocial.UpdateAchievements(new Result[]{
			new Result(SCORE_TYPE.DISTANCE, Distance), 
			new Result(SCORE_TYPE.FUEL_PICKED, Car.GetComponent<Car> ().FuelPickedUpThisGame), 
			new Result(SCORE_TYPE.FUEL_PICKED_WHEN_LOW, Car.GetComponent<Car> ().FuelPickedUpWhenLow),
			new Result(SCORE_TYPE.FUEL_PICKED_IN_ROW, Car.GetComponent<Car> ().FuelPickedUpInARow),
			new Result(SCORE_TYPE.TURNS, Car.GetComponent<Car> ().TurnsMade)
		});

	}


	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp (0)){
			Pressed = false;
		}


		if (Car == null){
			return ;
		}

		if ((Distance == 100 || Distance == 200) && !InGameAchievements.ContainsKey(Distance)) { //because there are several achievements which have to be made in the distance 100 or 200
			InGameAchievements.Add(Distance, new Result[]{
				new Result(SCORE_TYPE.TURNS, Car.GetComponent<Car>().TurnsMade),
				new Result(SCORE_TYPE.DISTANCE, Distance),
				new Result(SCORE_TYPE.FUEL_PICKED, Car.GetComponent<Car>().FuelPickedUpThisGame),
				new Result(SCORE_TYPE.FUEL_PICKED_IN_ROW, Car.GetComponent<Car>().FuelPickedUpInARow),
				new Result(SCORE_TYPE.FUEL_PICKED_WHEN_LOW, Car.GetComponent<Car>().FuelPickedUpWhenLow)
			});

		}

		//car should be always in the middle of the road
		int carIsAt = Mathf.RoundToInt( Car.GetComponent<InGamePosition>().y);
		if (carIsAt != lastCarWasAt){

			_Distance  = lastCarWasAt = carIsAt;
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

		if (IsGameOver) {
			GuiHelper.DrawElement("Images/popupWindow", 0.01, 0.03, 0.98, 1);

			if (ShowNewHighScoreScreen){

				GuiHelper.DrawText ("New High Score!", GuiHelper.SmallFont, 0.1, 0.08, 0.8, 0.07);
				GuiHelper.DrawText ("You just beat your high score with distance "+_Distance+". \n\n Like to tell your friends about it?", GuiHelper.SmallFont, 0.1, 0.2, 0.75, 0.07);
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.3), GuiHelper.PercentH(0.84), GuiHelper.PercentW(0.4), GuiHelper.PercentH(0.15)), SpriteManager.GetBackButton(), GuiHelper.CustomButton)){
					ShowNewHighScoreScreen = false;
				}
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.2), GuiHelper.PercentH(0.70), GuiHelper.PercentW(0.6), GuiHelper.PercentH(0.11)), SpriteManager.GetFbShareButton(), GuiHelper.CustomButton)){
					CarSmasherSocial.FB.FeedHighScore(_Distance);
					ShowNewHighScoreScreen = false;
				}

			} else if (ShowRideInfoScreen){
				GuiHelper.DrawText (GameOverReason, GuiHelper.SmallFont, 0.1, 0.08, 0.8, 0.07);

				int fuelPickedUp = Car.GetComponent<Car> ().FuelPickedUpThisGame;
				int fuelPickedUpInARow = Car.GetComponent<Car> ().FuelPickedUpInARow;
				int fuelPickedUpWhenLow = Car.GetComponent<Car> ().FuelPickedUpWhenLow;
				int turns = Car.GetComponent<Car> ().TurnsMade;


				GuiHelper.DrawText ("Collect oil drops to replenish fuel tank. Omit obstacles.\n"+
				                    "\nDistance made: "+Distance+
				                    "\nTurns made: "+turns+
				                    "\nFuel picked up: "+fuelPickedUp+" "+
				                    "(in a row: "+fuelPickedUpInARow+") "+
				                    "(when low: "+fuelPickedUpWhenLow+") "
				                    , GuiHelper.SmallFont, 0.1, 0.2, 0.75, 0.07);
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.3), GuiHelper.PercentH(0.84), GuiHelper.PercentW(0.4), GuiHelper.PercentH(0.15)), SpriteManager.GetBackButton(), GuiHelper.CustomButton)){
					ShowRideInfoScreen = false;
				}

			} else {
				GuiHelper.DrawText (GameOverReason, GuiHelper.SmallFontLeft, 0.1, 0.08, 0.8, 0.07);
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.75), GuiHelper.PercentH(0.08), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.1)), "help", GuiHelper.SmallFont)){
					ShowRideInfoScreen = true;
				}
				DrawTopScores(0.2f);
				
				Texture achievements = SpriteManager.GetAchievements();
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.07), GuiHelper.PercentH(0.6), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), achievements, GuiHelper.CustomButton)){
					CarSmasherSocial.ShowAchievements();
				}
				
				Texture leaderBoard = SpriteManager.GetLeaderboard();
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.28), GuiHelper.PercentH(0.56), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), leaderBoard, GuiHelper.CustomButton)){
					CarSmasherSocial.ShowLeaderBoard();
				}
				
				Texture soundButton = Sounds.IsMuted()?SpriteManager.GetSoundButtonMuted():SpriteManager.GetSoundButton();
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.52), GuiHelper.PercentH(0.56), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), soundButton, GuiHelper.CustomButton)){
					Sounds.Mute(!Sounds.IsMuted());
				}

				Texture fbButton = SpriteManager.GetFbIcon();
				if (GUI.Button(new Rect(GuiHelper.PercentW(0.73), GuiHelper.PercentH(0.6), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.14)), fbButton, GuiHelper.CustomButton)){
					CarSmasherSocial.FB.Like();
				}

				if (GUI.Button(new Rect(GuiHelper.PercentW(0.27), GuiHelper.PercentH(0.67), GuiHelper.PercentW(0.45), GuiHelper.PercentH(0.3)), SpriteManager.GetStartButton(), GuiHelper.CustomButton)){
					PrepareRace ();
				}
			}

		} else {

			if (GuiHelper.SmallFont != null){
				GUI.Label (new Rect (GuiHelper.oneTenthW/2, GuiHelper.oneTenthH/2, Screen.width, Screen.height), "Distance: " + _Distance, GuiHelper.SmallFontLeft);
			}
		}
	}

	private bool IsBetterDrawDrivingTips(){
		List<int> scores = HighScores.GetTopScores (1);

		return scores.Count > 0 && scores[0] < 200;
	}

	private void DrawTopScores(float y){
		int place = HighScores.GetPlaceFor (_Distance);
		bool isInTop = place <= HowManyInTopScores;
		List<int> top = HighScores.GetTopScores (HowManyInTopScores);
		bool yourWasSet = false;
		string topScores = "Best Distances:";
		for(int i=0; i < HowManyInTopScores && top.Count > i; i++){
			
			if (i < HowManyInTopScores){
				topScores += "\n";
			}
			if (!yourWasSet && i == HowManyInTopScores-1 && !isInTop){
				break;
			}
			
			int s = top[i];
			if (s == _Distance && !yourWasSet){
				yourWasSet = true;
				topScores += "Top " + (i+1) + ". " + s + " (Now)";
			} else {
				topScores += "Top " + (i+1) + ". " + s + "";
			}
			
			
		}
		
		if (!yourWasSet){
			topScores += "... Now: " + _Distance ;
		}
		
		GuiHelper.DrawText (topScores, GuiHelper.SmallFontLeft, 0.1, y, 0.8, 0.41);

		Texture fbButton = SpriteManager.GetFbShareButton();
		int bestScore = top.Count>0?top[0]:0;
		if (GUI.Button(new Rect(GuiHelper.PercentW(0.545), GuiHelper.PercentH(0.215), GuiHelper.PercentW(0.35), GuiHelper.PercentH(0.18)), fbButton, GuiHelper.CustomButton)){
			CarSmasherSocial.FB.FeedHighScore(bestScore);
		}
	}

}
