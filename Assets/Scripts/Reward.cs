public class Reward{
	public int Coins;
	public int Exp;
	
	public Reward(int coins, int exp){
		Coins = coins;
		Exp = exp;
	}
	
	public string Description{
		get { return Coins + " Coins, "+Exp+" Exp"; }
	}
	

}
