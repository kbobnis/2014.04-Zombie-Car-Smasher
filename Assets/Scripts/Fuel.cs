using UnityEngine;

public class Fuel : MonoBehaviour {
	
	public float Amount;
	public float MaxAmount;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Amount < 0){
			Amount = 0;
		}
	}


}
