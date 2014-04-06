//------------------------------------------------------------------------------
using UnityEngine;

public class Jumper : MonoBehaviour {

	public float JumpSpeed = 1;

	public void Jump(){
		//you can not
		if (GetComponent<InGamePosition>().z == 0){
			GetComponent<InGamePosition>().z = -2;
		}
	}

	public bool CanJump(){
		return GetComponent<InGamePosition>().z == 0;
	}

	void Update(){
		if (GetComponent<InGamePosition>().z < 0){
			GetComponent<InGamePosition>().z += Time.deltaTime * JumpSpeed;
		}

		if (GetComponent<InGamePosition>().z > 0){
			GetComponent<InGamePosition>().z = 0;
		}
	}

}