using UnityEngine;
using System.Collections;

public class BeingHit : MonoBehaviour {

	private float SpeedChange = 1.5f;
	private float StartingSpeed;
	private float ActualSpeed;
	private float NextSpeedChange;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (StartingSpeed != 0){
			InGamePosition igp = GetComponent<InGamePosition> ();
			NextSpeedChange += Time.deltaTime * SpeedChange;
			ActualSpeed -= NextSpeedChange;
			igp.y += ActualSpeed * Time.deltaTime;

			if (ActualSpeed < 0.1f){
				Destroy(this);
			}
		}
	}

	public void Prepare(float startingSpeed){
		ActualSpeed = StartingSpeed = startingSpeed * 3;
		NextSpeedChange = 0f;
		GetComponent<Tile> ().Conflicting = false;
	}
}
