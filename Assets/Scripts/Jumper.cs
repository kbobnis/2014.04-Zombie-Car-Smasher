//------------------------------------------------------------------------------
using UnityEngine;

public class Jumper : CarBuff {

	public float JumpDistance;
	public float JumpCost;

	public override void DoAction(){
		Flyier flyier = gameObject.AddComponent<Flyier>();
		flyier.Prepare(JumpCost, JumpDistance);

	}

	public override bool CanDoAction(){
		return GetComponent<Flyier>() == null && GetComponent<Fuel>().Amount >= JumpCost;
	}

	public override string GetActionName(){
		return "Jump";
	}

}