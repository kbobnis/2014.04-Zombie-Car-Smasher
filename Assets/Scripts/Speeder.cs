using UnityEngine;
using System.Collections.Generic;


public class Speeder : MonoBehaviour 
{
	public float v;
	public float RideCost;
	public bool DestroyWhenEmpty = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float rideCost = Time.deltaTime * RideCost;

		if (GetComponent<Fuel>().Amount > 0 && v > 0){
			GetComponent<InGamePosition>().y += v * Time.deltaTime;
			GetComponent<Fuel>().Amount -= rideCost;
		}

		if (GetComponent<Fuel>().Amount <= 0 && DestroyWhenEmpty){
			Destroy(gameObject);
		}

			foreach(KeyValuePair<int, GameObject> street in Minigame.Me.Streets){
				//check collision with obstacles
				if (Mathf.Abs(  gameObject.GetComponent<InGamePosition>().y - street.Value.GetComponent<InGamePosition>().y) < 0.5){
					Street tmp = street.Value.GetComponent<Street>();
					GameObject tile = tmp.Tiles[Mathf.RoundToInt( gameObject.GetComponent<InGamePosition>().x)];
					tile.GetComponent<Tile>().GMIsOn(gameObject);
				}
			}
	}

}


