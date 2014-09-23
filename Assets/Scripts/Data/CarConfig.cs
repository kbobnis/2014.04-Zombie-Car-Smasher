using System.Collections.Generic;
using UnityEngine;

public class CarConfig
{
	public Texture _CarTexture;
	private float _StartingCarSpeed;
	public float DefaultCarStartingSpeed;
	public float MaxCarSpeed;

	public Dictionary<CarStatisticType, CarStatistic> CarStatistics = new Dictionary<CarStatisticType, CarStatistic>();

	public const string MODE_CLASSIC = "classic";
	public const string MODE_ADV = "adventure";

	public CarConfig(string mode){

		CarStatistic combustion = new CarStatistic (CarStatisticType.COMBUSTION);
		CarStatistic fuelTank = new CarStatistic (CarStatisticType.FUEL_TANK);
		CarStatistic shield = new CarStatistic (CarStatisticType.SHIELD);
		CarStatistic wheel = new CarStatistic (CarStatisticType.WHEEL);
		CarStatistic startingOil = new CarStatistic ( CarStatisticType.STARTING_OIL);
		startingOil.AddDependency (new Dependency (fuelTank, CarStatisticParam.Value, SIGN.BIGGER, startingOil));

		CarStatistics.Add (CarStatisticType.COMBUSTION, combustion);
		CarStatistics.Add (CarStatisticType.FUEL_TANK, fuelTank);
		CarStatistics.Add (CarStatisticType.SHIELD, shield);
		CarStatistics.Add (CarStatisticType.WHEEL, wheel);
		CarStatistics.Add (CarStatisticType.STARTING_OIL, startingOil);

		switch (mode) {
			case MODE_CLASSIC: {
				DefaultCarStartingSpeed = 5f;
				MaxCarSpeed = 6.5f;
				startingOil.Value = 85;
				fuelTank.Value =  100 ;
				wheel.Value = 2f;
				_CarTexture = SpriteManager.GetCar();
				combustion.Value = 0.85f;
				
				break;
			}
			case MODE_ADV: {
				DefaultCarStartingSpeed = 5f;
				MaxCarSpeed = 6.5f;
				_CarTexture = SpriteManager.GetCarAdventure();
				break;
			}
			default:
				throw new UnityException("There is no mode " + mode +" in car config");
		}
	}

	public float StartingCarSpeed{
		get {
			//checking tutorial mode
			List<int> topScores = HighScores.GetTopScores (1);
			float bestScore =  topScores.Count==0?0:topScores[0];
			float carStartingSpeed = DefaultCarStartingSpeed;
			if (bestScore < 200){
				carStartingSpeed = 3 + bestScore/200 * 2;
			}
			return carStartingSpeed;
		}
	}
	
	public Texture CarTexture{
		get { return _CarTexture; }
	}

	public void UpdateCar(Result[] afterGameResults){
	}

	internal string Serialize(){
		Dictionary<string, string> dict = new Dictionary<string, string> ();
		foreach (KeyValuePair<CarStatisticType, CarStatistic> cs in CarStatistics) {
			dict[""+cs.Key.ToString()] = "" + cs.Value.Serialize();
		}
		return MiniJSON.Json.Serialize(dict);
	}

	internal void Deserialize(string serialized){
		Dictionary<string, object> dict = (Dictionary<string, object>)MiniJSON.Json.Deserialize (serialized);
		foreach (KeyValuePair<string, object> kvp in dict) {
			CarStatistic cs = CarStatistic.Deserialize((string)kvp.Value);
			if (cs != null){
				CarStatistics[cs.Type] = cs;
			}
		}

		//dependencies. those car statistics must to be in every carConfig.
		CarStatistic startingOil = CarStatistics[CarStatisticType.STARTING_OIL];
		CarStatistic fuelTank = CarStatistics[CarStatisticType.FUEL_TANK];
		startingOil.AddDependency (new Dependency (fuelTank, CarStatisticParam.Value, SIGN.BIGGER, startingOil));
	}
}

