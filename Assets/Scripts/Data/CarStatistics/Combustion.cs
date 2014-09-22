using UnityEngine;

public class Combustion : CarStatistic
{
	override public int UpgradeCost(){
		return 10 * Level;
	}

	override protected string Description() {
		return "Combustion determines how much oil will be used to drive one distance.";
	}
	

	override protected string BuyText(){
		return " Do you want to increase combustion value from "+Value+" to "+ValueAfterUpgrade() +" for " + UpgradeCost() + " Coins?";
	}

	private float ValueAfterUpgrade(){
		return Value - 0.1f;
	}

	override protected void InnerUpgrade(){
		SetValue( ValueAfterUpgrade () );
	}

	override public string TopText(){
		return "Upgrade combusion";
	}

}

