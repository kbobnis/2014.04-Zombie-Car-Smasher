using UnityEngine;


public class Speeder : MonoBehaviour 
{
	public float v;
	public float RideCost;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float rideCost = Time.deltaTime * RideCost;

		if (GetComponent<Fuel>().Amount > 0 && v > 0){
			GetComponent<InGamePosition>().y += v * Time.deltaTime;
			GetComponent<Fuel>().Amount -= rideCost;
		}
	}

}


