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

	public void Prepare(CarConfig carConfig, Dictionary<int, GameObject> streets){
		Streets = streets;

		SpriteRenderer carRenderer = gameObject.AddComponent<SpriteRenderer>();
		Texture2D carT = (Texture2D)carConfig.CarTexture;
		carRenderer.sprite = Sprite.Create (carT, new Rect (0, 0, carT.width, carT.height), new Vector2 (0.5f, 0.5f));
		carRenderer.sortingLayerName = "Layer3";

		float scale = InGamePosition.tileH / carRenderer.bounds.size.y ;
		gameObject.transform.localScale = new Vector3(scale, scale);

		if (carConfig.Shield.Value > 0) {
			ShieldCompo sc = gameObject.AddComponent<ShieldCompo>();
			sc.Prepare(carConfig.Shield);
		}
		
		Speeder speeder = gameObject.AddComponent<Speeder>();
		speeder.v = carConfig.StartingCarSpeed;
		speeder.RideCost = carConfig.Combustion.Value;
		
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

		Fuel fuel = gameObject.AddComponent<Fuel>(); 
		fuel.MaxAmount = carConfig.FuelTank.Value;
		fuel.Amount = carConfig.StartingOil.Value;

		CarTurner carTurner = gameObject.AddComponent<CarTurner>();
		carTurner.TurnSpeed = carConfig.Wheel.Value;

		Accelerator accelerator = gameObject.AddComponent<Accelerator> ();
		accelerator.Prepare (carConfig.MaxCarSpeed, 300);

		gameObject.AddComponent<HurtTaker> ();
	}

	public void PickedUpFuel(Tile fuelTile){
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

	public void PickedUpCoin(){
		PickedUpCoins ++;
		PlaySingleSound.SpawnSound (Sounds.Coin, gameObject.transform.position);
	}
}

