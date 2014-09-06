using UnityEngine;
using System.Collections;

public class FollowGM : MonoBehaviour {

	public GameObject FollowWhom;
	public Vector3 Offset = new Vector3();
	private float LastDelta;
	private float LastPos;
	private float MaxIncrement = 1.01f ;
	private float MinIncrement = 0.99f;

	void FixedUpdate () {
		if (FollowWhom != null){
			float thisPos = FollowWhom.GetComponent<InGamePosition>().y - Offset.y;
			if (LastPos == 0){
				LastPos = thisPos;
			}
			float thisDelta = thisPos - LastPos;
			if (LastDelta == 0){
				LastDelta = thisDelta;
			}

			if (LastPos != 0 ){
				if (  thisDelta / LastDelta > MaxIncrement){
					thisDelta = LastDelta * (MaxIncrement );//+ (thisDelta / LastDelta - 1)/100) ;
				} else if ( thisDelta / LastDelta < MinIncrement){
					thisDelta = LastDelta * (MinIncrement - (1- thisDelta / LastDelta)/50) ;
				}
			}

			thisPos = LastPos + thisDelta; 
			GetComponent<InGamePosition>().y = thisPos + 2 * Offset.y;
			LastDelta = thisDelta;
			LastPos = thisPos;

		}
	}
}
