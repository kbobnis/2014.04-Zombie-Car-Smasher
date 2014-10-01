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
	public float CarMaxSpeed ;
	public int MaxSpeedDistance;
	public float _CarStartingSpeed = 4f;

	public Environment(float coinChance=0.0f, float buffFuelChance=0.037f, float wallChance=0.12f, float holeChance=0.12f, float carMaxSpeed=6.5f, int maxSpeedDistance=300){
		_CoinChance = coinChance;
		BuffFuelChance = buffFuelChance;
		WallChance = wallChance;
		HoleChance = holeChance;
		CarMaxSpeed = carMaxSpeed;
		MaxSpeedDistance = maxSpeedDistance;
	}

	public float GetCoinChance(int distance){
		float _2logxlogx =  distance ;
		float coinChance = _CoinChance>0?_CoinChance + _2logxlogx / 30000f:0;
		return coinChance;
	}

	public float CarStartingSpeed{
		get { 
			float speed = _CarStartingSpeed;
			if (Parameter.IsOn(ParameterType.FASTER_START)){
				int bestScore = HighScores.GetTopScoreAll();
				//score speed
				//<100 default -> 4f
				// <200 5f
				// <300 5.5f
				// <400 6f
				if (bestScore > 100){
					speed = 5f;
				}
				if (bestScore > 200){
					speed = 5.5f;
				}
				if (bestScore > 300){
					speed = 6f;
				}
			}

			return speed; 
		} 
	}

	public static Environment ClassicMission{
		get { return new Environment (); }
	}

	public static Environment AdventureMission{
		get { return new Environment (0.02f, 0.037f, 0.12f, 0.12f, 9f, 1100); }
	}
}
