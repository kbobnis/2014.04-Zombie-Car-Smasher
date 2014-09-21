using UnityEngine;

public class Shield : CarStatistic{

	override public int UpgradeCost(){
		return 10 * Level; 
	}

	override protected string Description(){
		return "Shield destroys an obstacle. One shield is for one use only.";
	}


	override protected string BuyText(){
		return " Do you want to increase shield value from "+Value+" to "+(Value+1) +" for " + UpgradeCost() + " Coins?";
	}

	override protected void InnerUpgrade(){
		Value += 1;
	}

	override public string TopText(){
		return "Upgrade shield";
	}
}
