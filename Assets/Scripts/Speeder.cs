using UnityEngine;
using System.Collections.Generic;


public class Speeder : MonoBehaviour 
{
	public float v;
	public float RideCost;
	public bool DestroyWhenEmpty = false;


	private float LastDistance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (LastDistance == 0){
			LastDistance = GetComponent<InGamePosition>().y;
		}

		if (GetComponent<Fuel>().Amount > 0 && v > 0){
			GetComponent<InGamePosition>().y += v * Time.deltaTime;
			GetComponent<Fuel>().Amount -= (GetComponent<InGamePosition>().y - LastDistance) * RideCost;
			LastDistance  = GetComponent<InGamePosition>().y;
		}

		if (GetComponent<Fuel>().Amount <= 0 && DestroyWhenEmpty){
			Destroy(gameObject);
		}

			foreach(KeyValuePair<int, GameObject> street in Minigame.Me.Streets){
				//check collision with obstacles
				if (Mathf.Abs(  gameObject.GetComponent<InGamePosition>().y - street.Value.GetComponent<InGamePosition>().y) < 0.15f){
					Street tmp = street.Value.GetComponent<Street>();
					GameObject tile = tmp.Tiles[Mathf.RoundToInt( gameObject.GetComponent<InGamePosition>().x)];
					tile.GetComponent<Tile>().GMIsOn(gameObject);
				}
			}
	}

}


