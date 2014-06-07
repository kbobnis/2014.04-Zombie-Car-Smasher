using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour {

	public void Prepare(int gamePosX, int gamePosY, float carSpeed, float maxCarSpeed, float shooterProbability, float jumperProbability, float rideCost){

		SpriteRenderer carRenderer = gameObject.AddComponent<SpriteRenderer>();
		carRenderer.sprite = Resources.Load<Sprite>("Images/car");
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
		fuel.Amount = 70;

		gameObject.AddComponent<ActionReceiver>();

		//Animator carAnimator = Resources.Load<Animator>("Images/carAnimator");

		Animator animator = gameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Images/carAnimator");

		gameObject.AddComponent<CarTurner>();

		Accelerator accelerator = gameObject.AddComponent<Accelerator> ();
		accelerator.Prepare (maxCarSpeed, 300);
	}

	public void Update(){
		foreach(KeyValuePair<Fate, bool> keyValue in gameObject.GetComponent<ActionReceiver>().Fates){
			if (keyValue.Key == Fate.FELL_INTO_HOLE ){
				Minigame.Me.GameOver("Fell into hole");
			}
			if (keyValue.Key == Fate.CRASHED_INTO_WALL){
				Minigame.Me.GameOver("Crashed into wall");
			}
		}
	}


}

