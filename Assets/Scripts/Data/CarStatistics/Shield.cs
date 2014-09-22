using UnityEngine;

public class Shield : CarStatistic{


	public void Used(int howManyTimes){
		if (howManyTimes < 0) {
			throw new UnityException("How many times you have used your shield? " + howManyTimes);
		}
		if (howManyTimes > _Value) {
			throw new UnityException("You used your shield("+_Value+") " + howManyTimes + " times");
		}
		_Value -= howManyTimes;
		Level = (int)_Value+1;
	}

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
		_Value += 1;
	}

	override public string TopText(){
		return "Upgrade shield";
	}
}
