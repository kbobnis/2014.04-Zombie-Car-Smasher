using UnityEngine;

public class Environment{

	private float _CoinChance;

	public float WallChance = 0.12f;
	public float HoleChance = 0.12f;
	public float BuffFuelChance;
	public int FirstClearStreets = 5;
	public float BuffOilValue = 10;
	public int BuffCoinValue = 1;
	public float CactusChance = 0.05f;

	public float GetCoinChance(int distance){
		float coinChance = _CoinChance + distance / 10000f;
		return coinChance;
	}

	public Environment(float coinChance=0.01f, float buffFuelChance=0.037f){
		_CoinChance = coinChance;
		BuffFuelChance = buffFuelChance;
	}


	public static Environment ClassicMission{
		get { return new Environment (0, 0); }
	}
}
