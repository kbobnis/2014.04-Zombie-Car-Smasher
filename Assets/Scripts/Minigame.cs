using UnityEngine;
using System.Collections.Generic;

public class Minigame : MonoBehaviour {

	public float CarSpeed;

	private int lastCarWasAt = 0;
	private int Distance =0;
	private bool IsGameOver = false;
	private string GameOverReason = "No reason";
	public GameObject Car;
	private Dictionary<int, GameObject> Streets = new Dictionary<int, GameObject>();

	public static Minigame Me;

	private GUIStyle bigFontLeft = new GUIStyle();

	// Use this for initialization
	void Start () {
		Me = this;
		bigFontLeft.fontSize = 25 * Screen.width / 1900;
		bigFontLeft.normal.textColor = new Color (255/255f, 255/255f, 255/255f);

		Sprite sprite = Resources.Load<Sprite>("Images/street");
		InGamePosition.tileH = sprite.bounds.size.y/1.3f;
		InGamePosition.tileW = sprite.bounds.size.x/3;
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

		Tile.WallChance = 0.05f;
		Tile.HoleChance = 0.08f;
		Tile.BuffFuelChance = 0.01f;
		Tile.BuffOilValue = 10;

		for(int i=-8; i < 8; i ++){
			Streets.Add(i, CreateStreet(i));
		}

		Car = new GameObject();
		Car.name = "car";
		Car tmp = Car.AddComponent<Car>();
		tmp.Prepare(0, 0, CarSpeed);

		Camera.main.GetComponent<FollowGM>().FollowWhom = Car;
		Camera.main.GetComponent<FollowGM>().Offset.y = -0.5f;

	}

	private GameObject CreateStreet(int inGameY){
		GameObject g = new GameObject(); 
		g.name = "street";
		Street street = g.AddComponent<Street>();
		street.Prepare(inGameY);
		return g;
	}

	public void GameOver(string reason){
		IsGameOver = true;
		Car.GetComponent<Speeder>().v = 0;
		GameOverReason = reason;
	}


	// Update is called once per frame
	void Update () {
		if (Car == null){
			return ;
		}
		if (Car.GetComponent<Fuel>().Amount <= 0){
			GameOver("No more fuel");
		}


		//car should be always in the middle of the road
		int carIsAt = Mathf.RoundToInt( Car.GetComponent<InGamePosition>().y);
		if (carIsAt != lastCarWasAt){

			Distance  = lastCarWasAt = carIsAt;
			int minStreet = int.MaxValue;
			int maxStreet = int.MinValue;
			foreach(KeyValuePair<int, GameObject> street in Streets){
				//check car collision with obstacles
				if (Mathf.Abs( Car.GetComponent<InGamePosition>().y - street.Value.GetComponent<InGamePosition>().y) < 0.5){
					Street tmp = street.Value.GetComponent<Street>();
					foreach(KeyValuePair<int, GameObject> tile in tmp.Tiles){
						if (tile.Value.GetComponent<InGamePosition>().x == Car.GetComponent<InGamePosition>().x && Car.GetComponent<InGamePosition>().z >= 0){
							tile.Value.GetComponent<Tile>().CarIsOn();
						}
				    }
				}

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
				Streets.Add(maxStreet+1, CreateStreet(maxStreet+1));
			}

		}

	}

	private void UnloadStreet(GameObject gm){
		gm.GetComponent<Street>().UnloadResources();
		Destroy(gm);
	}

	void OnGUI () {
		int oneThirdW = Screen.width/3;
		int oneThirdH = Screen.height/3;
		int oneTenthW = Screen.width/10;
		int oneTenthH = Screen.height/10;
		int twentyPercent = Screen.height / 5;
		if (Car != null && Car.GetComponent<InGamePosition>() != null){
			float carX = Car.GetComponent<InGamePosition>().x;

			if(carX >= 0 && GUI.Button(new Rect(0, Screen.height*(2/3f), oneThirdW, twentyPercent), "Left")){
				Car.GetComponent<InGamePosition>().x --;
			}
			if(carX <= 0 && GUI.Button(new Rect(Screen.width - oneThirdW, Screen.height*(2/3f), oneThirdW, twentyPercent), "Right")){
				Car.GetComponent<InGamePosition>().x ++;
			}
			if (Car.GetComponent<Jumper>().CanJump() && GUI.Button(new Rect(oneThirdW, Screen.height -twentyPercent, oneThirdW, twentyPercent), "Jump")){
				Car.GetComponent<Jumper>().Jump();
			}
		}
		GUI.Label (new Rect (oneTenthW, oneTenthH, Screen.width, Screen.height), "Distance: " + Distance);//, bigFontLeft);


		if (IsGameOver){
			if(GUI.Button(new Rect(oneThirdW, oneThirdH, oneThirdW, oneThirdH), GameOverReason + "\nPoints: "+Distance + "\n One more time")){
				PrepareRace();
			}
		}
	}

}
