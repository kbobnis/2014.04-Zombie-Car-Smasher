

abstract public class CarStatistic
{
	public int Level;


	abstract public string TopText();
	abstract protected string BuyText ();
	abstract protected void InnerUpgrade ();
	abstract public int UpgradeCost();
	abstract public string Info (bool canAffordUpgrade);



	protected string NotAffordText(){
		return "You need " + UpgradeCost() +" coins to upgrade this";
	}

	public void Upgrade (){
		Level++;
		InnerUpgrade ();
	}



}

