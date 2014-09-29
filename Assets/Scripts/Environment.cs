using UnityEngine;

public class Environment{

	private float _CoinChance;

	public float WallChance;
	public float HoleChance;
	public float BuffFuelChance;
	public int FirstClearStreets = 5;
	public float BuffOilValue = 10;
	public int BuffCoinValue = 1;
	public float CactusChance = 0.05f;

	public float GetCoinChance(int distance){
		float coinChance = _CoinChance>0?_CoinChance + distance / 10000f:0;
		return coinChance;
	}

	public Environment(float coinChance=0.0f, float buffFuelChance=0.037f, float wallChance=0.12f, float holeChance=0.12f){
		_CoinChance = coinChance;
		BuffFuelChance = buffFuelChance;
		WallChance = wallChance;
		HoleChance = holeChance;
	}


	public static Environment ClassicMission{
		get { return new Environment (); }
	}

	public static Environment AdventureMission{
		get { return new Environment (0.02f, 0.02f); }
	}
}
