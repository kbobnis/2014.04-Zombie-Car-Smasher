using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour {

	private int _FuelPickedUpInARow;
	private int _FuelPickedUpInARowBest;
	public int FuelPickedUpThisGame;
	public int FuelPickedUpWhenLow;
	public int TurnsMade;
	public Dictionary<int, GameObject> Streets;
	public int PickedUpCoins;
	public int ShieldsUsed;


	public int FuelPickedUpInARow{
		get { return _FuelPickedUpInARow>_FuelPickedUpInARowBest?_FuelPickedUpInARow:_FuelPickedUpInARowBest; }
	}

	void Update(){
	}

	public void Prepare(CarConfig carConfig, Dictionary<int, GameObject> streets, Mission mission){
		Streets = streets;

		SpriteRenderer carRenderer = gameObject.AddComponent<SpriteRenderer>();
		Texture2D carT = (Texture2D)carConfig.CarTexture;
		carRenderer.sprite = Sprite.Create (carT, new Rect (0, 0, carT.width, carT.height), new Vector2 (0.5f, 0.5f));
		carRenderer.sortingLayerName = "Layer3";

		float scale = InGamePosition.tileH / carRenderer.bounds.size.y ;
		gameObject.transform.localScale = new Vector3(scale, scale);


		if (carConfig.CarStatistics[CarStatisticType.SHIELD].Value > 0) {
			ShieldCompo sc = gameObject.AddComponent<ShieldCompo>();
			sc.Prepare(carConfig.CarStatistics[CarStatisticType.SHIELD]);
		}
	
		Speeder speeder = gameObject.AddComponent<Speeder>();
		speeder.Prepare(carConfig.CarStatistics[CarStatisticType.COMBUSTION], mission.Env.CarStartingSpeed);

		Fuel fuel = gameObject.AddComponent<Fuel>(); 
		fuel.Prepare (carConfig.CarStatistics [CarStatisticType.FUEL_TANK], carConfig.CarStatistics [CarStatisticType.STARTING_OIL]);

		gameObject.AddComponent<InGamePosition>();

		float shooterProbability = 0;
		float jumperProbability = 0;
		float ticket = Random.Range(0, shooterProbability + jumperProbability);

		if (ticket <= jumperProbability){
			Jumper jumper = gameObject.AddComponent<Jumper>();
			jumper.JumpDistance = 2;
			jumper.JumpCost = 4;
		}
		else if (ticket <= jumperProbability + shooterProbability){
			gameObject.AddComponent<Shooter>();
		}

		CarTurner carTurner = gameObject.AddComponent<CarTurner>();
		carTurner.Prepare (carConfig.CarStatistics [CarStatisticType.WHEEL]);

		Accelerator accelerator = gameObject.AddComponent<Accelerator> ();
		accelerator.Prepare (mission.Env.CarMaxSpeed, mission.Env.MaxSpeedDistance);

		gameObject.AddComponent<HurtTaker> ();
	}

	public void PickedUpFuel(Tile fuelTile){
		gameObject.AddComponent<MinigameNotification> ().Prepare("+"+(int)fuelTile.Value+" Fuel", null, Camera.main.WorldToScreenPoint (fuelTile.gameObject.transform.position));
		//mn.Prepare ("Fuel +" + (int)fuelTile.Value, );
		FuelPickedUpThisGame ++;
		_FuelPickedUpInARow ++;
		if (GetComponent<Fuel> ().IsLowFuel ()) {
			FuelPickedUpWhenLow ++;
		}

		GetComponent<Fuel> ().PickedUpFuel ((int)fuelTile.Value);
	}

	public void JustMovedToAnotherTile(int newY){

		//checking if in the previous tile there wasn't any fuel
		foreach(KeyValuePair<int, GameObject> street in Streets){
			if (newY - 1 == Mathf.FloorToInt(street.Value.GetComponent<InGamePosition>().y)){
				foreach(KeyValuePair<int, GameObject> tilePair in street.Value.GetComponent<Street>().Tiles){
					Tile tile = tilePair.Value.GetComponent<Tile>();
					if (tile.TileContent == TileContent.BUFF_OIL){
						if (_FuelPickedUpInARow > _FuelPickedUpInARowBest){
							_FuelPickedUpInARowBest = _FuelPickedUpInARow;
						}
						_FuelPickedUpInARow = 0;
					}
				}
			}
		}
	}

	public void StartedTurning(){
		TurnsMade ++;
	}

	public void PickedUpCoin(Tile coinTile){
		gameObject.AddComponent<MinigameNotification> ().Prepare("+1 Coin", null, Camera.main.WorldToScreenPoint (coinTile.gameObject.transform.position));
		PickedUpCoins ++;
		PlaySingleSound.SpawnSound (Sounds.Coin, gameObject.transform.position);
	}
}

