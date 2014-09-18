using UnityEngine;
using System.Collections;

public class Accelerator : MonoBehaviour {

	private float AccelerateTo;
	private float DistanceOfAcceleration;
	private float StartingPosition;
	private float StartingSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float actualPos = GetComponent<InGamePosition> ().y;
		float fractionMade = (actualPos - StartingPosition) / (DistanceOfAcceleration - StartingPosition);
		if (fractionMade > 1) {
			fractionMade = 1;
		}
		if (GetComponent<Speeder> ()) {
			GetComponent<Speeder> ().v = StartingSpeed + (AccelerateTo - StartingSpeed) * fractionMade;
		}
	}

	public void Prepare (float accelerateTo, int distanceOfAcceleration){
		AccelerateTo = accelerateTo;
		DistanceOfAcceleration = distanceOfAcceleration;
		StartingPosition = GetComponent<InGamePosition> ().y;
		StartingSpeed = GetComponent<Speeder> ().v;
	}
}
