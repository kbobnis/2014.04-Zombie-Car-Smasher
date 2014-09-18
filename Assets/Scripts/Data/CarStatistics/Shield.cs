

public class Shield : CarStatistic{

	public int Count;

	override public int UpgradeCost(){
		return 10;
	}

	override public string Info(bool canAffordUpgrade){

		string text = "Shield destroys an obstacle and itself when drove into it.\n";
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
