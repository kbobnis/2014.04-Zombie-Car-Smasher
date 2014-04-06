//------------------------------------------------------------------------------
using UnityEngine;

public class Jumper : CarBuff {

	public float JumpSpeed ;
	public float JumpCost;
	public float JumpHeight;

	public override void DoAction(){
		//you can not
		if (GetComponent<InGamePosition>().z == 0 && GetComponent<Fuel>().Amount >= JumpCost){
			GetComponent<InGamePosition>().z = -1 * JumpHeight;
			GetComponent<Fuel>().Amount -= JumpCost;
		}
	}

	public override bool CanDoAction(){
		return GetComponent<InGamePosition>().z == 0 && GetComponent<Fuel>().Amount >= JumpCost;
	}

	public override string GetActionName(){
		return "Jump";
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