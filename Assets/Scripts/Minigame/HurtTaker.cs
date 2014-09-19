using UnityEngine;
using System.Collections;

public class HurtTaker : MonoBehaviour {

	public bool HasLost;
	public string LostReason;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeHurt(Tile fromWhat){

		ShieldCompo sc = GetComponent<ShieldCompo> ();
		if (sc == null || !sc.TakeThis(fromWhat)){
			Die (fromWhat);
		}

	}

	private void Die(Tile fromWhat){
		if (fromWhat.TileContent == TileContent.WALL || fromWhat.TileContent == TileContent.HOLE ) {
			PlaySingleSound.SpawnSound (Sounds.CartonImpact, Camera.main.transform.position);	
		} 
		HasLost = true;
	}

	public void OutOfOil(){
		PlaySingleSound.SpawnSound(Sounds.NoMoreFuel, Camera.main.transform.position, 0.4f);
		Die (null);
		HasLost = true;
	}
}
