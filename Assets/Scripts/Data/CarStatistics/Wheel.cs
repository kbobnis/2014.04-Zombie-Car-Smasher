using UnityEngine;

public class Wheel : CarStatistic
{
	override public int UpgradeCost(){
		return 10 * Level;
	}
	
	override protected string Description(){
		return "Wheel determines how fast the car is changing lanes.";
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


