using UnityEngine;
using System.Collections.Generic;


public class Speeder : MonoBehaviour 
{
	private AudioSource DriveSound;
	private float Distortion = 0.2f;
	private float LastDistance;

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
		if (LastDistance == 0){
			LastDistance = GetComponent<InGamePosition>().y;
		}

		if (v > 0.5f) {
			if (GetComponent<Fuel> ().HasEnoughFuel ()) {
				GetComponent<InGamePosition> ().y += v * Time.deltaTime * (1 + Random.value * Distortion);
				GetComponent<Fuel> ().ChargeForDistance (GetComponent<InGamePosition> ().y - LastDistance, RideCost);
				LastDistance = GetComponent<InGamePosition> ().y;
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


