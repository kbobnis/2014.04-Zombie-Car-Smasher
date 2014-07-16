using UnityEngine;

public class Fuel : MonoBehaviour {

	private float _Amount;
	public float Amount{
		get { return _Amount; }
		set {
			_Amount = value;
			if (_Amount > 100){
				_Amount = 100;
			}
		}
	}
	public float MaxAmount;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Amount < 0){
			Amount = 0;
			Minigame.Me.GameOver("No fuel");
		}
	}

	void OnGUI(){

		GuiHelper.DrawElement ("images/fuel", 0.041, 0.1, 0.05, 0.55, 0.05 , 0.55 * Amount / MaxAmount, true); 
		GuiHelper.DrawElement ("images/border", 0.041, 0.1, 0.05, 0.55);

		GUI.DrawTexture(new Rect(GuiHelper.PercentW(0.0), GuiHelper.PercentH(0.6), GuiHelper.PercentW(0.15), GuiHelper.PercentH(0.1)), SpriteManager.GetOilTexture());
		
	}



}
