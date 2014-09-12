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
		} else {
			Minigame.Me.GameOver(Minigame.OUT_OF_OIL);
		}

		foreach(KeyValuePair<int, GameObject> street in Minigame.Me.Streets){
			if (Mathf.Abs(  gameObject.GetComponent<InGamePosition>().y - street.Value.GetComponent<InGamePosition>().y) < 0.15f){
				Street tmp = street.Value.GetComponent<Street>();
				GameObject tile = tmp.Tiles[Mathf.RoundToInt( gameObject.GetComponent<InGamePosition>().x)];
				tile.GetComponent<Tile>().GMIsOn(gameObject);
			}
		}
	}

	void OnDestroy() {
		if (DriveSound && DriveSound.isPlaying) {
			DriveSound.Stop();
		}
	}

}


