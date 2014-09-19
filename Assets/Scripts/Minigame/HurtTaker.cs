using UnityEngine;
using System.Collections;

public class HurtTaker : MonoBehaviour {

	public const string CRASHED_INTO_WALL = "crashedIntoWall";
	public const string FELL_INTO_HOLE = "fellIntoHole";
	public const string OUT_OF_OIL = "outOfOil";

	public bool HasLost;
	public string LostReason;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeHurt(string reason){

		if (reason == CRASHED_INTO_WALL || reason == FELL_INTO_HOLE) {
			PlaySingleSound.SpawnSound (Sounds.CartonImpact, Camera.main.transform.position);	
		} else if (reason == OUT_OF_OIL){
			PlaySingleSound.SpawnSound(Sounds.NoMoreFuel, Camera.main.transform.position, 0.4f);
		}
		HasLost = true;
		LostReason = reason;
	}
}
