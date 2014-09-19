

public class FuelTank : CarStatistic{
	
	public int Capacity;
	
	override public int UpgradeCost(){
		return 2 * Capacity;
	}
	
	override public string Info(bool canAffordUpgrade){
		
		string text = "Fuel tank lets you gather more fuel at a time.\n";
		if (canAffordUpgrade){
			text += BuyText();
		} else {
			text += NotAffordText();
		}
		
		return text;
	}
	
	override protected string BuyText(){
		return " Do you want to increase fuel tank from "+Capacity+" to "+(Capacity+1) +" for " + UpgradeCost() + " Coins?";
	}
	
	override protected void InnerUpgrade(){
		Capacity ++;
	}
	
	override public string TopText(){
		return "Increase fuel tank";
	}
}

