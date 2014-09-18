
public class Wheel : CarStatistic
{
	public float Value;

	override public int UpgradeCost(){
		return 10 * Level;
	}
	
	override public string Info(bool canAffordUpgrade){
		
		string text = "Wheel determines how fast the car is changing lanes.\n";
		if (canAffordUpgrade){
			text += BuyText();
		} else {
			text += NotAffordText();
		}
		
		return text;
	}
	
	override protected string BuyText(){
		return " Do you want to increase wheel value from "+Value+" to "+ValueAfterUpgrade() +" for " + UpgradeCost() + " Coins?";
	}

	private float ValueAfterUpgrade(){
		return Value + 0.25f;
	}

	override protected void InnerUpgrade(){
		Value = ValueAfterUpgrade ();
	}

	override public string TopText(){
		return "Upgrade wheel";
	}

}


