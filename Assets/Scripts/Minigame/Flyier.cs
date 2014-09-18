using UnityEngine;


/**
 * Game Object has flyier component if is flying.
 * Especially usefull for not falling into holes.
 * */
public class Flyier : MonoBehaviour
{
	public float JumpCost;
	public float JumpDistance;
	private float JumpStopY;


	public void Prepare(float jumpCost, float jumpDistance){
		GetComponent<Fuel>().Amount -= jumpCost;
		JumpStopY = GetComponent<InGamePosition>().y + jumpDistance;
		if (gameObject.GetComponent<Animator>() != null){
			gameObject.GetComponent<Animator>().ResetTrigger("Land");
			gameObject.GetComponent<Animator>().SetTrigger("StartFlying");
		}
	}


	void Update(){
		if (JumpStopY < GetComponent<InGamePosition>().y){

			if (gameObject.GetComponent<Animator>() != null){
				gameObject.GetComponent<Animator>().ResetTrigger("StartFlying");
				gameObject.GetComponent<Animator>().SetTrigger("Land");
			}
			Destroy(this);
		}

	}
}

