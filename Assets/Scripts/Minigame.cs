using UnityEngine;
using System.Collections.Generic;

public class Minigame : MonoBehaviour {

	private float CarSpeed = 2;

	private int lastCarWasAt = 0;
	private GameObject Car;
	private Dictionary<int, GameObject> Streets = new Dictionary<int, GameObject>();
	private GameObject FurthestStreet;

	// Use this for initialization
	void Start () {
		Sprite sprite = Resources.Load<Sprite>("Images/street");
		InGamePosition.tileH = sprite.bounds.size.y/1.3f;
		InGamePosition.tileW = sprite.bounds.size.x/3;


	}

	public void PrepareRace(){
		for(int i=0; i < 16; i ++){
			Streets.Add(i, CreateStreet(i));
		}
		
		Car = CreateCar(0, 8, CarSpeed);
		Camera.main.GetComponent<FollowGM>().FollowWhom = Car;
		Camera.main.GetComponent<FollowGM>().Offset.y = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Car == null){
			return ;
		}
		//car should be always in the middle of the road
		int carIsAt = Mathf.RoundToInt( Car.GetComponent<InGamePosition>().y);
		if (carIsAt != lastCarWasAt){
			lastCarWasAt = carIsAt;
			int minStreet = 100;
			int maxStreet = -100;
			foreach(KeyValuePair<int, GameObject> street in Streets){
				if (street.Key < minStreet){
					minStreet = street.Key;
				}
				if (street.Key > maxStreet){
					maxStreet = street.Key;
				}
			}
	
			float streetsMiddle = (maxStreet - minStreet)/2 + minStreet;
			//Debug.Log("car is at:  " + carIsAt +", middle: " + streetsMiddle + ", min: " + minStreet+ ", max: " + maxStreet);
			if (Mathf.Abs(streetsMiddle - carIsAt) > 1){
				Destroy(Streets[minStreet]);
				Streets.Remove(minStreet);
				Streets.Add(maxStreet+1, CreateStreet(maxStreet+1));
			}
		}

	}

	void OnGUI () {
		int oneThirdW = Screen.width/3;
		int twentyPercent = Screen.height / 5;
		if (Car != null && Car.GetComponent<InGamePosition>() != null){
			float carX = Car.GetComponent<InGamePosition>().x;

			if(carX >= 0 && GUI.Button(new Rect(0, Screen.height*(2/3f), oneThirdW, twentyPercent), "Left")){
				Car.GetComponent<InGamePosition>().x --;
			}
			if(carX <= 0 && GUI.Button(new Rect(Screen.width - oneThirdW, Screen.height*(2/3f), oneThirdW, twentyPercent), "Right")){
				Car.GetComponent<InGamePosition>().x ++;
			}
			GUI.Button(new Rect(oneThirdW, Screen.height -twentyPercent, oneThirdW, twentyPercent), "Jump");
		}
	}

	private GameObject CreateCar(float inGameX, float inGameY, float speed){
		GameObject car = new GameObject();
		SpriteRenderer carRenderer = car.AddComponent<SpriteRenderer>();
		carRenderer.sprite = Resources.Load<Sprite>("Images/car");
		carRenderer.sortingLayerName = "Layer2";

		float scale = InGamePosition.tileH / carRenderer.bounds.size.y ;
		car.transform.localScale = new Vector3(scale, scale);

		Speeder speeder = car.AddComponent<Speeder>();
		speeder.v = speed;

		InGamePosition tmp = car.AddComponent<InGamePosition>();
		tmp.x = inGameX;
		tmp.y = inGameY;
		return car;
	}

	private GameObject CreateStreet(float inGameY){
		GameObject g = new GameObject(); 
		SpriteRenderer streetRenderer = g.AddComponent<SpriteRenderer>();
		streetRenderer.sprite = Resources.Load<Sprite>("Images/street");
		streetRenderer.sortingLayerName = "Layer1";

		Vector3 size = streetRenderer.sprite.bounds.size;
		g.transform.localScale = new Vector3(InGamePosition.tileW * 3 / size.x, InGamePosition.tileH  / size.y);

		InGamePosition tmp = g.AddComponent<InGamePosition>();
		tmp.x = 0;
		tmp.y = inGameY;
		return g;
    }
    
}
