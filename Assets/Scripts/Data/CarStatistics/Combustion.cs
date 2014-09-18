
public class Combustion : CarStatistic
{
	public float Value;

	override public int UpgradeCost(){
		return 10 * Level;
	}
	
	override public string Info(bool canAffordUpgrade){
		
		string text = "Combustion determines how much oil will be used to drive one distance.\n";
		if (canAffordUpgrade){
			text += BuyText();
		} else {
			text += NotAffordText();
		}
		
		return text;
	}
	
	override protected string BuyText(){
		return " Do you want to increase combustion value from "+Value+" to "+ValueAfterUpgrade() +" for " + UpgradeCost() + " Coins?";
	}

	private float ValueAfterUpgrade(){
		return Value - 0.1f;
	}

	override protected void InnerUpgrade(){
		Value = ValueAfterUpgrade ();
	}

	override public string TopText(){
		return "Upgrade combusion";
	}

}

