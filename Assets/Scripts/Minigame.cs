using UnityEngine;
using System.Collections.Generic;

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
	private int Distance =0;
	private bool IsGameOver = false;
	private string GameOverReason = "No reason";
	public Dictionary<int, GameObject> Streets = new Dictionary<int, GameObject>();

	public static Minigame Me;

	private GUIStyle bigFontLeft = new GUIStyle();

	private bool Pressed = false;

	// Use this for initialization
	void Start () {
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
		Distance = 0;
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
		for(int i=-8; i < 8; i ++){
			bool noObstacles = clearStreets-8-carStartingStreet<=FirstClearStreets;
			GameObject thisStreet = CreateStreet(i, previousStreet, noObstacles);
			Streets.Add(i, thisStreet);
			previousStreet = thisStreet;
			clearStreets ++;
		}
		Car = new GameObject();
		Car.name = "car";
		Car tmp = Car.AddComponent<Car>();
		tmp.Prepare(0, carStartingStreet, StartingCarSpeed, EndingCarSpeed, ShooterProbability, JumperProbability, RideCost);

		Camera.main.GetComponent<FollowGM>().FollowWhom = Car;
		Camera.main.GetComponent<FollowGM>().Offset.y = -0.5f;

	}

	private GameObject CreateStreet(int inGameY, GameObject previousStreet, bool noObstacles=false){
		GameObject g = new GameObject(); 
		g.name = "street";
		Street street = g.AddComponent<Street>();
		street.Prepare(inGameY, previousStreet, noObstacles);
		return g;
	}

	public void GameOver(string reason){
		IsGameOver = true;
		Destroy (Car.GetComponent<Speeder> ());
		Destroy (Car.GetComponent<CarTurner> ());
		HighScores.AddScore (Distance);
		GameOverReason = reason;
	}


	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp (0)){
			Pressed = false;
		}


		if (Car == null){
			return ;
		}

		//car should be always in the middle of the road
		int carIsAt = Mathf.RoundToInt( Car.GetComponent<InGamePosition>().y);
		if (carIsAt != lastCarWasAt){

			Distance  = lastCarWasAt = carIsAt;
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
			List<int> top = HighScores.GetTopScores (5);
			string topScores = "HighScores: \n";
			int i = 0; 
			foreach (int s in top) {
					topScores += "Score " + ++i + ". " + s + "\n";
			}

			GuiHelper.DrawText (topScores, GuiHelper.CustomButton, 0.1, 0.1, 0.8, 0.4);

			if (GUI.Button (new Rect (GuiHelper.PercentW (0.1), GuiHelper.PercentH (0.6), GuiHelper.PercentW (0.8), GuiHelper.PercentH (0.3)), "Actual score: " + Distance + "\n" + GameOverReason + "\n One more time", GuiHelper.CustomButton)) {
					PrepareRace ();
			}
		} else {
			//GUI.DrawTexture(new Rect( GuiHelper.PercentW(0.03), GuiHelper.PercentH(0.034) , GuiHelper.PercentW(0.5), GuiHelper.PercentH(0.1) ), SpriteManager.GetDistanceBorder());

			GUI.Label (new Rect (GuiHelper.oneTenthW/2, GuiHelper.oneTenthH/2, Screen.width, Screen.height), "Distance: " + Distance, GuiHelper.SmallFont);
		}
	}

}
