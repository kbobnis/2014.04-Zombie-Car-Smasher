//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public class Shooter : CarBuff {
	
	public float ShootCost = 5;
	private float ShootSpeed = 10;

	public override void DoAction(){

		Minigame.Me.Car.GetComponent<Fuel>().Amount -= ShootCost;

		GameObject g = new GameObject();
		g.name = "bullet";

		SpriteRenderer r = g.AddComponent<SpriteRenderer>(); //to show sprite
		r.sprite = Resources.Load<Sprite>("Images/bullet");
		r.sortingLayerName = "Layer3";
		Vector3 size = r.sprite.bounds.size;
		g.transform.localScale = new Vector3(InGamePosition.tileW / size.x / 3f, InGamePosition.tileH  / size.y / 3f);

		InGamePosition tmp = g.AddComponent<InGamePosition>(); //to update position
		tmp.x = Minigame.Me.Car.GetComponent<InGamePosition>().x;
		tmp.y = Minigame.Me.Car.GetComponent<InGamePosition>().y;

		Speeder speeder = g.AddComponent<Speeder>(); //to have speed
		speeder.v = ShootSpeed;
		speeder.RideCost = 0.1f;
		speeder.DestroyWhenEmpty = true;

		//for speeder to work
		g.AddComponent<Fuel>().Amount = 10; //to have fuel for speeder

		g.AddComponent<Destroyer>(); //to destroy obstacles
		g.AddComponent<Flyier>(); //to not fall into holes
	}

	public override bool CanDoAction(){
		return Minigame.Me.Car.GetComponent<Fuel>().Amount >= ShootCost;
	}
	public override string GetActionName(){
		return "Shoot";
	}

}