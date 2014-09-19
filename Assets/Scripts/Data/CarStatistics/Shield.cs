using UnityEngine;

public class Shield : CarStatistic{

	private int _Count;

	public int Count{
		get { return _Count; }
		set {
			_Count = value;
			if (_Count < 0) {
				Debug.Log ("Why shield count is lower than zero?!");
			}
		}
	}

	override public int UpgradeCost(){
		return 10;
	}

	override public string Info(bool canAffordUpgrade){

		string text = "Shield destroys an obstacle. One shield is for one use only.\n";
		if (canAffordUpgrade){
			text += BuyText();
		} else {
			text += NotAffordText();
		}

		return text;
	}

	override protected string BuyText(){
		return " Do you want to increase shield value from "+Count+" to "+(Count+1) +" for " + UpgradeCost() + " Coins?";
	}

	override protected void InnerUpgrade(){
		Count ++;
	}

	override public string TopText(){
		return "Upgrade shield";
	}
}
