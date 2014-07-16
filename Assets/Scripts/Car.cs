using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour {

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
		fuel.Amount = 70;

		//Animator carAnimator = Resources.Load<Animator>("Images/carAnimator");

		Animator animator = gameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Images/carAnimator");

		gameObject.AddComponent<CarTurner>();

		Accelerator accelerator = gameObject.AddComponent<Accelerator> ();
		accelerator.Prepare (maxCarSpeed, 300);
	}




}

