using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour {

	public int FuelPickedUpThisGame;
	private int FuelPickedUpThisGameInARow;
	public int FuelPickedUpWhenLow;
	private int TurnsMade;

	void Update(){
		CarSmasherSocial.TurnsMade(TurnsMade, Minigame.Me.Distance);
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
		fuel.Amount = 40;

		//Animator carAnimator = Resources.Load<Animator>("Images/carAnimator");

		Animator animator = gameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Images/carAnimator");

		gameObject.AddComponent<CarTurner>();

		Accelerator accelerator = gameObject.AddComponent<Accelerator> ();
		accelerator.Prepare (maxCarSpeed, 300);
	}

	public void PickedUpFuel(float buffOilValue){
		FuelPickedUpThisGame ++;
		FuelPickedUpThisGameInARow ++;
		if (FuelPickedUpThisGame == 5 || FuelPickedUpThisGame == 25 || FuelPickedUpThisGame == 50) {
			CarSmasherSocial.FuelPickedUpInOneGame(FuelPickedUpThisGame);
		}
		GetComponent<Fuel> ().PickedUpFuel (buffOilValue);

		if (FuelPickedUpThisGameInARow == 10 || FuelPickedUpThisGameInARow == 25) {
			CarSmasherSocial.FuelPickedUpInOneGameInARow(FuelPickedUpThisGameInARow);
		}

		if (GetComponent<Fuel> ().IsLowFuel ()) {
			FuelPickedUpWhenLow ++;
			CarSmasherSocial.FuelPickedUpWhenLow(FuelPickedUpWhenLow);
		}
	}

	public void JustMovedToAnotherTile(int newY){
		//checking if in the previous tile there wasn't any fuel
		foreach(KeyValuePair<int, GameObject> street in Minigame.Me.Streets){
			if (newY - 1 == Mathf.FloorToInt(street.Value.GetComponent<InGamePosition>().y)){
				foreach(KeyValuePair<int, GameObject> tilePair in street.Value.GetComponent<Street>().Tiles){
					Tile tile = tilePair.Value.GetComponent<Tile>();
					if (tile.TileContent == TileContent.BUFF_OIL){
						FuelPickedUpThisGameInARow = 0;
					}
				}
			}
		}
	}

	public void StartedTurning(){
		TurnsMade ++;
	}
}

