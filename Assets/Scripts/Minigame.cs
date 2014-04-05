using UnityEngine;
using System.Collections.Generic;

public class Minigame : MonoBehaviour {

	private float StreetHeight;

	private GameObject Car;
	private List<GameObject> Streets = new List<GameObject>();
	private GameObject FurthestStreet;

	// Use this for initialization
	void Start () {
		StreetHeight = Resources.Load<Sprite>("Images/street").bounds.size.y * 0.75f;
	}
	
	// Update is called once per frame
	void Update () {
		int removedCount = RemoveStreetsOutsideCamera();
		AddStreets(removedCount);
	}

	private void AddStreets(int howMany)
	{
		//adding on top 
		if (FurthestStreet != null){
			for(int i=0; i < howMany; i++){
				GameObject tmp = CreateStreet(FurthestStreet.transform.position.y + StreetHeight);
				Streets.Add(tmp);
				FurthestStreet = tmp;
			}
		}
	}

	public void StartRace(){

		//y on screen is Screen.height on top and 0 on bottom.
		for(float yOnScreen = 2*Screen.height; yOnScreen > 0; ){
			float yInWorld = Camera.main.ScreenToWorldPoint(new Vector3(0, yOnScreen, Mathf.Abs( Camera.main.transform.position.z))).y;
			GameObject tmp = CreateStreet(yInWorld);
			if (FurthestStreet == null){
				FurthestStreet = tmp;
			}
			float streetTop = tmp.transform.position.y;
			Vector3 point = new Vector3(0, streetTop - StreetHeight, 0);
			float screenPointStreetBottom = Camera.main.WorldToScreenPoint(point).y;
			yOnScreen = screenPointStreetBottom;

			Streets.Add(tmp);
		}

		Car = new GameObject();
		SpriteRenderer carRenderer = Car.AddComponent<SpriteRenderer>();
		carRenderer.sprite = Resources.Load<Sprite>("Images/car");
		carRenderer.sortingLayerName = "Layer2";

		Vector3 tmp2 = Car.transform.position;
		float scale = StreetHeight / carRenderer.bounds.size.y ;
		Car.transform.localScale = new Vector3(scale, scale);
		tmp2.x -= carRenderer.bounds.size.x/2;
		Car.transform.position = tmp2;

		Speeder speeder = Car.AddComponent<Speeder>();
		speeder.v = 5;

		Car.AddComponent<InGamePosition>();
		Camera.main.GetComponent<FollowGM>().FollowWhom = Car;
	}

	private GameObject CreateStreet(float y){
		GameObject g = new GameObject(); 
		SpriteRenderer streetRenderer = g.AddComponent<SpriteRenderer>();
		streetRenderer.sprite = Resources.Load<Sprite>("Images/street");
		streetRenderer.sortingLayerName = "Layer1";
		Vector3 tmpPos = g.transform.localPosition;
		tmpPos.y = y;
		float scale = StreetHeight / streetRenderer.bounds.size.y;
		tmpPos.x = - streetRenderer.bounds.size.x * scale /2;
		g.transform.localPosition = tmpPos;
		g.transform.localScale = new Vector3(scale, scale);

		g.AddComponent<InGamePosition>();
		return g;
    }
    
    public int RemoveStreetsOutsideCamera(){
		int removedCount = 0;

		for(int i=Streets.Count-1; i >= 0; i--){
			GameObject street = Streets[i];

			float streetTop = Screen.height - Camera.main.WorldToScreenPoint(street.transform.position).y;
			float screenHeight = Screen.height;

			if ( streetTop > screenHeight){
				//if you remove one, you have to add one
				removedCount ++;
				if (street == FurthestStreet){
					throw new UnityException("You seriously removed furthest street? Why?");
				}
				Destroy(street);
				Streets.RemoveAt(i);

			}
		}
		return removedCount;
	}

}
