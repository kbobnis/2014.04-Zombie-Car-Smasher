﻿using UnityEngine;
using System.Collections;

public class HurtTaker : MonoBehaviour {

	public bool HasLost;
	public string LostReason;

	public void TakeHurt(Tile fromWhat){

		ShieldCompo sc = GetComponent<ShieldCompo> ();
		if (sc == null || !sc.TakeThis(fromWhat)){
			Die (fromWhat);
		}

	}

	private void Die(Tile fromWhat){
		//null is when out of oil
		if (fromWhat != null && (fromWhat.TileContent == TileContent.WALL || fromWhat.TileContent == TileContent.HOLE) ) {
			PlaySingleSound.SpawnSound (Sounds.CartonImpact, Camera.main.transform.position);	
		} 
		LostReason = "Hit into obstacle";
		HasLost = true;
	}

	public void OutOfOil(){
		PlaySingleSound.SpawnSound(Sounds.NoMoreFuel, Camera.main.transform.position, 0.4f);
		Die (null);
		LostReason = "Out of fuel";
		HasLost = true;
	}
}
