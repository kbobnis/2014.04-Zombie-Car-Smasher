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

	
	void OnGUI(){

		GuiHelper.DrawElement ("images/fuel", 0.01, 0.01, 0.05, 0.98, 0.05 , 0.98 * Amount / MaxAmount, true); 
		GuiHelper.DrawElement ("images/border", 0.01, 0.01, 0.05, 0.98);	
		
	}



}
