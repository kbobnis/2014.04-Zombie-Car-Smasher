using System.Collections.Generic;
using UnityEngine;

public class CarConfig
{
	public Texture _CarTexture;
	private float _StartingCarSpeed;
	public float DefaultCarStartingSpeed;
	public float MaxCarSpeed;

	public float MaxFuel;
	public Shield Shield;
	public Combustion Combustion;
	public Wheel Wheel;
	public StartingOil StartingOil;

	public const string MODE_CLASSIC = "classic";
	public const string MODE_ADV = "adventure";

	public CarConfig(string mode){
		Shield = new Shield ();
		Combustion = new Combustion ();
		Wheel = new Wheel ();
		StartingOil = new StartingOil ();

		switch (mode) {
			case MODE_CLASSIC: {
				DefaultCarStartingSpeed = 5f;
				MaxCarSpeed = 6.5f;
				MaxFuel = 100f;
				Wheel.Value =2f;
				_CarTexture = SpriteManager.GetCar();
				Combustion.Value = 0.85f;
				StartingOil.Value = 85;
				break;
			}
			case MODE_ADV: {
				DefaultCarStartingSpeed = 5f;
				MaxCarSpeed = 6.5f;
				MaxFuel = 50f;
				Wheel.Value =1f;
				Wheel.Level = 1;
				Combustion.Value = 1.5f;
				Combustion.Level = 1;
				StartingOil.Value = 35;
				StartingOil.Level = 1;
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
}

