
public class StartingOil : CarStatistic
{
	public float Value;
	
	override public int UpgradeCost(){
		return Level;
	}
	
	override public string Info(bool canAffordUpgrade){
		
		string text = "Starting oil determines how much oil is in the car at start of the race.\n";
		if (canAffordUpgrade){
			text += BuyText();
		} else {
			text += NotAffordText();
		}
		
		return text;
	}
	
	override protected string BuyText(){
		return " Do you want to increase starting oil value from "+Value+" to "+ValueAfterUpgrade() +" for " + UpgradeCost() + " Coins?";
	}
	
	private float ValueAfterUpgrade(){
		return Value + 1;
	}
	
	override protected void InnerUpgrade(){
		Value = ValueAfterUpgrade ();
	}
	
	override public string TopText(){
		return "Upgrade starting oil";
	}
	
}


