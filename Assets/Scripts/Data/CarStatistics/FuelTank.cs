using UnityEngine;

public class FuelTank : CarStatistic{
	
	override public int UpgradeCost(){
		return (int)(Value / 5);
	}
	override protected string Description(){
		return "Fuel tank lets you gather more fuel at a time.";
	}

	override protected string BuyText(){
		return " Do you want to increase fuel tank from "+Value+" to "+(Value+1) +" for " + UpgradeCost() + " Coins?";
	}
	
	override protected void InnerUpgrade(){
		_Value += 1;
	}
	
	override public string TopText(){
		return "Increase fuel tank";
	}
}

