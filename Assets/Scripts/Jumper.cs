//------------------------------------------------------------------------------
using UnityEngine;

public class Jumper : MonoBehaviour {

	public float JumpSpeed ;
	public float JumpCost;
	public float JumpHeight;

	public void Jump(){
		//you can not
		if (GetComponent<InGamePosition>().z == 0 && GetComponent<Fuel>().Amount >= JumpCost){
			GetComponent<InGamePosition>().z = -1 * JumpHeight;
			GetComponent<Fuel>().Amount -= JumpCost;
		}
	}

	public bool CanJump(){
		return GetComponent<InGamePosition>().z == 0 && GetComponent<Fuel>().Amount >= JumpCost;
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