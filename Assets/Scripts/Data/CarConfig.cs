using System.Collections.Generic;
using UnityEngine;

public class CarConfig
{
	public Texture _CarTexture;
	private float _StartingCarSpeed;
	public float DefaultCarStartingSpeed;
	public float MaxCarSpeed;

	public Shield Shield;
	public Combustion Combustion;
	public Wheel Wheel;
	public StartingOil StartingOil;
	public FuelTank FuelTank;

	public const string MODE_CLASSIC = "classic";
	public const string MODE_ADV = "adventure";

	public CarConfig(string mode){
		Shield = new Shield ();
		Combustion = new Combustion ();
		Wheel = new Wheel ();
		StartingOil = new StartingOil ();
		FuelTank = new FuelTank ();

		switch (mode) {
			case MODE_CLASSIC: {
				DefaultCarStartingSpeed = 5f;
				MaxCarSpeed = 6.5f;
				FuelTank.SetValue( 100 );
				Wheel.SetValue(2f);
				_CarTexture = SpriteManager.GetCar();
				Combustion.SetValue(0.85f);
				StartingOil.SetValue( 85 );
				break;
			}
			case MODE_ADV: {
				DefaultCarStartingSpeed = 5f;
				MaxCarSpeed = 6.5f;
				FuelTank.SetValue( 50 );
				Wheel.SetValue (0.75f);
				Combustion.SetValue(  1.5f );
				StartingOil.SetValue( 35 );
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
		foreach (Result result in afterGameResults) {
			switch(result.ScoreType){
				case SCORE_TYPE.SHIELDS_USED:
					Shield.Used(result.Value);
					break;
			}
		}
	}

	internal string Serialize(){
		Dictionary<string, string> dict = new Dictionary<string, string> ();
		dict ["shield"] = ""+Shield.Serialize();
		dict ["combustion"] = "" + Combustion.Serialize ();
		dict ["wheel"] = "" + Wheel.Serialize();
		dict ["startingOil"] = "" + StartingOil.Serialize ();
		dict ["fuelTank"] = "" + FuelTank.Serialize ();
		return MiniJSON.Json.Serialize(dict);
	}

	internal void Deserialize(string serialized){
		Dictionary<string, object> dict = (Dictionary<string, object>)MiniJSON.Json.Deserialize (serialized);
		Shield.Deserialize((string)dict["shield"]);
		Combustion.Deserialize((string)dict["combustion"]);
		Wheel.Deserialize((string)dict["wheel"]);
		StartingOil.Deserialize((string)dict["startingOil"]);
		FuelTank.Deserialize ((string)dict ["fuelTank"]);
	}
}

