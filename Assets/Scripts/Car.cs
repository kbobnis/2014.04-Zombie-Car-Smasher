using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour {

	private int _FuelPickedUpInARow;
	private int _FuelPickedUpInARowBest;
	public int FuelPickedUpThisGame;
	public int FuelPickedUpWhenLow;
	public int TurnsMade;


	public int FuelPickedUpInARow{
		get { return _FuelPickedUpInARow>_FuelPickedUpInARowBest?_FuelPickedUpInARow:_FuelPickedUpInARowBest; }
	}

	void Update(){
	}

	public void Prepare(int gamePosX, int gamePosY, float carSpeed, float maxCarSpeed, float shooterProbability, float jumperProbability, float rideCost){

		SpriteRenderer carRenderer = gameObject.AddComponent<SpriteRenderer>();
		carRenderer.sprite = SpriteManager.GetCar ();
		carRenderer.sortingLayerName = "Layer3";

		float scale = InGamePosition.tileH / carRenderer.bounds.size.y ;
		gameObject.transform.localScale = new Vector3(scale, scale);
		
		Speeder speeder = gameObject.AddComponent<Speeder>();
		speeder.v = carSpeed;
		speeder.RideCost = rideCost;
		
		InGamePosition tmp = gameObject.AddComponent<InGamePosition>();
		tmp.y = gamePosY;
		tmp.x = gamePosX;

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
		fuel.MaxAmount = 100;
		fuel.Amount = 85;

		//Animator carAnimator = Resources.Load<Animator>("Images/carAnimator");

		Animator animator = gameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Images/carAnimator");

		gameObject.AddComponent<CarTurner>();

		Accelerator accelerator = gameObject.AddComponent<Accelerator> ();
		accelerator.Prepare (maxCarSpeed, 300);
	}

	public void PickedUpFuel(float buffOilValue){
		FuelPickedUpThisGame ++;
		_FuelPickedUpInARow ++;
		if (GetComponent<Fuel> ().IsLowFuel ()) {
			FuelPickedUpWhenLow ++;
		}

		GetComponent<Fuel> ().PickedUpFuel (buffOilValue);
	}

	public void JustMovedToAnotherTile(int newY){
		int distance = Minigame.Me.Distance;

		//checking if in the previous tile there wasn't any fuel
		foreach(KeyValuePair<int, GameObject> street in Minigame.Me.Streets){
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
}

