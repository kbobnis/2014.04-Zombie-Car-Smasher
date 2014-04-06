using UnityEngine;

public class Car : MonoBehaviour {

	public void Prepare(int gamePosX, int gamePosY, float carSpeed){
		SpriteRenderer carRenderer = gameObject.AddComponent<SpriteRenderer>();
		carRenderer.sprite = Resources.Load<Sprite>("Images/car");
		carRenderer.sortingLayerName = "Layer3";
		carRenderer.receiveShadows = true;
		carRenderer.castShadows = true;
		
		float scale = InGamePosition.tileH / carRenderer.bounds.size.y ;
		gameObject.transform.localScale = new Vector3(scale, scale);
		
		Speeder speeder = gameObject.AddComponent<Speeder>();
		speeder.v = carSpeed;
		speeder.RideCost = 1f;
		
		InGamePosition tmp = gameObject.AddComponent<InGamePosition>();
		tmp.y = gamePosY;
		tmp.x = gamePosX;

		//Jumper jumper = gameObject.AddComponent<Jumper>();
		//jumper.JumpSpeed = 5;
		//jumper.JumpCost = 4;
		//jumper.JumpHeight = 1.5f;

		Shooter shooter = gameObject.AddComponent<Shooter>();
		shooter.ShootCost = 5;

		Fuel fuel = gameObject.AddComponent<Fuel>();
		fuel.MaxAmount = 100;
		fuel.Amount = 70;

		gameObject.AddComponent<ActionReceiver>();
	}

	public void Update(){
		foreach(Fate fate in gameObject.GetComponent<ActionReceiver>().Fates){
			if (fate == Fate.FELL_INTO_HOLE){
				Minigame.Me.GameOver("Fell into hole");
			}
			if (fate == Fate.CRASHED_INTO_WALL){
				Minigame.Me.GameOver("Crashed into wall");
			}
		}
	}

}

