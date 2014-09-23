using UnityEngine;
using System.Collections.Generic;


public class Speeder : MonoBehaviour 
{
	private AudioSource DriveSound;
	private float Distortion = 0.1f;
	private float LastY;

	public float _v;
	public float RideCost;
	public float Decelerate = 0.3f;

	public float v {
		set{
			if (value > 0 && DriveSound != null && !DriveSound.isPlaying){
				//audio.Play();
			}
			_v = value;
		}
		get{ return _v; }
	}

	public void Prepare(CarStatistic combustion, float startingCarSpeed){
		v = startingCarSpeed;
		RideCost = combustion.Value;

	}

	void Start () {
		DriveSound = gameObject.AddComponent<AudioSource> ();
		DriveSound.clip = Sounds.CarDrive;
		DriveSound.loop = true;
	}
	
	void FixedUpdate () {
		if (LastY == 0){
			LastY = GetComponent<InGamePosition>().y;
		}


		if (v > 0.5f) {
			if (GetComponent<Fuel> ().HasEnoughFuel ()) {
				float distort = GetComponent<CarTurner>().IsTurning()?0: Random.value * Distortion;
				GetComponent<InGamePosition> ().y += v * Time.deltaTime * (1 + distort);
				GetComponent<Fuel> ().ChargeForDistance (GetComponent<InGamePosition> ().y - LastY, RideCost);

				if ( Mathf.FloorToInt( GetComponent<InGamePosition> ().y) != Mathf.FloorToInt( LastY)) {
					GetComponent<Car>().JustMovedToAnotherTile(Mathf.FloorToInt(GetComponent<InGamePosition>().y));
				}

				LastY = GetComponent<InGamePosition> ().y;
			}
		}
		if (GetComponent<Car>().Streets != null){
			foreach (KeyValuePair<int, GameObject> street in GetComponent<Car>().Streets) {
				if (Mathf.Abs (gameObject.GetComponent<InGamePosition> ().y - street.Value.GetComponent<InGamePosition> ().y) < 0.15f) {
					Street tmp = street.Value.GetComponent<Street> ();
					GameObject tile = tmp.Tiles [Mathf.RoundToInt (gameObject.GetComponent<InGamePosition> ().x)];
					GMIsOn (tile.GetComponent<Tile> ());
				}
			}
		}
	}

	public void GMIsOn(Tile tile){
		//if crashed
		if (tile.TileContent == TileContent.HOLE && GetComponent<Flyier>() == null && tile.Conflicting){
			GetComponent<HurtTaker>().TakeHurt(tile);
		}
		
		if (tile.TileContent == TileContent.WALL ){
			if (GetComponent<Destroyer>() != null){
				tile.TileContent  = TileContent.NONE;
				Destroy(tile);
			} else if (tile.Conflicting){ 
				GetComponent<HurtTaker>().TakeHurt(tile);
			}
		}
		
		if (tile.TileContent == TileContent.BUFF_OIL){
			
			if (GetComponent<Destroyer>() != null){
				tile.TileContent  = TileContent.NONE;
				Destroy(tile);
			} else  if (GetComponent<Flyier>() == null){ //flying object don't pick up oil buffs
				GetComponent<Car>().PickedUpFuel(tile);
				tile.TileContent = TileContent.NONE;
			}
		}

		if (tile.TileContent == TileContent.COIN) {
			GetComponent<Car>().PickedUpCoin();
			tile.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			tile.TileContent = TileContent.NONE;
		}
	}

	void OnDestroy() {
		if (DriveSound && DriveSound.isPlaying) {
			DriveSound.Stop();
		}
	}

}


