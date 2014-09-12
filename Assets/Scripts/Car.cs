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
		if (Minigame.Me != null && (Minigame.Me.Distance == 100 || Minigame.Me.Distance == 200)) { //because there are several achievements which have to be made in the distance 100 or 200
			CarSmasherSocial.UnlockAchievements (new Result[]{
				new Result(SCORE_TYPE.TURNS, TurnsMade),
				new Result(SCORE_TYPE.DISTANCE, Minigame.Me.Distance),
				new Result(SCORE_TYPE.FUEL_PICKED, FuelPickedUpThisGame),
				new Result(SCORE_TYPE.FUEL_PICKED_IN_ROW, FuelPickedUpInARow),
				new Result(SCORE_TYPE.FUEL_PICKED_WHEN_LOW, FuelPickedUpWhenLow)
			});
		}

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

		//this big if is to prevent to much requests to google play
		if (FuelPickedUpThisGame == 5 || 
			FuelPickedUpThisGame == 25 || 
			FuelPickedUpThisGame == 50 || 
			FuelPickedUpInARow == 10 || 
			FuelPickedUpInARow == 25 || 
			FuelPickedUpWhenLow ==1 || 
			FuelPickedUpWhenLow == 3)  {
			CarSmasherSocial.UnlockAchievements(new Result[]{
				new Result(SCORE_TYPE.TURNS, TurnsMade),
				new Result(SCORE_TYPE.DISTANCE, Minigame.Me.Distance),
				new Result(SCORE_TYPE.FUEL_PICKED, FuelPickedUpThisGame),
				new Result(SCORE_TYPE.FUEL_PICKED_IN_ROW, FuelPickedUpInARow),
				new Result(SCORE_TYPE.FUEL_PICKED_WHEN_LOW, FuelPickedUpWhenLow)
			});
		}
		GetComponent<Fuel> ().PickedUpFuel (buffOilValue);

	}

	public void JustMovedToAnotherTile(int newY){
		int distance = Minigame.Me.Distance;
		if (distance == 100 || distance == 250 || distance == 400 || distance == 1000 || distance == 1255) {
			CarSmasherSocial.UnlockAchievements (new Result[]{ new Result (SCORE_TYPE.DISTANCE, distance)});
		}

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
		if ((TurnsMade == 50 || TurnsMade == 75) && Minigame.Me.Distance < 100) {
			CarSmasherSocial.UnlockAchievements (new Result[]{
				new Result(SCORE_TYPE.TURNS, TurnsMade),
				new Result(SCORE_TYPE.DISTANCE, Minigame.Me.Distance)
			});
		}
	}
}

